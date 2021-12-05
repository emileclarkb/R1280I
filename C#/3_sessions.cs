/*
Each tag is either "inventoried" (scanned this inventory process) "or non-inventoried".
Once a tag is scanned it is set to the inventoried status. Once a certain duration
of time has elapsed it is reset to non-inventoried. While a tag is in the inventoried
state it cannot be rescanned This period of time is its "persistence" and is determined
by what "session" is being used.

// NOTE: this table also references energy states and temperatures but I'm not
//       sure how to interpret that information. If the device is running in continous
//       mode this seems important. Otherwise, if the device operates on button-press
//       S0 should be used.
[Session flag: persistence]
S0: non-inventorize on next frame (default)
S1: 500ms < persistence < 5s
S2: 2s < persistence
S3: 2s < persistence

"Sessions S2 and S3 have a longer and not explicitly limited persistence time
giving the opportunity to detect tags only one time during the inventory process
repetitions. These sessions are for advanced use only and are out of the scope of
this manual."


"Here below a code sample where the session S1 is used during the inventory process"
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
            // set session flag
            MySource.SetSession_EPC_C1G2(CAENRFIDLogicalSourceConstants.EPC_C1G2_SESSION_S1);
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
