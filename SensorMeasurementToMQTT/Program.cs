using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SensorMeasurementToMQTT
{
    class Program
    {

        MqttClient client;
        Random random;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }


        public void Run()
        {
            random = new Random();

            while(true)
            {
                try
                {
                    if (client == null)
                    
                    {
                        client = new MqttClient("test.mosquitto.org"); //Of je kan hier ook jouw eigen broker gebruiken uit vorig labo.
                        client.Connect("Sender");

                        SensorMeasurement sensorMeasurement = new SensorMeasurement();
                        sensorMeasurement.IDSensor = 1;
                        sensorMeasurement.Datetime = DateTime.Now;
                        sensorMeasurement.Value = Math.Round(18 + random.NextDouble() * 5, 2); //Een waaarde tussen 18 en 23

                        string json = JsonConvert.SerializeObject(sensorMeasurement);

                        if(client.IsConnected)
                        {
                            client.Publish("hogent/jouwVoornaamEnNaam/SensorMeasurement", Encoding.UTF8.GetBytes(json), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                            Thread.Sleep(500); //Even wachten zodat het bericht verzonden kan worden.

                            client.Disconnect();
                        }
                        client = null;
                    }
                }
                catch (Exception)
                {
                }

                Console.WriteLine("Press ENTER to upload data to MQTT");
                Console.ReadLine();
            }
        }
    }

    class SensorMeasurement
    {
        public int IDSensor { get; set; }
        public DateTime Datetime { get; set; }
        public double Value { get; set; }
    }


}
