#include <Arduino.h>
//#include <Adafruit_Fingerprint.h>
#include <fpm.h>
#include <ArduinoJson.h>

#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>
#include <ArduinoMqttClient.h>

#include "arduino_secrets.h"

const char* ssid = "SibrienAP2";
const char* password = "Siberia51244";
const char* serverUrl = "https://jespercal.ngrok.app/api/auth/fingerprint";

const char broker[] = MQTT_HOST;
int        port     = MQTT_PORT;
const char mqtt_username[] = MQTT_USERNAME;
const char mqtt_password[] = MQTT_PASSWORD;

const char* listenerTopic = "zigbee2mqtt/{}/activate";

HardwareSerial fingerSerial(2); // UART2
#define RXD2 16
#define TXD2 17

//Adafruit_Fingerprint finger = Adafruit_Fingerprint(&fingerSerial);
FPM finger(&fingerSerial);
FPMSystemParams params;

WiFiClientSecure client;
WiFiClientSecure client2;

MqttClient mqttClient(client);

String macAddress = "";

String getMacAddressString() {
  if(macAddress == "")
  {
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


bool isSetup = false;

void printHex(int num, int precision);
bool downloadFingerprintImage();
uint32_t imageToBuffer(void);
void sendBufferToAPI(size_t bufferLen);
void sendExampleBufferToAPI();
void onMqttMessage(int messageSize);

void setup() {
  isSetup = false;

  Serial.begin(9600);

  while(!Serial)
  {
    ;
  }

  WiFi.begin(ssid, password);
  Serial.println("Connecting to WiFi...");

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  
  Serial.println("");
  Serial.println("WiFi connected");

  Serial.print("Address: ");
  Serial.println(getMacAddressString());

  fingerSerial.begin(57600, SERIAL_8N1, RXD2, TXD2);
  //finger.begin(57600);

  Serial.println("Before begin");
  if (finger.begin()) {
    Serial.println("After begin");
    finger.readParams(&params);
    Serial.println("After params");
    Serial.println("Found fingerprint sensor!");
    Serial.print("Capacity: "); Serial.println(params.capacity);
    Serial.print("Packet length: "); Serial.println(FPM::packetLengths[static_cast<uint8_t>(params.packetLen)]);
  } 
  else {
    Serial.println("Did not find fingerprint sensor :(");
    while (1) yield();
  }

  client.setInsecure();
  client2.setInsecure();

  mqttClient.setUsernamePassword(mqtt_username, mqtt_password);

  if (!mqttClient.connect(broker, port)) {
    Serial.print("MQTT connection failed! Error code = ");
    Serial.println(mqttClient.connectError());

    while (1);
  }

  mqttClient.onMessage(onMqttMessage);

  String topic = listenerTopic;
  topic.replace("{}", getMacAddressString());

  Serial.print("Topic: ");
  Serial.println(topic);

  mqttClient.subscribe(topic);

  isSetup = true;
}

int userId;
void onMqttMessage(int messageSize) {
  // we received a message, print out the topic and contents
  Serial.println("Received a message with topic '");
  Serial.print(mqttClient.messageTopic());
  Serial.print("', length ");
  Serial.print(messageSize);
  Serial.println(" bytes:");

  // use the Stream interface to print the contents
  /*while (mqttClient.available()) {
    Serial.print((char)mqttClient.read());
  }*/

  JsonDocument doc;
  deserializeJson(doc, mqttClient);

  //if(doc.containsKey("user_id"))
  if(doc["user_id"].is<int>())
  {
    userId = (int)doc["user_id"];
    imageToBuffer();
  }

  Serial.println();
}

void loop() {
  if(!isSetup) {
    return;
  }

  mqttClient.poll();

  //imageToBuffer();

  //delay(100);

  //isSetup = false;
}

#define IMAGE_SZ	36864UL
#define PRINTF_BUF_SZ   60
char printfBuf[PRINTF_BUF_SZ];
uint8_t imageBuffer[IMAGE_SZ];

void printHex(int num, int precision) {
  char tmp[16];
  char format[128];

  sprintf(format, "%%.%dX", precision);

  sprintf(tmp, format, num);
  Serial.print(tmp);
}

uint32_t imageToBuffer(void)
{
    FPMStatus status;

    Serial.print("Starting scan for user: ");
    Serial.println(userId);
    Serial.println();
    
    /* Take a snapshot of the finger */
    Serial.println("\r\nPlace a finger.");
    
    do {
        status = finger.getImage();
        
        switch (status) 
        {
            case FPMStatus::OK:
                Serial.println("Image taken.");
                break;
                
            case FPMStatus::NOFINGER:
                Serial.println(".");
                break;
                
            default:
                /* allow retries even when an error happens */
                snprintf(printfBuf, PRINTF_BUF_SZ, "getImage(): error 0x%X", static_cast<uint16_t>(status));
                Serial.println(printfBuf);
                break;
        }
        
        yield();
    }
    while (status != FPMStatus::OK);
    
    /* Initiate the image transfer */
    status = finger.downloadImage();
    
    switch (status) 
    {
        case FPMStatus::OK:
            Serial.println("Starting image stream...");
            break;
            
        default:
            snprintf(printfBuf, PRINTF_BUF_SZ, "downloadImage(): error 0x%X", static_cast<uint16_t>(status));
            Serial.println(printfBuf);
            return 0;
    }
    
    uint32_t totalRead = 0;
    uint16_t readLen = IMAGE_SZ;
    
    bool readComplete = false;

    while (!readComplete) 
    {
        bool ret = finger.readDataPacket(imageBuffer + totalRead, NULL, &readLen, &readComplete);
        
        if (!ret)
        {
            snprintf_P(printfBuf, PRINTF_BUF_SZ, PSTR("readDataPacket(): failed after reading %u bytes"), totalRead);
            Serial.println(printfBuf);
            return 0;
        }
        
        totalRead += readLen;
        readLen = IMAGE_SZ - totalRead;
        
        yield();
    }

    Serial.println();
    Serial.print(totalRead); Serial.println(" bytes transferred.");

    sendBufferToAPI(totalRead);

    return totalRead;
}


void sendBufferToAPI(size_t bufferLen) {
  if ((WiFi.status() == WL_CONNECTED)) {

    HTTPClient http;

    Serial.println("[HTTP] Begin...");
    if(http.begin(client2, "10.130.0.151", 7265, "/api/auth/fingerprint", true))
    {
      http.addHeader("Content-Type", "application/octet-stream");

      int httpResponseCode = http.POST(imageBuffer, sizeof(imageBuffer));

      Serial.print("[HTTP] Response code: ");
      Serial.println(httpResponseCode);

      if (httpResponseCode > 0) {
        String response = http.getString();
        Serial.println("[HTTP] Response payload: " + response);
      }

      http.end();

    }
  } else {
    Serial.println("WiFi not connected");
  }
}