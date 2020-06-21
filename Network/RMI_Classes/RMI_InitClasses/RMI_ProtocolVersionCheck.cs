using Network.RMI_Common.RMI_ParsingClasses;
using System;

[Serializable]
public class RMI_ProtocolVersionCheck {

    public int rmi_protocol_version;

    public RMI_ProtocolVersionCheck() { }

    public RMI_ProtocolVersionCheck(flat_RMI_ProtocolVersionCheck data) {
        this.rmi_protocol_version = data.RmiProtocolVersion;
    }

    public static RMI_ProtocolVersionCheck CreateRMI_ProtocolVersionCheck(byte[] data)
    {
        return flat_RMI_ProtocolVersionCheck.GetRootAsflat_RMI_ProtocolVersionCheck( data );
    }

    public static byte[] getBytes(RMI_ProtocolVersionCheck data)
    {
        return flat_RMI_ProtocolVersionCheck.Createflat_RMI_ProtocolVersionCheck( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}