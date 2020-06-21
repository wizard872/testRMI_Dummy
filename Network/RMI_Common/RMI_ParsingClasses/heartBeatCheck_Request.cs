using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class heartBeatCheck_Request {

    public float timeData;

    public heartBeatCheck_Request() { }

    public heartBeatCheck_Request(flat_heartBeatCheck_Request data) {
        this.timeData = data.TimeData;
    }

    public static heartBeatCheck_Request CreateheartBeatCheck_Request(byte[] data)
    {
        return flat_heartBeatCheck_Request.GetRootAsflat_heartBeatCheck_Request( data );
    }

    public static byte[] getBytes(heartBeatCheck_Request data)
    {
        return flat_heartBeatCheck_Request.Createflat_heartBeatCheck_Request( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}