/*
 * ArduinoAsDevice firmware
 *
 * Protocollo seriale testuale (un comando per riga, terminato da '\n').
 * Comandi (PC -> Arduino):
 *   PING                              -> PONG <version>
 *   MODE <pin> <IN|OUT|IN_PULLUP>     -> OK | ERR <msg>
 *   DWRITE <pin> <0|1>                -> OK | ERR <msg>
 *   DREAD <pin>                       -> D <pin> <0|1>
 *   AWRITE <pin> <0-255>              -> OK | ERR <msg>   (PWM duty = ampiezza)
 *   AREAD <pin>                       -> A <pin> <0-1023>
 *   TONE <pin> <freq_hz>              -> OK (freq 0 = stop)  (frequenza regolabile)
 *   STREAM <A|D> <pin> <interval_ms>  -> OK, poi invia "S <pin> <value>" ogni interval
 *   STREAM STOP                       -> OK
 *
 * Compatibile con schede AVR generiche (Uno, Nano, Mega, Leonardo...).
 */

#define FW_VERSION "1.0.0"
#define BAUD_RATE 115200
#define LINE_BUF 64

static char lineBuf[LINE_BUF];
static uint8_t lineLen = 0;

// Stato streaming (un solo pin alla volta)
static bool streamActive = false;
static bool streamAnalog = false;
static uint8_t streamPin = 0;
static unsigned long streamInterval = 100;
static unsigned long streamLast = 0;

void setup() {
  Serial.begin(BAUD_RATE);
  while (!Serial) { ; } // necessario per Leonardo/Micro
}

void loop() {
  while (Serial.available() > 0) {
    char c = (char)Serial.read();
    if (c == '\r') continue;
    if (c == '\n') {
      lineBuf[lineLen] = '\0';
      handleLine(lineBuf);
      lineLen = 0;
    } else if (lineLen < LINE_BUF - 1) {
      lineBuf[lineLen++] = c;
    } else {
      lineLen = 0; // overflow: scarta
    }
  }

  if (streamActive) {
    unsigned long now = millis();
    if (now - streamLast >= streamInterval) {
      streamLast = now;
      int v = streamAnalog ? analogRead(streamPin) : digitalRead(streamPin);
      Serial.print(F("S "));
      Serial.print(streamPin);
      Serial.print(' ');
      Serial.println(v);
    }
  }
}

static void ok() { Serial.println(F("OK")); }
static void err(const char* msg) { Serial.print(F("ERR ")); Serial.println(msg); }

void handleLine(char* line) {
  // tokenizza in-place
  char* tok[5];
  uint8_t n = 0;
  char* p = strtok(line, " ");
  while (p != NULL && n < 5) { tok[n++] = p; p = strtok(NULL, " "); }
  if (n == 0) return;

  if (strcmp(tok[0], "PING") == 0) {
    Serial.print(F("PONG ")); Serial.println(F(FW_VERSION));
    return;
  }

  if (strcmp(tok[0], "MODE") == 0 && n == 3) {
    int pin = atoi(tok[1]);
    if (strcmp(tok[2], "IN") == 0) pinMode(pin, INPUT);
    else if (strcmp(tok[2], "OUT") == 0) pinMode(pin, OUTPUT);
    else if (strcmp(tok[2], "IN_PULLUP") == 0) pinMode(pin, INPUT_PULLUP);
    else { err("bad mode"); return; }
    ok(); return;
  }

  if (strcmp(tok[0], "DWRITE") == 0 && n == 3) {
    digitalWrite(atoi(tok[1]), atoi(tok[2]) ? HIGH : LOW);
    ok(); return;
  }

  if (strcmp(tok[0], "DREAD") == 0 && n == 2) {
    int pin = atoi(tok[1]);
    Serial.print(F("D ")); Serial.print(pin);
    Serial.print(' '); Serial.println(digitalRead(pin));
    return;
  }

  if (strcmp(tok[0], "AWRITE") == 0 && n == 3) {
    int val = constrain(atoi(tok[2]), 0, 255);
    analogWrite(atoi(tok[1]), val);
    ok(); return;
  }

  if (strcmp(tok[0], "AREAD") == 0 && n == 2) {
    int pin = atoi(tok[1]);
    Serial.print(F("A ")); Serial.print(pin);
    Serial.print(' '); Serial.println(analogRead(pin));
    return;
  }

  if (strcmp(tok[0], "TONE") == 0 && n == 3) {
    int pin = atoi(tok[1]);
    long freq = atol(tok[2]);
    if (freq <= 0) noTone(pin);
    else tone(pin, (unsigned int)freq);
    ok(); return;
  }

  if (strcmp(tok[0], "STREAM") == 0) {
    if (n == 2 && strcmp(tok[1], "STOP") == 0) {
      streamActive = false;
      ok(); return;
    }
    if (n == 4) {
      streamAnalog = (tok[1][0] == 'A');
      streamPin = (uint8_t)atoi(tok[2]);
      streamInterval = atol(tok[3]);
      if (streamInterval == 0) streamInterval = 1;
      streamLast = millis();
      streamActive = true;
      ok(); return;
    }
    err("bad stream"); return;
  }

  err("unknown cmd");
}
