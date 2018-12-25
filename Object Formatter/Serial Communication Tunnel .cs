using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Object_Formatter
{
   public class Serial_Communication_Tunnel
    {
        SerialPort  serialPort = new SerialPort();
        
       
        public Serial_Communication_Tunnel()
        {
            if (!serialPort.IsOpen)
            {
                serialPort.PortName = "COM12";
                serialPort.Handshake = Handshake.None;
                serialPort.BaudRate = 57600;
                serialPort.Handshake = Handshake.None;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.ReadTimeout = 200;
                serialPort.WriteTimeout = 50;
                serialPort.Open();
                //Debug_On Command
                byte[] Debug_On_Command = { 0xAA, 0x55, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x06, 0x00, 0x24, 0x49, 0x53, 0x2C, 0x30, 0x3B, 0x5E, 0x6F };
                serialPort.Write(Debug_On_Command, 0, Debug_On_Command.Length);
            }
        }
        public string ToDevice()
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
            // Asking for parameters command
         
            string FromSerial;
            byte[] ParameterQueryCommand = { 0xAA, 0x55, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x19, 0x00, 0x23, 0x53, 0x43, 0x2C, 0x41, 0x44, 0x3C, 0x3E, 0x7C, 0x41, 0x61, 0x3C, 0x3E, 0x7C, 0x41, 0x62, 0x3C, 0x3E, 0x3B, 0xED, 0xB0 };
            string TimeBeforWrite = DateTime.Now.ToString();
            serialPort.Write(ParameterQueryCommand, 0, ParameterQueryCommand.Length);
            Thread.Sleep(200);
            FromSerial = serialPort.ReadExisting();
            byte[] bytestosend3 = { 0xAA, 0x00, 0x24, 0x49, 0x53, 0x2C, 0x2A, 0x3B, 0x5E, 0x6F };
            serialPort.Write(bytestosend3, 0, bytestosend3.Length);

            string TimeSent = DateTime.Now.ToString();
            string mega = "";
         
            mega += FromSerial;

            //  return FromSerial+ DateTime.Now.ToString() +" when sent" + sentnow;
            int Counter = 0;
            bool KeepRolling = true;
            while (KeepRolling && Counter < 5)
            {
                if (FromSerial != "")
                {
                    for (int i = 1; i < FromSerial.Length; i++)
                    {
                        if (FromSerial[i] == 120 && FromSerial[i-1] == 48)
                        {
                            KeepRolling = false;

                            return "From here " + FromSerial.Substring(0, FromSerial.IndexOf(";"));

                        }
                    }
                    FromSerial = serialPort.ReadExisting();
                    //mega += TimeSent +"   "+ DateTime.Now.ToString() +"\n";
                    Counter++;

                }
                else
                {
                    FromSerial = serialPort.ReadExisting();
                    Counter++;
                }
            }
            return mega;

        }

            //Debug_Off Command
         //   byte[] bytestosend3 = { 0xAA, 0x55, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x06, 0x00, 0x24, 0x49, 0x53, 0x2C, 0x31, 0x3B, 0x5E, 0x6F };
         //   serialPort.Write(bytestosend3, 0, bytestosend.Length);
           
        }
    }

