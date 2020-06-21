using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class RMI_Vector3 {

    public short xPos;
    public short yPos;
    public short zPos;

    public RMI_Vector3() { }

    public RMI_Vector3(flat_RMI_Vector3 data) {
        this.xPos = data.XPos;
        this.yPos = data.YPos;
        this.zPos = data.ZPos;
    }

    public static RMI_Vector3 CreateRMI_Vector3(byte[] data)
    {
        return flat_RMI_Vector3.GetRootAsflat_RMI_Vector3( data );
    }

    public static byte[] getBytes(RMI_Vector3 data)
    {
        return flat_RMI_Vector3.Createflat_RMI_Vector3( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}