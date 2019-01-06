#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include "MPU9250.h"

// Wi-Fi connection variables
const char* ssid = "ssw";
const char* password = "ssw112233";
boolean wifiConnected = false;
String macAdr;

// UDP variables
unsigned int localPort = 8888;
unsigned int UDPPort = 6000;
IPAddress UDPBroadcast(192, 168, 0, 255);
WiFiUDP UDP;
boolean udpConnected = false;
char packetBuffer[UDP_TX_PACKET_MAX_SIZE];

// an MPU9250 object with the MPU-9250 sensor on I2C bus 0 with address 0x68
MPU9250 IMU(Wire, 0x68);
int status;
float sensorVal = 0;
float EMA_A = 0.6;
float EMA_S = 0;
float isMoving = false;

// variables
char ReplyBuffer[] = "IMU UDP acknowledge";

void setup() {
  Serial.begin(115200);
  while(!Serial) {}
  
  wifiConnected = connectWiFi();

  if (wifiConnected) {
    udpConnected = connectUDP();
    if (udpConnected) {
      // IMU
      status = IMU.begin();
      if (status < 0) {
        Serial.println("IMU initialization unsuccessful");
        Serial.println("Check IMU wiring or try cycling power");
        Serial.print("Status: ");
        Serial.println(status);
        while(1) {}
      }
      
      // PIN Setup
      pinMode(BUILTIN_LED, OUTPUT);
      pinMode(D3, OUTPUT);
      pinMode(D7, OUTPUT);
    }
  }
}

void loop() {
  if (wifiConnected) {
    if (udpConnected) {
      int packetSize = UDP.parsePacket();
      if (packetSize) {
        Serial.print("Received packet of size ");
        Serial.print(packetSize);
        Serial.print("From ");
        IPAddress remote = UDP.remoteIP();
        for (int i = 0; i < 4; i++) {
          Serial.print(remote[i], DEC);
          if (i < 3) {
            Serial.print(".");
          }
        }
        Serial.print(", port ");
        Serial.println(UDP.remotePort());

        // read the packet into packetBuffer
        UDP.read(packetBuffer, UDP_TX_PACKET_MAX_SIZE);
        Serial.println("Contents: ");
        Serial.println(packetBuffer);

        // send a reply to the IP address and port that sent us the packet we received
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(ReplyBuffer);
        UDP.endPacket();
      }

      // Read the IMU sensor
      IMU.readSensor();
      sensorVal = IMU.getGyroZ_rads();
      EMA_S = (EMA_A * sensorVal) + ((1 - EMA_A) * EMA_S);

      Serial.print(sensorVal);
      Serial.print("\t");
      Serial.println(EMA_S);
      
      if (EMA_S < -0.4 && !isMoving) {
        isMoving = true;
        digitalWrite(D7, 1);
        digitalWrite(BUILTIN_LED, 1);
        digitalWrite(D3, 0);
        char sendBuffer[] = "imuReverse";
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(sendBuffer);
        UDP.endPacket();
      } else if (EMA_S >= -0.04 && EMA_S <= 0.4 && isMoving) {
        isMoving = false;
        digitalWrite(D7, 0);
        digitalWrite(BUILTIN_LED, 0);
        digitalWrite(D3, 0);
        char sendBuffer[] = "imuIdle";
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(sendBuffer);
        UDP.endPacket();
      } else if (EMA_S > 0.4 && !isMoving) {
        isMoving = true;
        digitalWrite(D7, 0);
        digitalWrite(BUILTIN_LED, 1);
        digitalWrite(D3, 1);
        char sendBuffer[] = "imuObverse";
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(sendBuffer);
        UDP.endPacket();
      }

      // interaction
      
    }
  }

  delay(100);
}


/*
   Connect to Wi-Fi and UDP
*/

// connect to UDP - return true if successful or false if not
boolean connectUDP() {
  boolean state = false;

  Serial.print("");
  Serial.println("Connecting to UDP...");

  if (UDP.begin(localPort) == 1) {
    Serial.println("UDP Connection successful.");
    state = true;
  } else {
    Serial.println("UDP Connection failed.");
  }
  return state;
}

// connect to Wi-Fi - return true if successful or false if not
boolean connectWiFi() {
  boolean state = true;
  int i = 0;
  WiFi.begin(ssid, password);

  Serial.println("");
  Serial.println("Connecting to WiFi...");

  // Wait for connect to Wi-Fi
  Serial.print("Connecting");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
    if (i > 10) {
      state = false;
      break;
    }
    i++;
  }

  if (state) {
    Serial.println("");
    Serial.print("Connected to ");
    Serial.println(ssid);
    Serial.print("IP addredd: ");
    Serial.println(WiFi.localIP());
    Serial.printf("Chip ID = %08X\n", ESP.getChipId());
    Serial.print("MAC addredd: ");
    macAdr = WiFi.macAddress();
    Serial.println(macAdr);
  } else {
    Serial.println("");
    Serial.println("WiFi Connection failed.");
  }
  return state;
}
