using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class heartBeatCheck_Response {

    public float timeData;

    public heartBeatCheck_Response() { }

    public heartBeatCheck_Response(flat_heartBeatCheck_Response data) {
        this.timeData = data.TimeData;
    }

    public static heartBeatCheck_Response CreateheartBeatCheck_Response(byte[] data)
    {
        return flat_heartBeatCheck_Response.GetRootAsflat_heartBeatCheck_Response( data );
    }

    public static byte[] getBytes(heartBeatCheck_Response data)
    {
        return flat_heartBeatCheck_Response.Createflat_heartBeatCheck_Response( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}