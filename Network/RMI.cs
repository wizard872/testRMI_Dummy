using Network.RMI_Common.RMI_ParsingClasses;
using System;
using System.Net;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;

public class RMI
{
    public RMI(NetworkSession networkSession)
    {
        this._networkSession = networkSession;
    }

    private NetworkSession _networkSession;

    //RMI 설정 부분.
    //이 값이 다를 경우, 통신하지 않고 연결을 차단한다.
    //=======================================================================================

    //RMI 송수신용 프로토콜 버전.
    //이 값을 기반으로 버전 구분을 한다. 서버, 클라이언트 각각의 rmi_protocol_version 값이 일치해야 한다.
    public static int rmi_protocol_version = 2019102701;

    //송수신 로그 활성화 여부.
    //true로 설정되어있으면 Log 기록용 메소드인 onRMI_Call(송신시), onRMI_Recv(수신시) 메소드가 호출된다.
    //로그를 사용할 경우, onRMI_Call(송신시), onRMI_Recv(수신시) 메소드에 로그 기록 로직을 작성해야 한다.
    public static bool isLogEnable = false;

    //=======================================================================================
    //RMI 설정 부분 종료.


    //직접적으로 네트워크 송수신 함수 부분과 접하는 지점. 수동으로 수정할 필요가 있음.
    //================================================================================

    //송신 로직. TCP
    public void sendByte_TCP(RMI_ID rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] data)
    {
        if (rmi_id == null)
        {
            throw new ArgumentException("sendByte_TCP()::RMI_ID는 null일 수 없습니다.");
        }

        if (rmi_id.getTCP_Object() == null)
        {
            throw new ArgumentException("sendByte_TCP()::rmi_id.getTCP_Object() == null입니다.");
        }

        if (data == null || data.Length <= 0)
        {
            throw new ArgumentException("sendByte_TCP()::길이가 0인 데이터나 null은 전송할 수 없습니다.");
        }

        if (!(RMI_Context.Reliable <= rmi_ctx && rmi_ctx <= RMI_Context.UnReliable_Public_AES_256))
        {
            throw new ArgumentException("sendByte_TCP()::RMI_Context 값이 잘못되었습니다. rmi_ctx = " + rmi_ctx);
        }

        if (!(0 < packetType))
        {
            throw new ArgumentException("sendByte_TCP()::RMI_PacketType 값이 잘못되었습니다. packetType = " + packetType);
        }

        if (rmi_id.Equals(RMI_ID.NONE))
        {
            //Default값으로 설정되어있다면
            return;
        }

        //아직 연결상태가 아니라면.
        if (this._networkSession.currentRMIState == RMI_State.Disable)
        {
            return;
        }

        IByteBuffer sendData = alloc.DirectBuffer(32768);

        sendData.WriteIntLE(data.Length);
        sendData.WriteIntLE(this._networkSession.MYSELF.rmi_host_id);
        sendData.WriteShortLE((short)rmi_ctx);
        sendData.WriteShortLE((short)packetType);
        sendData.WriteBytes(data);

        this._networkSession.SendByte(sendData);
    }

    //송신 로직. UDP
    public void sendByte_UDP(RMI_ID rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] data)
    {
        if (rmi_id == null)
        {
            throw new ArgumentException("sendByte_UDP()::RMI_ID는 null일 수 없습니다.");
        }

        if (rmi_id.getUDP_Object() == null)
        {
            throw new ArgumentException("sendByte_UDP()::rmi_id.getUDP_Object() == null입니다.");
        }

        if (data == null || data.Length <= 0)
        {
            throw new ArgumentException("sendByte_UDP()::길이가 0인 데이터나 null은 전송할 수 없습니다.");
        }

        if (!(RMI_Context.Reliable <= rmi_ctx && rmi_ctx <= RMI_Context.UnReliable_Public_AES_256))
        {
            throw new ArgumentException("sendByte_UDP()::RMI_Context 값이 잘못되었습니다. rmi_ctx = " + rmi_ctx);
        }

        if (!(0 < packetType))
        {
            throw new ArgumentException("sendByte_UDP()::RMI_PacketType 값이 잘못되었습니다. packetType = " + packetType);
        }

        if (rmi_id.Equals(RMI_ID.NONE))
        {
            //Default값으로 설정되어있다면
            return;
        }

        //아직 연결상태가 아니라면.
        if (this._networkSession.currentRMIState == RMI_State.Disable)
        {
            return;
        }

        IByteBuffer sendData = alloc.DirectBuffer(32768);

        sendData.WriteIntLE(data.Length);
        sendData.WriteIntLE(this._networkSession.MYSELF.rmi_host_id);
        sendData.WriteShortLE((short)rmi_ctx);
        sendData.WriteShortLE((short)packetType);
        sendData.WriteBytes(data);

        this._networkSession.udpSendByte(sendData);
    }


    //송신 로직. TCP
    public void sendByte_TCP(RMI_ID[] rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] data)
    {
        //아직 연결상태가 아니라면.
        if (this._networkSession.currentRMIState == RMI_State.Disable)
        {
            throw new Exception("RMI_Connection : 현재 송수신 가능한 상태가 아닙니다");
            return;
        }
        Console.WriteLine("sendByte_TCP 범위 대상 전송은 지원하지 않습니다.");
    }
    //송신 로직. UDP
    public void sendByte_UDP(RMI_ID[] rmi_id, RMI_Context rmi_ctx, RMI_PacketType packetType, byte[] data)
    {
        //아직 연결상태가 아니라면.
        if (this._networkSession.currentRMIState == RMI_State.Disable)
        {
            throw new Exception("RMI_Connection : 현재 송수신 가능한 상태가 아닙니다");
            return;
        }
        Console.WriteLine("sendByte_UDP 범위 대상 전송은 지원하지 않습니다.");
    }


    //수신 로직.
    //암호화 키를 찾기위한 object, byte[] 로부터 가져온 PacketType, ContextType, 그리고 RMI_Data를 가져옴.
    //맨앞의 size 4byte부분은 제외됨.
    public void recvByte(int rmi_host_id, short rmi_ctx, short packetType, byte[] data, EndPoint UDP_Sender, IChannelHandlerContext ctx)
    {
        //RMI 커넥션 초기화 패킷의 경우.
        if ((short)RMI_ConnectionPacketType.RMI_ProtocolVersionCheck <= packetType && packetType <= (short)RMI_ConnectionPacketType.RMI_OverConnectionAnnounce)
        {
            switch (packetType)
            { 
                case (short)RMI_ConnectionPacketType.RMI_ProtocolVersionCheck: //Protocol Version 체크시.
                                                                  //클라이언트에서는 해당사항 없음.
                    Console.WriteLine("잘못된 패킷! 연결을 종료합니다.");
                    this._networkSession.Disconnect();
                    break;
                case (short)RMI_ConnectionPacketType.RMI_RSA_PublicKey: //서버로부터 RSA 공개키 데이터 수신시.

                    RMI_RSA_PublicKey recvPublicKey = null;
                    try
                    {
                        //고정된 대칭키로 복호화!
                        byte[] recvData_ =
                            RMI_EncryptManager.decryptAES_256(data, "RMI_Connection_Protocol", "RMI_Connection_Protocol");

                        //서버로부터 공개키를 받았으므로, 자신의 AES키를 수신한 공개키로 암호화! 
                        recvPublicKey = RMI_RSA_PublicKey.CreateRMI_RSA_PublicKey(recvData_);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("잘못된 패킷! 연결을 종료합니다.");
                        this._networkSession.Disconnect();
                        break;
                    }

                    if (recvPublicKey != null)
                    {
                        //base64Encoding된 공개키.
                        string base64EncodedPublicKey = recvPublicKey.base64Encoded_publicKey;

                        //클라이언트의 AES키 데이터 준비.
                        RMI_Send_EncryptedAES_Key rmi_Send_EncryptedAES_Key = new RMI_Send_EncryptedAES_Key();
                        rmi_Send_EncryptedAES_Key.RSAEncrypted_AESKey = Convert.FromBase64String(this._networkSession.MYSELF.AESKey.AES_Key);
                        rmi_Send_EncryptedAES_Key.RSAEncrypted_AESIV = Convert.FromBase64String(this._networkSession.MYSELF.AESKey.AES_IV);

                        //클라이언트의 AES키 데이터를 수신한 공개키로 암호화!
                        byte[] encryptedData = RMI_EncryptManager.encryptRSA_public(
                            rmi_Send_EncryptedAES_Key.getBytes(),
                            RMI_EncryptManager.getPublicKey(base64EncodedPublicKey, 2048)
                            );

                        //클라이언트의 AES키 암호화 데이터 송신 준비.
                        IByteBuffer sendData = alloc.DirectBuffer(32768);
                        sendData.WriteIntLE(encryptedData.Length);
                        sendData.WriteIntLE(this._networkSession.MYSELF.rmi_host_id);
                        sendData.WriteShortLE((short)RMI_Context.Reliable);
                        sendData.WriteShortLE((short)RMI_ConnectionPacketType.RMI_Send_EncryptedAES_Key);
                        sendData.WriteBytes(encryptedData);

                        //송신.
                        this._networkSession.SendByte(sendData);
                    }
                    break;
                case (short)RMI_ConnectionPacketType.RMI_Send_EncryptedAES_Key: //클라로부터, 서버가 보낸 공개키에 의해 암호화된 AES대칭키 데이터 수신시.
                                                                   //클라이언트에서는 해당사항 없음.
                    Console.WriteLine("잘못된 패킷! 연결을 종료합니다.");
                    this._networkSession.Disconnect();
                    break;
                case (short)RMI_ConnectionPacketType.RMI_Send_EncryptedAccept_Data: //서버로부터 AES키 및 RMI_HostID, UDP initPort등의 정보를 수신시.

                    RMI_Send_EncryptedAccept_Data recvAcceptData = null;
                    try
                    {
                        //복호화.
                        byte[] recvData_ =
                            RMI_EncryptManager.decryptAES_256(data, this._networkSession.MYSELF.AESKey.AES_Key, this._networkSession.MYSELF.AESKey.AES_IV);

                        //받은 byte[] 데이터를 객체로 변환!
                        recvAcceptData = RMI_Send_EncryptedAccept_Data.CreateRMI_Send_EncryptedAccept_Data(recvData_);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("잘못된 패킷! 연결을 종료합니다.");
                        this._networkSession.Disconnect();
                        break;
                    }

                    if (recvAcceptData != null)
                    {
                        //RMI_HostID 세팅!
                        this._networkSession.MYSELF.rmi_host_id = recvAcceptData.RMI_HostID;

                        //서버의 공용AES암호화키 등록!
                        RMI_EncryptManager.Public_AES_Keys.setAES_Key(Encoding.UTF8.GetString(recvAcceptData.AESEncrypted_PublicAESKey) + "§" + Encoding.UTF8.GetString(recvAcceptData.AESEncrypted_PublicAESIV));

                        //RMI_ID에 소켓채널 할당.
                        this._networkSession.MYSELF.setTCP_Object(this._networkSession.getTCPChannel());
                        this._networkSession.MYSELF.setUDP_Object(this._networkSession.getUDPChannel());
                        this._networkSession.MYSELF.setTCP_Object(this._networkSession.getTCPChannel());
                        this._networkSession.MYSELF.setUDP_Object(this._networkSession.getUDPChannel());
                        
                        //할당받은 UDP포트로 바인딩!
                        this._networkSession.initUDPConnection(recvAcceptData.UDP_InitPort);
                    }

                    break;

                case (short)RMI_ConnectionPacketType.RMI_OverConnectionAnnounce: //접속자 제한을 넘겼을 경우.
                    Console.WriteLine("최대 접속자수 제한을 넘었습니다. 잠시후 다시 접속하시기 바랍니다. 연결을 해제합니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 패킷! 연결을 종료합니다.");
                    this._networkSession.Disconnect();
                    break;
            }
            
            return;
        }

        //수신시 설정
        this._networkSession.serverToClient.recvRMI_Method(this._networkSession.MYSELF, (RMI_Context)rmi_ctx, (RMI_PacketType)packetType, data);
    }

    //================================================================================
    //직접적으로 네트워크 송수신 함수 부분과 접하는 지점.


    //호출되었을 때 실행되는 부분. 로그 기록에 사용되는 용도
    //이 부분을 수동으로 작성할 필요가 있음.
    //================================================================================

    public void onRMI_Call(RMI_Context rmi_ctx, RMI_PacketType packetType, object LogData)
    {
        //로그 기록 설정여부 판단.
        if (!isLogEnable)
            return;

        //로그 작업
        string log = getCurrentTime() + " onRMI_Call:" + packetType + " " + rmi_ctx + "]\n" + LogData;
        Console.WriteLine(log);
    }

    public void onRMI_Recv(RMI_Context rmi_ctx, RMI_PacketType packetType, object LogData)
    {
        //로그 기록 설정여부 판단.
        if (!isLogEnable)
            return;

        //로그 작업
        string log = getCurrentTime() + " onRMI_Recv:" + packetType + " " + rmi_ctx + "]\n" + LogData;
        Console.WriteLine(log);
    }

    //================================================================================
    //호출되었을 때 실행되는 부분. 로그 기록에 사용되는 용도.




    //RMI 암호화용 클래스.
    private RMI_EncryptManager RMI_ENCRYPT = new RMI_EncryptManager();

    //버퍼 재사용을 위한 버퍼Pool 할당을 위한 객체. 이로서 버퍼 생성, 삭제에 많은 비용을 들일 필요없이 재사용을 하여
    //자원 절약을 통해 성능 향상에 도움이 된다!
    PooledByteBufferAllocator alloc = PooledByteBufferAllocator.Default;

    //날짜 문자열 출력용 메소드.
    public static string getCurrentTime()
    {
        DateTime currentDate = DateTime.Now;
        return "[" + currentDate.ToString("yyyy.MM.dd HH:mm:ss.SSS") + "]";
    }
}
