#include <Arduino.h>

#include <ArduinoJson.h>

#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>
#include <ArduinoMqttClient.h>

#include <ESP32Servo.h>

#include "arduino_secrets.h"

static const int servoPin = 13;

Servo servo1;

const char* ssid = "SibrienAP2";
const char* password = "Siberia51244";
const char* serverUrl = "https://jespercal.ngrok.app/api/auth/fingerprint";

const char broker[] = MQTT_HOST;
int        port     = MQTT_PORT;
const char mqtt_username[] = MQTT_USERNAME;
const char mqtt_password[] = MQTT_PASSWORD;

const char* listenerTopic = "zigbee2mqtt/{}/activate";

WiFiClientSecure client;
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

  client.setInsecure();

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

  String message = "";
  JsonDocument doc;
  doc["type"] = "smart-lock";
  doc["address"] = macAddress;
  
  serializeJson(doc, message);

  mqttClient.beginMessage(topic);
  mqttClient.print(message);
  mqttClient.endMessage();

  servo1.attach(servoPin);

  servo1.write(0);

  delay(2000);

  isSetup = true;
}

int duration;
void onMqttMessage(int messageSize) {
  // we received a message, print out the topic and contents
  Serial.println("Received a message with topic '");
  Serial.print(mqttClient.messageTopic());
  Serial.print("', length ");
  Serial.print(messageSize);
  Serial.println(" bytes:");

  JsonDocument doc;
  deserializeJson(doc, mqttClient);

  duration = 2000;

  if(doc["duration"].is<int>())
  {
    duration = (int)doc["duration"];
  }

  Serial.println("Opening");
  servo1.write(45);
  delay(duration);

  Serial.println("Closing");
  servo1.write(0);

  Serial.println("Done\n");
}

void loop() {
  if(!isSetup) {
    return;
  }

  mqttClient.poll();
}