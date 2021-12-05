/*
"Here below a code sample about the EventInventoryTag method: the code shows how to setup an event handler for
processing data sent by the reader, start a continuous inventory operation and at the end stop it using the
InventoryAbort method."
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.caen.RFIDLibrary;

static void Main(string[] args)
{
    CAENRFIDReader Reader = new CAENRFIDReader();
    CAENRFIDLogicalSource LS0;
    byte[] Mask = new byte[4];

    Reader.CAENRFIDEvent += new CAENRFIDEventHandler(Reader_EventHandler);
    Reader.Connect(CAENRFIDPort.CAENRFID_TCP, "10.0.32.125");

    LS0 = Reader.GetSource("Source_0");
    LS0.SetReadCycle(0);

    // continuous inventory reading
    // the documentation says the second parameter should be the mask size
    // so I'd assume it should instead by 0x4 but I could be wrong
    LS0.EventInventoryTag(Mask, 0x0, 0x0, 0x06);
    // inventory reading on button press
    //LS0.EventInventoryTag (Mask, 0x0, 0x0, 0x26);

    Thread.Sleep(2*1000);
    Console.WriteLine("Main Task awake");
    Console.WriteLine("Tags read : " + ntag.ToString());
    Reader.InventoryAbort();
    Console.ReadLine();

    Reader.Disconnect();
}

static void Reader_EventHandler(object Sender, CAENRFIDEventArgs Event)
{
    foreach (CAENRFIDNotify n in Event.Data)
    {
     ntag++;
    }
}
