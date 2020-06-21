using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class sendTestDataToClient2 {

    public float timeData;
    public RMI_TestData testData;
    public List <RMI_Vector3> testDataArray = new List<RMI_Vector3>();

    public sendTestDataToClient2() { }

    public sendTestDataToClient2(flat_sendTestDataToClient2 data) {
        this.timeData = data.TimeData;
        this.testData = new RMI_TestData(data.TestData.Value);
        for(int i = 0;i < data.TestDataArrayLength;i++) {
            this.testDataArray.Add(new RMI_Vector3(data.TestDataArray(i).Value));
        }
    }

    public static sendTestDataToClient2 CreatesendTestDataToClient2(byte[] data)
    {
        return flat_sendTestDataToClient2.GetRootAsflat_sendTestDataToClient2( data );
    }

    public static byte[] getBytes(sendTestDataToClient2 data)
    {
        return flat_sendTestDataToClient2.Createflat_sendTestDataToClient2( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}