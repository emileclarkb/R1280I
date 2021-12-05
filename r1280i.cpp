/*
This is a complete C++ implementation demonstrating (for the most part) how the
API would be used. This is based off the pseudocode given in the user manual.
Implementations in other languages should be pretty much identical (mostly syntax changes).

Based on the content In the technical information manual, the methods shown in this
file are barely scratching the surface of the api.


References:
    - skID R1280I Main Page: https://www.caenrfid.com/en/products/skid-r1280i/
    - Technical Information Manual: https://www.caenrfid.com/wp-content/uploads/2021/02/R1280I_skID_Technical-Information-Manual_Rev_01.pdf
    - API Reference Manual: https://www.caenrfid.com/wp-content/uploads/2020/12/CAEN-RFID-API-Reference-Manual_rev_12.pdf
    - API User Manual: https://www.caenrfid.com/wp-content/uploads/2020/12/CAEN-RFID-API_UserMan_rev_05.pdf

The technical information manual is a guide for setting up the physcial product as a user with their pre-existing apps.
It also has some technical specifications on radiation patterns, orthographic designs, etc.

The reference manual lists the API's classes, methods, etc. It shows how they were
declared in all support languages (C#, Java and Android, C) although the C declarations
are also valid C++.

The user manual is pseudocode for using the API and a collection of C# examples.
*/


// import SDK
#include "CAENRFID.h"
#include <string> // std::string
#include <cmath> // std::pow


// calculate RF power (formula given on page 11 of the user manual)
// RF power units are specified (I'd hope it's in mW too but idk)

// effective radiate power (mW): ERP
// antenna's gain (dBi): gain
// cable attenuation (dB): loss
double rfPower(double ERP, double gain, double loss) {
    // no clue how this is derived
    return ERP / std::pow(10, (gain - 2.14 - loss) / 10);
}


int main() {
    // prior to connected

    // this part is not explained but the examples just use an empty constructor
    CAENRFIDReader reader();

    // connect the SDK and the reader
    // must be already linekd via bluetooth
    // the property CAENRFID_RS232 of CAENRFIDPort can be directly accessed as
    // the technical information manual specifies CAENRFIDPort is a typedef enum
    reader.Connect(CAENRFID_RS232,"COM1");

    // get reader info (model and serial number)
    CAENRFIDReaderInfo info = reader.GetReaderInfo();
    // get information as strings
    std::string model = info.GetModel();
    std::string serialNumber = info.GetSerialNumber();
    // this could probably be combined with webscrapping to complete auto-updates
    // I also saw a firmware updater binary on the skID R1280I page
    // so the two could potentially be combined
    std::string firmwareRelease = reader.GetFWRelease();

    // values given by the user manual (not sure if they're realistic or not)
    // the technical information manual does say the antenna gain is
    // "0.0 dBic 0.0 (typical)" and I'm sure there's a way to convert dBic to dBi
    // I just don't know it
    double gain = 8.0;
    double loss = 1.5;
    double ERP = 2000.0;
    // the power is type cast to int to be an input for the method:
    // void CAENRFIDReader::SetPower(int power);
    int power = (int) rfPower(ERP, gain, loss);
    // set power (integer required)
    reader.SetPower(power);

    // The formula for rfPower could be rearranged to find any other value
    // when combined with the power getter.
    // Also as this returns an integer there would be a slight margin of error in
    // any calculations that use the (integer) power due to the rounding from double to int
    power = reader.GetPower();






    // disconnect the SDK and the reader (does not disconnect bluetooth)
    reader.Disconnect();

    return 0;
}
