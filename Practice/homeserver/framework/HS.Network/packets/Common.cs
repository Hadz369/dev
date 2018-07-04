using System;
using System.Collections.Generic;

namespace HS.Network
{
    public enum PacketType
    {
        ACK          = 0x06,
        NAK          = 0x15,
        Signon       = 0xC8,
        Status       = 0xC9,
        Sensor       = 0xCA,
		Command      = 0xCB,
        EnergyMeter  = 0xCC,
        ServerPoll   = 0xCD,
		EnergyCal    = 0xCE,
        PowerSummary = 0xCF,
    }
	
	public enum CommandCode
	{
		GetCalibrationData = 0xB0
	}
}

