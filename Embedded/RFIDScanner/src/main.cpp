#include <Arduino.h>
#include <WiFiNINA.h>
#include <ArduinoJson.h>
#include <MFRC522.h>
#include <ArduinoHttpClient.h>

#include "arduino_secrets.h"

#define SS_PIN 10
#define RST_PIN 9

char serverAddress[] = "https://jespercal.ngrok.app";
int port = 443;
ulong stateChanged = 0;

char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;
int state = 0;
String macAddress = "";
bool started = false;

WiFiSSLClient wifiClient;
HttpClient client = HttpClient(wifiClient, serverAddress, port);
MFRC522 mfrc522(SS_PIN, RST_PIN);

void requestApi( String rfid );
void SetState( int newState );

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

  Serial.begin(9600);

  Serial.println("Attempting to connect to Wifi");
  while (WiFi.begin(ssid, pass) != WL_CONNECTED) {
    Serial.print(".");
    delay(5000);
  }

  Serial.println("Connected to wifi!");

  Serial.print("Got IEEE address: ");
  Serial.println(getMacAddressString());

  Serial.println("Setting up SPI and MFRC522...");

  SPI.begin();
  mfrc522.PCD_Init();

  Serial.println("Setup completed\n");

  started = true;
}

void loop() {
  if(!started)
  {
    delay(50);
    return;
  }

  if(state != 0)
  {
    if((state < 0 || state > 1) && (stateChanged + 2000) >= millis())
    {
      SetState(0);
    }

    return;
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

  doc["rfid"] = rfid.c_str();

  String output;
  serializeJson(doc, output);

  // POST request
  String contentType = "application/json";

  client.beginRequest();
  client.post("/api/access/card");
  client.sendHeader("Content-Type", "application/json");
  client.sendHeader("Content-Length", contentType.length());
  client.sendHeader("ngrok-skip-browser-warning", "yes");
  client.beginBody();
  client.print(contentType);
  client.endRequest();

  int statusCode = client.responseStatusCode();
  String response = client.responseBody();

  Serial.print("Status code: ");
  Serial.println(statusCode);
  Serial.print("Response: ");
  Serial.println(response);

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