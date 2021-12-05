/*
"Here below a code sample of reading/writing data from/to the user memory"
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.caen.RFIDLibrary;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CAENRFIDReader MyReader = new CAENRFIDReader();
            MyReader.Connect(CAENRFIDPort.CAENRFID_RS232, "COM3");
            CAENRFIDLogicalSource MySource = MyReader.GetSource("Source_0");

            CAENRFIDTag[] MyTags = MySource.InventoryTag();
            if (MyTags.Length > 0)
            {
                for (int i = 0; i < MyTags.Length; i++)
                {
                    String EPCString = BitConverter.ToString(MyTags[i].GetId());
                    Console.WriteLine(EPCString);

                    byte[] DataToWrite;
                    ASCIIEncoding Enc = new ASCIIEncoding();

                    DataToWrite = Enc.GetBytes("Hello!");
                    MySource.WriteTagData_EPC_C1G2(MyTags[i], 3, 0, 6, DataToWrite);
                    Console.WriteLine("Tag written!");

                    byte[] DataToRead;
                    DataToRead = MySource.ReadTagData_EPC_C1G2(MyTags[i], 3, 0, 6);
                    Console.WriteLine("Tag read, value = " + Enc.GetString(DataToRead));
                }
           }
           Console.WriteLine("Press a key to end the program.");
           Console.ReadKey();
           MyReader.Disconnect();
        }
    }
}
