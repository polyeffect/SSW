/*
 * 1, 4, 7, 10
 * 13, 16, 2, 5
 * 8, 11, 14, 3
 * 6, 9, 12, 15
 */

#include <ESP8266WiFi.h>
#include <WiFiUDP.h>

// wi-fi connection variables
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

// variables
char ReplyBuffer[] = "D1 MINI ORANGE";
int val1 = 0;
int val2 = 0;
int val3 = 0;
int val4 = 0;
char sendBuffer1[] = "i06";
char sendBuffer2[] = "i09";
char sendBuffer3[] = "i12";
char sendBuffer4[] = "i15";

void setup() {
  Serial.begin(115200);
  wifiConnected = connectWifi();

  if (wifiConnected) {
    udpConnected = connectUDP();
    if (udpConnected) {
      // PIN setup
      pinMode(BUILTIN_LED, OUTPUT);
      pinMode(D5, INPUT);
      pinMode(D6, INPUT);
      pinMode(D7, INPUT);
      pinMode(D8, INPUT);
    }
  }
}

void loop() {
  if (wifiConnected) {
    if (udpConnected) {
      int packetSize = UDP.parsePacket();
      if (packetSize) {
        Serial.print("Received packet of size ");
        Serial.println(packetSize);
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

        // read the packet into packetBufffer
        UDP.read(packetBuffer, UDP_TX_PACKET_MAX_SIZE);
        Serial.println("Contents:");
        Serial.println(packetBuffer);

        // send a reply to the IP address and port that sent us the packet we received
        //UDP.beginPacket(UDP.remoteIP(), UDPPort);
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(ReplyBuffer);
        UDP.endPacket();
      }

      // interaction
      // button
      val1 = digitalRead(D5);
      val2 = digitalRead(D6);
      val3 = digitalRead(D7);
      val4 = digitalRead(D8);

      if (val1 == 1) {
        digitalWrite(BUILTIN_LED, val1);
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(sendBuffer1);
        UDP.endPacket();
      }

      if (val2 == 1) {
        digitalWrite(BUILTIN_LED, val2);
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(sendBuffer2);
        UDP.endPacket();
      }

      if (val3 == 1) {
        digitalWrite(BUILTIN_LED, val3);
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(sendBuffer3);
        UDP.endPacket();
      }

      if (val4 == 1) {
        digitalWrite(BUILTIN_LED, val4);
        UDP.beginPacket(UDPBroadcast, UDPPort);
        UDP.write(sendBuffer4);
        UDP.endPacket();
      }

      if (val1 ==0 && val2 == 0 && val3 == 0 && val4 == 0) digitalWrite(BUILTIN_LED, 0);
    }
  }
  delay(200);
}


/*
   connect to wifi and UDP
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

// connect to wifi - return true if successful or false if not
boolean connectWifi() {
  boolean state = true;
  int i = 0;
  WiFi.begin(ssid, password);
  
  Serial.println("");
  Serial.println("Connecting to WiFi...");

  // Wait for connection
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
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());
    Serial.printf("Chip ID = %08X\n", ESP.getChipId());
    Serial.print("MAC address: ");
    macAdr = WiFi.macAddress();
    Serial.println(macAdr);
  } else {
    Serial.println("");
    Serial.println("WiFi Connection failed.");
  }
  return state;
}
