using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class sendTestDataToServer {

    public float timeData;
    public string testData;

    public sendTestDataToServer() { }

    public sendTestDataToServer(flat_sendTestDataToServer data) {
        this.timeData = data.TimeData;
        this.testData = data.TestData;
    }

    public static sendTestDataToServer CreatesendTestDataToServer(byte[] data)
    {
        return flat_sendTestDataToServer.GetRootAsflat_sendTestDataToServer( data );
    }

    public static byte[] getBytes(sendTestDataToServer data)
    {
        return flat_sendTestDataToServer.Createflat_sendTestDataToServer( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}