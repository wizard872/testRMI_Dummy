using Network.RMI_Common.RMI_ParsingClasses;
using System;

[Serializable]
public class RMI_OverConnectionAnnounce {

    public int maxConnection;

    public RMI_OverConnectionAnnounce() { }

    public RMI_OverConnectionAnnounce(flat_RMI_OverConnectionAnnounce data) {
        this.maxConnection = data.MaxConnection;
    }

    public static RMI_OverConnectionAnnounce CreateRMI_OverConnectionAnnounce(byte[] data)
    {
        return flat_RMI_OverConnectionAnnounce.GetRootAsflat_RMI_OverConnectionAnnounce( data );
    }

    public static byte[] getBytes(RMI_OverConnectionAnnounce data)
    {
        return flat_RMI_OverConnectionAnnounce.Createflat_RMI_OverConnectionAnnounce( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}