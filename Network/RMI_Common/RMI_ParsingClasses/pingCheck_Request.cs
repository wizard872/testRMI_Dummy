using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class pingCheck_Request {

    public float timeData;

    public pingCheck_Request() { }

    public pingCheck_Request(flat_pingCheck_Request data) {
        this.timeData = data.TimeData;
    }

    public static pingCheck_Request CreatepingCheck_Request(byte[] data)
    {
        return flat_pingCheck_Request.GetRootAsflat_pingCheck_Request( data );
    }

    public static byte[] getBytes(pingCheck_Request data)
    {
        return flat_pingCheck_Request.Createflat_pingCheck_Request( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}