/*
The Q parameter is a way of telling the reader how many tags it should expect
to scan on every inventory process. In turn, an inventory process is to the reader
what frames are to a monitor. Inventory processes only occur whilst the reader
is adding to the inventory (attempting to scan tags, either continously or by
button process).

[Estimated number of Tags: Starting Q value]
1:       0
2:       1
3-6:     2
7-15:    3
16-30:   4
30-50:   5
50-100:  6
100-200: 7


"Here below a code sample where the Q value is set to 3 before to start the inventory process"
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

             MySource.SetQ_EPC_C1G2(3);
             CAENRFIDTag[] MyTags = MySource.InventoryTag();
             if (MyTags.Length > 0)
             {
                 for (int i = 0; i < MyTags.Length; i++)
                 {
                     String s = BitConverter.ToString(MyTags[i].GetId());
                    Console.WriteLine(s);
                 }
             }
             Console.WriteLine("Press a key to end the program.");
             Console.ReadKey();
             MyReader.Disconnect();
         }
     }
}
