using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class sendTestDataToServer2 {

    public float timeData;
    public string testData;
    public List <RMI_Vector3> testDataArray = new List<RMI_Vector3>();

    public sendTestDataToServer2() { }

    public sendTestDataToServer2(flat_sendTestDataToServer2 data) {
        this.timeData = data.TimeData;
        this.testData = data.TestData;
        for(int i = 0;i < data.TestDataArrayLength;i++) {
            this.testDataArray.Add(new RMI_Vector3(data.TestDataArray(i).Value));
        }
    }

    public static sendTestDataToServer2 CreatesendTestDataToServer2(byte[] data)
    {
        return flat_sendTestDataToServer2.GetRootAsflat_sendTestDataToServer2( data );
    }

    public static byte[] getBytes(sendTestDataToServer2 data)
    {
        return flat_sendTestDataToServer2.Createflat_sendTestDataToServer2( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}