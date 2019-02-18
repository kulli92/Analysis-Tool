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
        public static string FinalFormattedString { get; set; } 
       // public static string  { get; set; } 
        SerialPort  serialPort = new SerialPort();
        
        public static string StaticResponseString { get; set; }
        public Serial_Communication_Tunnel()
        {
            FinalFormattedString = "";
            if (!serialPort.IsOpen)
            {
               
            }
        }
        public  void OpenPort(string s)
        {
            if (!serialPort.IsOpen)
            {
                serialPort.PortName = s;
                serialPort.Handshake = Handshake.None;
                serialPort.BaudRate = 57600;
                serialPort.Handshake = Handshake.None;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.ReadTimeout = 200;
                serialPort.WriteTimeout = 50;
                serialPort.DtrEnable = true;

                serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                serialPort.Open();
            }
        }
        public void ClosePort()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        private  void DataReceivedHandler( object sender, SerialDataReceivedEventArgs e)
        {
        }
        public void ToDevice(string ManullyWrittenShema, short IntervalTime, short Repeat)
        {
            /*
            $GS,<2 bytes Schema ID or buffered Sch length ><2 bytes Interval Time ><2 Bytes Repeat ><Buffered Sch >
            value < 0x8000 indicate to Schema ID 
            else first byte show length of buffered Sch which will be sent in same packet 
            buffered sch max length 128 char 
            included 3B ';'send for continuous Repeat = 0
            to cancel current command send Interval = 0*/
            // Store integer 182
            //int intValue = 182;
            //int intValue;/*
           /*byte[] intBytes = BitConverter.GetBytes(intValue);
            Array.Reverse(intBytes);
            byte[] result = intBytes;*/


            if (serialPort.IsOpen)
            {
                
              

                byte[] IntervalTimeArray = BitConverter.GetBytes(IntervalTime);
                //Array.Reverse(IntervalTimeArray);

                byte[] RepeatArray = BitConverter.GetBytes(Repeat);
             

                char[] CustomSchema = ManullyWrittenShema.ToCharArray();

                byte[] SchemaIdArray = BitConverter.GetBytes((short)(ManullyWrittenShema.Count()+ 32768)); //  32768 = 8000



                byte[] PrefixCommand = { 0xAA, 0x55, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0xFF, 0xFF};

                byte[] DataLength = BitConverter.GetBytes((short)(ManullyWrittenShema.Count() + 10));
                //byte DataLength = SchemaIdArray[0];
                                
                byte[] Command = { 0x24, 0x47, 0x53, 0x2c }; // @GS,
                byte[] crc = { 0xAA, 0XEA };//Any crc
              
               // char[] temparray = FinalFormattedString.ToCharArray();

                serialPort.Write(PrefixCommand, 0, PrefixCommand.Length);
                serialPort.Write(DataLength, 0, 2);
                serialPort.Write(Command, 0, Command.Length);
                serialPort.Write(SchemaIdArray, 0, SchemaIdArray.Count());
                serialPort.Write(IntervalTimeArray, 0, IntervalTimeArray.Count());
                serialPort.Write(RepeatArray, 0, RepeatArray.Count());
                serialPort.Write(CustomSchema, 0, CustomSchema.Count());
                serialPort.Write(crc, 0, crc.Count());
                Thread.Sleep(1000);
                StaticResponseString = serialPort.ReadExisting() + " ";
              /*  int x = StaticResponseString.LastIndexOf(';');
                if (x == -1)
                    x = 0;
                while (StaticResponseString[x] == 59 && FinalFormattedString.Count() != 0 )
                {
                    StaticResponseString = serialPort.ReadExisting() + " ";
                }*/
            }
            // Asking for parameters command
  
        }

       /* private byte[] AnalayzStringToHex()
        {

           char[] temparray =  FinalFormattedString.ToCharArray();
            byte DataLenght = (byte)temparray.Count();

            byte[] Result = {DataLenght, 0x00 };
            foreach (var character in temparray)
            {
                
            }
            
          
        }
        */
        }
    }








 /*
                 int Counter = 0;
            bool KeepRolling = true;
            while (KeepRolling && Counter < 5)
            {
                if ( != "")
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
            return mega;*/
       
        //Debug_Off Command
        //   byte[] bytestosend3 = { 0xAA, 0x55, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x06, 0x00, 0x24, 0x49, 0x53, 0x2C, 0x31, 0x3B, 0x5E, 0x6F };
        //   serialPort.Write(bytestosend3, 0, bytestosend.Length);
        //Debug_On Command
        //  byte[] Debug_On_Command = { 0xAA, 0x55, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x06, 0x00, 0x24, 0x49, 0x53, 0x2C, 0x30, 0x3B, 0x5E, 0x6F };
        // serialPort.Write(Debug_On_Command, 0, Debug_On_Command.Length);


