/*
"Here below a code example of the LockTag_EPC_C1G2 method utilization"
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

                    // +-------+-------+-------+-------+-------+-------+-------+-------+-------+-------+
                    // | Kill | Acces | EPC | TID | User | Kill | Acces | EPC | TID | User |
                    // | Mask | Mask | Mask | Mask | Mask | Act. | Act. | Act. | Act. | Act. |
                    // +---+---+---+---+---+---+---+---+---+---+---+---+-------+-------+-------+-------+
                    // | W | P | W | P | W | P | W | P | W | P | W | P | W | P | W | P | W | P | W | P |
                    // +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
                    // | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 |
                    // +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
                    //
                    // Non permanent locking of User Memory Bank

                    int Payload = 0x00802;
                    MySource.LockTag_EPC_C1G2(MyTags[i], Payload);
                }
            }
            Console.WriteLine("Press a key to end the program.");
            Console.ReadKey();
            MyReader.Disconnect();
        }
    }
}
