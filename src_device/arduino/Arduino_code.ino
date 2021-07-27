const int ledPin = 11;
int ledState = LOW;
unsigned long previousMillis = 0;
const long interval = 500;

void setup()
{
Serial.begin(9600);
pinMode(ledPin,OUTPUT);
}

void loop()
{
    Serial.println(analogRead(A0));
    delay(1);
    unsigned long currentMillis = millis();
    if (currentMillis - previousMillis >= interval)
    {
      previousMillis = currentMillis;
      if (ledState == LOW)
      {
        ledState = HIGH;
      } else
      {
        ledState = LOW;
      }
      digitalWrite(ledPin,ledState);
    }
 }
