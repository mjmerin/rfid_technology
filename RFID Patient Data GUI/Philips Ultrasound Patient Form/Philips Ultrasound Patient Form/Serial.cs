using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace Philips_Ultrasound_Patient_Form
{
    class Serial
    {
        private SerialPort comPort;
        //public byte[] receivedData; //Creates an array of bytes good for 256 bytes
        //private bool dataReceived = false;

        public Serial(string portName)
        {
            //System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            comPort = new SerialPort();
            comPort.PortName = portName;
            comPort.BaudRate = 4800;
            //comPort.BaudRate = 115200;
            comPort.Parity = Parity.Odd;
           // comPort.Parity = Parity.None;
            comPort.DataBits = 8;
            comPort.StopBits = StopBits.Two;
            //comPort.StopBits = StopBits.One;
            comPort.Handshake = Handshake.None;
            comPort.ReceivedBytesThreshold = 1;
            comPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            comPort.ReadTimeout = 1000;//in ms
            //comPort.Encoding = utf8;
        }

        public string ComName
        {
            get
            {
                return comPort.PortName;
            }
            set
            {
                Close();
                comPort.PortName = value;
                Open();
            }
        }
        public bool Open()
        {
            try
            {
                comPort.Open();
                TalkRFIDString("debug_off");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            comPort.Close();
        }

        public bool Write(byte[] dataToSend)
        {
            string response = TalkRFIDString("writetag");
            if ( response.Contains("OK") )
            {
                //Add the "end of transmission buffer" to the actual data to make a final data
                byte[] final_data = PrepareData(dataToSend);

                //Write this data through the comport
                comPort.Write(final_data, 0, final_data.Length);

                return true;
            }
            else
            {
                return false;
            }

        }

        public void Write(string command)
        {
            char[] commandArray = command.ToCharArray();
            byte[] dataToSend = new byte[commandArray.Length];
            
            for (int i = 0; i < dataToSend.Length; i++)
            {
                dataToSend[i] = (byte)commandArray[i];
            }
            //Add the "end of transmission buffer" to the actual data to make a final data
            byte[] final_data = PrepareData(dataToSend);

            
            //Write this data through the comport
            comPort.Write(final_data, 0, final_data.Length);

            //comPort.Write(dataToSend, 0, dataToSend.Length);
        }

        public byte[] TalkRFIDByte(string command)
        {
            byte[] receivedBuffer = new byte[64];
            Write(command); // Write the command array
            int count = 0;

            MemoryStream dataStream = new MemoryStream();
            try
            {
                do
                {
                    count = comPort.Read(receivedBuffer, 0, 64);
                    dataStream.Write(receivedBuffer, 0, count);
                } while (count != 0);
            }
            catch
            {
                if (dataStream.Length == 0)
                    return null;
            }
            
            int dataLength = (int)(dataStream.Length - 5);
            byte[] receivedData = new byte[dataLength];
            dataStream.Position = 0;
            dataStream.Read(receivedData, 0, dataLength);
            return receivedData; 
        }

        public string TalkRFIDString(string command)
        {
            Write(command);

            string response;
            try
            {
                //while (!dataReceived) ;
                response = comPort.ReadTo("\r\r\r\r\r");
                //response = comPort.ReadTo("\r\n");
            }
            catch
            {
                    return null;
            }

            return response;
        }

        //ARGGHH I SHOULD HAVE WRITTEN THIS!!!!!!!!!!!!!!!
        private byte[] PrepareData(byte[] data)
        {
            //Create a buffer of size 5
            byte[] end_of_transmission = new byte[5] { 0x0D, 0x0D, 0x0D, 0x0D, 0x0D }; 

            //Create an array named prepared data of type byte that can store the length of original data
            //plus "end of transmission buffer"
            byte[] preparedData = new byte[data.Length + end_of_transmission.Length]; 

            //Copy the information from our data to the prepared data array
            Buffer.BlockCopy(data, 0, preparedData, 0, data.Length);
            
            //Copy the "end of transmission buffer" to the prepared data array after our original data. 
            Buffer.BlockCopy(end_of_transmission, 0, preparedData, data.Length, end_of_transmission.Length);

            //Return the prepared data array. 
            return preparedData;
        }

        // Set the data received flag to true. 
        private void DataReceived( object sender, SerialDataReceivedEventArgs e ) 
        {
            //dataReceived = true;
        }   
    }
}