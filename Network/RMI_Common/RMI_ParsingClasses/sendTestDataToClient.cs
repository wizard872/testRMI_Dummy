using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class sendTestDataToClient {

    public float timeData;
    public string testData;

    public sendTestDataToClient() { }

    public sendTestDataToClient(flat_sendTestDataToClient data) {
        this.timeData = data.TimeData;
        this.testData = data.TestData;
    }

    public static sendTestDataToClient CreatesendTestDataToClient(byte[] data)
    {
        return flat_sendTestDataToClient.GetRootAsflat_sendTestDataToClient( data );
    }

    public static byte[] getBytes(sendTestDataToClient data)
    {
        return flat_sendTestDataToClient.Createflat_sendTestDataToClient( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}