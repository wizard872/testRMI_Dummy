using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class pingCheck_Response {

    public float timeData;

    public pingCheck_Response() { }

    public pingCheck_Response(flat_pingCheck_Response data) {
        this.timeData = data.TimeData;
    }

    public static pingCheck_Response CreatepingCheck_Response(byte[] data)
    {
        return flat_pingCheck_Response.GetRootAsflat_pingCheck_Response( data );
    }

    public static byte[] getBytes(pingCheck_Response data)
    {
        return flat_pingCheck_Response.Createflat_pingCheck_Response( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}