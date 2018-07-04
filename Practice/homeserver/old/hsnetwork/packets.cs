using System;

namespace HomeServer.Core
{
    public enum PacketType
    {
        ACK = 0x06,
        NAK = 0x15,
        Signon = 0xC8,
        Status = 0xC9,
        Sensor = 0xCA,
		Command = 0xCB,
        EnergyMeter = 0xCC,
        ServerPoll = 0xCD,
		EnergyCal = 0xCE
    }
	
	public enum CommandCode
	{
		GetCalibrationData = 0xB0
	}

    public interface IPacket
    {
		PacketType Type { get; }
		int Handle { get; }
		int PacketLength { get; }
	    byte[] Data { get; }
		
	    byte GetBytes();
	}
	
    public class EnergyPacket: IPacket
    {
		#region IPacket implementation
		public byte GetBytes ()
		{
			throw new NotImplementedException ();
		}

		public PacketType Type {
			get {
				throw new NotImplementedException ();
			}
		}

		public int Handle {
			get {
				throw new NotImplementedException ();
			}
		}

		public int PacketLength {
			get {
				throw new NotImplementedException ();
			}
		}

		public byte[] Data {
			get {
				throw new NotImplementedException ();
			}
		}
		#endregion
	    /*
	     * public:
        // Constructor
	      EnergyPacket(
	        byte,       // Sensor Pin
		      unsigned long, // Voltage
		      unsigned long, // Current
		      unsigned long, // Real Power
		      unsigned long  // Power Increment
		  );
		  */
    };

    public class SignOnPacket: IPacket
    {
		#region IPacket implementation
		public byte GetBytes ()
		{
			throw new NotImplementedException ();
		}

		public PacketType Type {
			get {
				throw new NotImplementedException ();
			}
		}

		public int Handle {
			get {
				throw new NotImplementedException ();
			}
		}

		public int PacketLength {
			get {
				throw new NotImplementedException ();
			}
		}

		public byte[] Data {
			get {
				throw new NotImplementedException ();
			}
		}
		#endregion
		/*
        public:
  	        // Constructor
  	        SignOnPacket(DeviceType, byte*);
  	    */
    }
}

