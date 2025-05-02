#include <Arduino.h>
#include "WiFiS3.h"
#include "WiFiSSLClient.h"
#include "IPAddress.h"
#include <ArduinoJson.h>
#include <MFRC522.h>
#include <ArduinoMqttClient.h>
#include "arduino_secrets.h"

#define SS_PIN 10
#define RST_PIN 9

int status = WL_IDLE_STATUS;
long stateChanged = 0;

char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;
int state = 0;
String macAddress = "";
bool started = false;

const char broker[] = MQTT_HOST;
int        port     = MQTT_PORT;
const char mqtt_username[] = MQTT_USERNAME;
const char mqtt_password[] = MQTT_PASSWORD;

String listenerTopic = "zigbee2mqtt/{}";

String activateTopic;
String startRegisterTopic;
String cancelRegisterTopic;

WiFiSSLClient wifiClient;
MFRC522 mfrc522(SS_PIN, RST_PIN);

MqttClient mqttClient(wifiClient);

void requestApi( String rfid );
void SetState( int newState );
void onMqttMessage( int messageSize );


bool doRegisterCard = false;
int registerToUser = 0;
long registerStartedAt = 0;
String registerRFID = "";



String getMacAddressString() {
  if(macAddress == "") {
    byte mac[6];
    WiFi.macAddress(mac);
    String macStr = "0x";
    for (int i = 0; i < 6; i++) {
      if (mac[i] < 16) macStr += "0";
      macStr += String(mac[i], HEX);
    }
    macStr.toLowerCase();
    macAddress = macStr;
  }
  return macAddress;
}

void setup() {
  started = false;
  state = 0;

  //Initialize serial and wait for port to open:
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }

  // check for the WiFi module:
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");
    // don't continue
    while (true);
  }

  String fv = WiFi.firmwareVersion();
  Serial.println(fv);
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Please upgrade the firmware");
  }

  // attempt to connect to WiFi network:
  while (status != WL_CONNECTED) {
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    // Connect to WPA/WPA2 network.
    status = WiFi.begin(ssid, pass);

    // wait 10 seconds for connection:
    delay(10000);
  }

  Serial.println("Connected to wifi!");

  Serial.print("Got IEEE address: ");
  Serial.println(getMacAddressString());

  Serial.println("Setting up SPI and MFRC522...");

  SPI.begin();
  mfrc522.PCD_Init();

  Serial.println("SPI and MFRC522 setup!");

  mqttClient.setUsernamePassword(mqtt_username, mqtt_password);

  if (!mqttClient.connect(broker, port)) {
    Serial.print("MQTT connection failed! Error code = ");
    Serial.println(mqttClient.connectError());

    while (1);
  }

  mqttClient.onMessage(onMqttMessage);

  String topic = listenerTopic;
  topic.replace("{}", getMacAddressString());

  startRegisterTopic = topic + "/add_card";
  activateTopic = topic + "/activate";
  cancelRegisterTopic = topic + "/cancel_card";

  Serial.print("Topic: ");
  Serial.println(topic);

  mqttClient.subscribe(listenerTopic, 2);
  mqttClient.subscribe(startRegisterTopic, 2);
  mqttClient.subscribe(cancelRegisterTopic, 2);
  Serial.println("Subscribing to:");
  Serial.println(listenerTopic);
  Serial.println(startRegisterTopic);
  Serial.println(cancelRegisterTopic);

  String message = "";
  JsonDocument doc;
  doc["type"] = "rfid-scanner";
  doc["address"] = macAddress;
  
  serializeJson(doc, message);

  mqttClient.beginMessage(topic);
  mqttClient.print(message);
  mqttClient.endMessage();

  Serial.println("Setup completed\n");

  started = true;
}

void ResetRegisterCard()
{
  doRegisterCard = false;
  registerToUser = 0;
  registerStartedAt = 0;
  registerRFID = "";
}

void loop() {
  if(!started)
  {
    delay(50);
    return;
  }

  mqttClient.poll();

  if(state != 0)
  {
    if((state < 0 || state > 1) && millis() >= (stateChanged + 3000))
    {
      SetState(0);
    }

    return;
  }

  if(doRegisterCard && millis() > (registerStartedAt + 10000))
  {
    ResetRegisterCard();
  }

  if(!mfrc522.PICC_IsNewCardPresent()) {
    return;
  }

  if(!mfrc522.PICC_ReadCardSerial()) {
    return;
  }

  // Card registered
  SetState(1);

  Serial.print("UID tag :");
  String content = "";
  for (byte i = 0; i < mfrc522.uid.size; i++) 
  {
     Serial.print(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " ");
     Serial.print(mfrc522.uid.uidByte[i], HEX);
     content.concat(String(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " "));
     content.concat(String(mfrc522.uid.uidByte[i], HEX));
  }
  Serial.println();
  Serial.print("Message : ");
  content.toUpperCase();

  requestApi(content);
}

void requestApi( String rfid )
{
  Serial.println("Make POST request");

  JsonDocument doc;

  rfid.trim();

  doc["rfid"] = rfid.c_str();

  if(doRegisterCard)
  {
    doc["command"] = "register_card";
    doc["user_id"] = registerToUser;

    ResetRegisterCard();
  }
  else
  {
    doc["command"] = "grant_access";
  }

  String output;
  serializeJson(doc, output);

  Serial.println("Sending");

  mqttClient.beginMessage("zigbee2mqtt/" + macAddress);
  mqttClient.print(output);
  mqttClient.endMessage();

  Serial.println("Sent");

  int statusCode = 200;

  if(statusCode >= 200 && statusCode < 300)
  {
    SetState(2);
    return;
  }
  else if(statusCode >= 400 && statusCode < 500)
  {
    SetState(-1);
    return;
  }
  else if(statusCode >= 500)
  {
    SetState(-2);
    return;
  }

  SetState(-1);
}

void SetState( int newState )
{
  stateChanged = millis();
  state = newState;
}


void onMqttMessage(int messageSize)
{
  // we received a message, print out the topic and contents
  Serial.print("Received a message with topic '");
  Serial.print(mqttClient.messageTopic());
  Serial.print("', length ");
  Serial.print(messageSize);
  Serial.print(" bytes: ");

  String topic = mqttClient.messageTopic();

  JsonDocument doc;
  deserializeJson(doc, mqttClient);

  serializeJson(doc, Serial);

  Serial.println();

  if(topic == startRegisterTopic)
  {
    registerStartedAt = millis();
    registerToUser = doc["user_id"];

    doRegisterCard = true;

    Serial.println("Ready to register access card");
  }
  
  if(topic == cancelRegisterTopic && doRegisterCard)
  {
    ResetRegisterCard();

    Serial.println("Register card cancelled");
  }
}