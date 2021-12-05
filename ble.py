'''
Really simple command line tool I made to impletment of asynchronous and platform
agnostic BLE support.

However a socket implementation is doable in any language.

Dependency: bleak
'''

import sys
import re
import asyncio

from bleak import BleakScanner
from _manufacturers import MANUFACTURERS


address = '24:71:89:cc:09:05'
MODEL_NBR_UUID = '00002a24-0000-1000-8000-00805f9b34fb'


# modified from bleak.BLEDevice.__str__
def deviceName(device):
    if not device.name:
        if "manufacturer_data" in device.metadata:
            ks = list(device.metadata["manufacturer_data"].keys())
            if len(ks):
                mf = MANUFACTURERS.get(ks[0], MANUFACTURERS.get(0xFFFF))
                value = device.metadata["manufacturer_data"].get(
                    ks[0], MANUFACTURERS.get(0xFFFF)
                )
                #return "{0}: {1} ({2})".format(device.address, mf, value)
                return mf
    #return "{0}: {1}".format(device.address, device.name or "Unknown")
    return device.name or "Unknown"


# list devices
async def devices():
    # get device names
    devices = await BleakScanner.discover()
    if len(devices):
        print('Devices:')
    # iterate enumerated names
    for i, d in enumerate(devices):
        print(f'\t{i}: {d.address} {deviceName(d)}')

        # find important section of name (ignores binary ending)
        #name = re.search('^(((?:[A-Z0-9]{2}:){4}[A-Z0-9]{2}):[\w\s.]*)', string(d))
        #print(f'\t{i}: {name}')

# connect to device
async def connect(address):
    async with BleakClient(address) as client:
        model_number = await client.read_gatt_char(MODEL_NBR_UUID)
        print('Model Number: {0}'.format(''.join(map(chr, model_number))))




def main():
    if sys.argv[1] == 'devices':
        try:
            asyncio.run(devices())
        except OSError:
            print('[!] Make sure BlueTooth is enabled before continuing...')
    elif sys.argv[1] == '':

if __name__ == '__main__':
    main()
