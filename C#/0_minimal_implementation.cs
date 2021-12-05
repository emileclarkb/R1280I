/*
C# implementation (as per the user manual):
    - https://www.caenrfid.com/wp-content/uploads/2020/12/CAEN-RFID-API_UserMan_rev_05.pdf


"The minimum steps a programmer should follow in order to operate with a CAEN RFID reader using the API are the
following:

    - Open a connection with the reader
    - Configuration of the logical source (optional if the default configuration is fine)
    - Detection of tags and other operations on the tags
    - Close the connection with the reader.

Here below a simple but complete code snippet showing the minimum required lines of code to detect RFID tags using
a CAEN RFID reader and the CAEN RFID API. The code is shown using .NET C# programming language but it can be
easily adapted to the other languages supported by the API."
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
