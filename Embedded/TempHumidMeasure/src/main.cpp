#include <Arduino.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <DHT_U.h>
#include <ArduinoMqttClient.h>
#include <WiFiNINA.h>
#include <ArduinoJson.h>

#include "arduino_secrets.h"

#define DHTPIN 2

#define DHTTYPE    DHT11     // DHT 11
//#define DHTTYPE    DHT22     // DHT 22 (AM2302)
//#define DHTTYPE    DHT21     // DHT 21 (AM2301)

DHT_Unified dht(DHTPIN, DHTTYPE);

WiFiSSLClient wifiClient;
MqttClient mqttClient(wifiClient);

char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;

const char broker[] = MQTT_HOST;
int        port     = MQTT_PORT;
const char username[] = MQTT_USERNAME;
const char password[] = MQTT_PASSWORD;

bool fakeWindowState = false;
unsigned long previousWindowState = 0;
const long interval = INTERVAL;
unsigned long previousMillis = 0;
uint32_t delayMS;

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

bool started = false;

void setup() {
  started = false;

  Serial.begin(9600);

  Serial.println("Attempting to connect to Wifi");
  while (WiFi.begin(ssid, pass) != WL_CONNECTED) {
    Serial.print(".");
    delay(5000);
  }

  Serial.println("Connected to wifi!");

  Serial.print("Got IEEE address: ");
  Serial.println(getMacAddressString());
  
  dht.begin();
  Serial.println(F("DHTxx Unified Sensor Example"));
  // Print temperature sensor details.
  sensor_t sensor;
  dht.temperature().getSensor(&sensor);
  Serial.println(F("------------------------------------"));
  Serial.println(F("Temperature Sensor"));
  Serial.print  (F("Sensor Type: ")); Serial.println(sensor.name);
  Serial.print  (F("Driver Ver:  ")); Serial.println(sensor.version);
  Serial.print  (F("Unique ID:   ")); Serial.println(sensor.sensor_id);
  Serial.print  (F("Max Value:   ")); Serial.print(sensor.max_value); Serial.println(F("°C"));
  Serial.print  (F("Min Value:   ")); Serial.print(sensor.min_value); Serial.println(F("°C"));
  Serial.print  (F("Resolution:  ")); Serial.print(sensor.resolution); Serial.println(F("°C"));
  Serial.println(F("------------------------------------"));
  // Print humidity sensor details.
  dht.humidity().getSensor(&sensor);
  Serial.println(F("Humidity Sensor"));
  Serial.print  (F("Sensor Type: ")); Serial.println(sensor.name);
  Serial.print  (F("Driver Ver:  ")); Serial.println(sensor.version);
  Serial.print  (F("Unique ID:   ")); Serial.println(sensor.sensor_id);
  Serial.print  (F("Max Value:   ")); Serial.print(sensor.max_value); Serial.println(F("%"));
  Serial.print  (F("Min Value:   ")); Serial.print(sensor.min_value); Serial.println(F("%"));
  Serial.print  (F("Resolution:  ")); Serial.print(sensor.resolution); Serial.println(F("%"));
  Serial.println(F("------------------------------------"));
  // Set delay between sensor readings based on sensor details.
  delayMS = sensor.min_delay / 1000;

  Serial.println("Connecting to broker:");

  mqttClient.setUsernamePassword(username, password);

  if (!mqttClient.connect(broker, port)) {
    Serial.print("MQTT connection failed! Error code = ");
    Serial.println(mqttClient.connectError());

    while (1);
  }

  Serial.println("Connected to broker!");

  started = true;
}

void loop() {
  if(!started)
  {
    delay(50);
    return;
  }

  mqttClient.poll();

  unsigned long currentMillis = millis();

  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;

    JsonDocument doc;

    // Get temperature event and print its value.
    sensors_event_t event;
    dht.temperature().getEvent(&event);
    if (isnan(event.temperature)) {
      Serial.println(F("Error reading temperature!"));
    }
    else {
      Serial.print(F("Temperature: "));
      Serial.print(event.temperature);
      Serial.println(F("°C"));
      doc["temperature"] = event.temperature;
    }
    // Get humidity event and print its value.
    dht.humidity().getEvent(&event);
    if (isnan(event.relative_humidity)) {
      Serial.println(F("Error reading humidity!"));
    }
    else {
      Serial.print(F("Humidity: "));
      Serial.print(event.relative_humidity);
      Serial.println(F("%"));
      doc["humidity"] = event.relative_humidity;
    }

    if(doc.size() > 0)
    {
      String output;
      serializeJson(doc, output);
  
      mqttClient.beginMessage("zigbee2mqtt/" + getMacAddressString());
      mqttClient.print(output);
      mqttClient.endMessage();
    }

  }


  if (currentMillis - previousWindowState >= (interval*2)) {
    previousWindowState = currentMillis;

    JsonDocument doc;

    fakeWindowState = !fakeWindowState;
    doc["contact"] = fakeWindowState;

    if(doc.size() > 0)
    {
      String output;
      serializeJson(doc, output);
  
      String realMac = getMacAddressString();
      realMac.setCharAt(realMac.length() - 1, '0');

      mqttClient.beginMessage("zigbee2mqtt/" + getMacAddressString());
      mqttClient.print(output);
      mqttClient.endMessage();
    }

  }

}