using System.Collections.Generic;
using System;

public class Logic_sendTestDataToClient {

    //Called_RMI(Remote Method Invocation)_Method
    public static void RMI_Packet(RMI_ID rmi_id, RMI_Context rmi_ctx, float timeData, string testData)
    {
        //do something.

        //수신시 로직 처리
        Console.WriteLine(timeData);
    }
}