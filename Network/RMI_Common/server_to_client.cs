using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;

public class server_to_client
{
    private NetworkSession networkSession;

    public server_to_client(NetworkSession networkSession)
    {
        this.networkSession = networkSession;
    }
    
    //네트워크로 송신하는 로직을 담음.
    private void callRMI_Method(RMI_ID rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] RMIdata) {

        //암호화를 담당하는 부분.
        byte[] data = RMI_EncryptManager.RMI_EncryptMethod(rmi_id, rmi_ctx, true, RMIdata);

        //RMI_Context.Reliable = 1; ~ RMI_Context.UnReliable_Public_AES256 = 10;
        if(1<=(short)rmi_ctx && (short)rmi_ctx<=5)
            networkSession.rmiParser.sendByte_TCP(rmi_id, rmi_ctx, packetType, data); //신뢰성 전송
        else
            networkSession.rmiParser.sendByte_UDP(rmi_id, rmi_ctx, packetType, data); //비신뢰성 전송
    }

    //네트워크로 송신하는 로직을 담음.
    private void callRMI_Method(RMI_ID[] rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] RMIdata) {

        //암호화를 담당하는 부분.
        byte[] data = RMI_EncryptManager.RMI_EncryptMethod_Arr(rmi_id, rmi_ctx, true, RMIdata);

        //RMI_Context.Reliable = 1; ~ RMI_Context.UnReliable_Public_AES256 = 10;
        if(1<=(short)rmi_ctx && (short)rmi_ctx<=5)
            networkSession.rmiParser.sendByte_TCP(rmi_id, rmi_ctx, packetType, data); //신뢰성 전송
        else
            networkSession.rmiParser.sendByte_UDP(rmi_id, rmi_ctx, packetType, data); //비신뢰성 전송
    }

    //네트워크에서 수신하는 로직을 담음.
    public void recvRMI_Method(RMI_ID rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] RMIdata) {

        //복호화를 담당하는 부분.
        byte[] data = RMI_EncryptManager.RMI_EncryptMethod(rmi_id, rmi_ctx, false, RMIdata);

        parseRMI(rmi_id, rmi_ctx, packetType, data);
    }





//수신 처리.
////////////////////////////////////////////////////////////////////////

    private void parseRMI(RMI_ID rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] RMIdata)
    {
        try {
        switch (packetType) {

            case RMI_PacketType.pingCheck_Response:
                pingCheck_Response r0 = flat_pingCheck_Response.GetRootAsflat_pingCheck_Response(RMIdata);
                Logic_pingCheck_Response.RMI_Packet(rmi_id, rmi_ctx, r0.timeData);
                networkSession.rmiParser.onRMI_Recv(rmi_ctx, packetType, r0);
                r0 = null;
                break;
            case RMI_PacketType.heartBeatCheck_Response:
                heartBeatCheck_Response r1 = flat_heartBeatCheck_Response.GetRootAsflat_heartBeatCheck_Response(RMIdata);
                Logic_heartBeatCheck_Response.RMI_Packet(rmi_id, rmi_ctx, r1.timeData);
                networkSession.rmiParser.onRMI_Recv(rmi_ctx, packetType, r1);
                r1 = null;
                break;
            case RMI_PacketType.sendTestDataToClient:
                sendTestDataToClient r2 = flat_sendTestDataToClient.GetRootAsflat_sendTestDataToClient(RMIdata);
                Logic_sendTestDataToClient.RMI_Packet(rmi_id, rmi_ctx, r2.timeData, r2.testData);
                networkSession.rmiParser.onRMI_Recv(rmi_ctx, packetType, r2);
                r2 = null;
                break;
            case RMI_PacketType.sendTestDataToClient2:
                sendTestDataToClient2 r3 = flat_sendTestDataToClient2.GetRootAsflat_sendTestDataToClient2(RMIdata);
                Logic_sendTestDataToClient2.RMI_Packet(rmi_id, rmi_ctx, r3.timeData, r3.testData, r3.testDataArray);
                networkSession.rmiParser.onRMI_Recv(rmi_ctx, packetType, r3);
                r3 = null;
                break;
                
            default:
                Console.WriteLine("[server_to_client] 그룹에 존재하지 않는 RMI콜. 패킷 무시.");
                break;
        }
        } catch (Exception e) {
            Console.WriteLine("Packet파싱중 에러발생! 데이터 손상! rmi_ctx="+rmi_ctx+" packetType="+packetType+" RMIdata="+RMIdata.Length + "\n"+e.StackTrace);
        }
    }

//송신 처리!
////////////////////////////////////////////////////////////////////////
    public void pingCheck_Response(RMI_ID rmi_id, RMI_Context rmi_ctx, float timeData) {
        pingCheck_Response _data__1 = new pingCheck_Response();
        _data__1.timeData = timeData;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.pingCheck_Response, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.pingCheck_Response, _data__1);
        _data__1 = null;
    }

    public void pingCheck_Response(RMI_ID[] rmi_id, RMI_Context rmi_ctx, float timeData) {
        pingCheck_Response _data__1 = new pingCheck_Response();
        _data__1.timeData = timeData;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.pingCheck_Response, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.pingCheck_Response, _data__1);
        _data__1 = null;
    }

    public void heartBeatCheck_Response(RMI_ID rmi_id, RMI_Context rmi_ctx, float timeData) {
        heartBeatCheck_Response _data__1 = new heartBeatCheck_Response();
        _data__1.timeData = timeData;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.heartBeatCheck_Response, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.heartBeatCheck_Response, _data__1);
        _data__1 = null;
    }

    public void heartBeatCheck_Response(RMI_ID[] rmi_id, RMI_Context rmi_ctx, float timeData) {
        heartBeatCheck_Response _data__1 = new heartBeatCheck_Response();
        _data__1.timeData = timeData;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.heartBeatCheck_Response, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.heartBeatCheck_Response, _data__1);
        _data__1 = null;
    }

    public void sendTestDataToClient(RMI_ID rmi_id, RMI_Context rmi_ctx, float timeData, string testData) {
        sendTestDataToClient _data__1 = new sendTestDataToClient();
        _data__1.timeData = timeData;
        _data__1.testData = testData;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.sendTestDataToClient, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.sendTestDataToClient, _data__1);
        _data__1 = null;
    }

    public void sendTestDataToClient(RMI_ID[] rmi_id, RMI_Context rmi_ctx, float timeData, string testData) {
        sendTestDataToClient _data__1 = new sendTestDataToClient();
        _data__1.timeData = timeData;
        _data__1.testData = testData;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.sendTestDataToClient, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.sendTestDataToClient, _data__1);
        _data__1 = null;
    }

    public void sendTestDataToClient2(RMI_ID rmi_id, RMI_Context rmi_ctx, float timeData, RMI_TestData testData, List<RMI_Vector3> testDataArray) {
        sendTestDataToClient2 _data__1 = new sendTestDataToClient2();
        _data__1.timeData = timeData;
        _data__1.testData = testData;
        _data__1.testDataArray = testDataArray;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.sendTestDataToClient2, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.sendTestDataToClient2, _data__1);
        _data__1 = null;
    }

    public void sendTestDataToClient2(RMI_ID[] rmi_id, RMI_Context rmi_ctx, float timeData, RMI_TestData testData, List<RMI_Vector3> testDataArray) {
        sendTestDataToClient2 _data__1 = new sendTestDataToClient2();
        _data__1.timeData = timeData;
        _data__1.testData = testData;
        _data__1.testDataArray = testDataArray;
        byte[] RMIdata = _data__1.getBytes(); //_data__1.getBytes()
        callRMI_Method(rmi_id, rmi_ctx, RMI_PacketType.sendTestDataToClient2, RMIdata);
        networkSession.rmiParser.onRMI_Call(rmi_ctx, RMI_PacketType.sendTestDataToClient2, _data__1);
        _data__1 = null;
    }

}
