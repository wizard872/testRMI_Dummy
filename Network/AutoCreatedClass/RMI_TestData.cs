using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

[Serializable]
public class RMI_TestData {

    public string testValue1;
    public int testValue2;
    public float testValue3;

    public RMI_TestData() { }

    public RMI_TestData(flat_RMI_TestData data) {
        this.testValue1 = data.TestValue1;
        this.testValue2 = data.TestValue2;
        this.testValue3 = data.TestValue3;
    }

    public static RMI_TestData CreateRMI_TestData(byte[] data)
    {
        return flat_RMI_TestData.GetRootAsflat_RMI_TestData( data );
    }

    public static byte[] getBytes(RMI_TestData data)
    {
        return flat_RMI_TestData.Createflat_RMI_TestData( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}