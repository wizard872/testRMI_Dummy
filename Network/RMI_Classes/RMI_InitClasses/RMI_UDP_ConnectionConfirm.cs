using Network.RMI_Common.RMI_ParsingClasses;
using System;

[Serializable]
public class RMI_UDP_ConnectionConfirm {

    public int checkUDP_Connection;

    public RMI_UDP_ConnectionConfirm() { }

    public RMI_UDP_ConnectionConfirm(flat_RMI_UDP_ConnectionConfirm data) {
        this.checkUDP_Connection = data.CheckUDPConnection;
    }

    public static RMI_UDP_ConnectionConfirm CreateRMI_UDP_ConnectionConfirm(byte[] data)
    {
        return flat_RMI_UDP_ConnectionConfirm.GetRootAsflat_RMI_UDP_ConnectionConfirm( data );
    }

    public static byte[] getBytes(RMI_UDP_ConnectionConfirm data)
    {
        return flat_RMI_UDP_ConnectionConfirm.Createflat_RMI_UDP_ConnectionConfirm( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}