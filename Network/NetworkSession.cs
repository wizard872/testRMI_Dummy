using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DisruptorUnity3d;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

//NetworkManager
public class NetworkSession
{
    //접속정보
    public string type; //접속주소 타입 지정.  문자열로 "ip"  (192.168.99.111 형식)또는 "domain"  (naver.com 형식)지정가능. 
    public string ip; //아이피 주소
    public string domain; //도메인 주소
    public int port; //접속 포트

    //테스트용 임시변수.
    public RMI rmiParser;
    public server_to_client serverToClient;
    public client_to_server clientToServer;
    public RMI_ID MYSELF;

    //네트워크 이벤트 처리용 부트스트랩.
    Bootstrap tcpBootstrap;
    Bootstrap udpBootstrap;

    //통신용 소켓채널.
    IChannel tcpChannel;
    IChannel udpChannel;

    //목적지 지정.
    IPEndPoint serverAddress_TCP;
    IPEndPoint serverAddress_UDP;

    //통신상태 관련 변수들.
    public TCPClientState currentState;
    public UDPClientState currentUDPState;
    public RMI_State currentRMIState;

    //메인스레드 통신용 Queue. (임시)
    public PacketQueue recv_Queue;

    //버퍼 재사용을 위한 PooledByteBufferAllocator 객체.
    public PooledByteBufferAllocator alloc = PooledByteBufferAllocator.Default;

    //디바이스 고유 문자열ID (test)
    public string deviceUniqueId;

    public NetworkSession(string ip, int port)
    {
        
        this.rmiParser = new RMI(this);
        this.serverToClient = new server_to_client(this);
        this.clientToServer = new client_to_server(this);
        this.MYSELF = new RMI_ID(0, -10, null, null);

        this.type = "ip";
        this.ip = ip;
        this.domain = "test.com"; //example domain address
        this.port = port;

        deviceUniqueId = "[deviceUniqueId test]";

        currentState = TCPClientState.Disconnected; //초기상태는 disconnected 이다!
        currentUDPState = UDPClientState.Disconnected; //초기상태는 disconnected 이다!
        currentRMIState = RMI_State.Disable; //초기상태는 disable 이다!

        //RMI 현재 시각 호출
        string time = RMI.getCurrentTime();

        //수신큐 지정.
        recv_Queue = new PacketQueue();
    }


    //RMI 연결이 끝나는 시점에 호출됨
    public void isConnectCompleted()
    {
        Console.WriteLine("requestLoginTest? = " + deviceUniqueId);
        
        //연결 준비가 완료되었으므로, 서버로 데이터 송신!
        //각 인자값으로 [서버에 해당하는 RMI_ID], [송신방식(tcp or udp & 평문 or 암호화)], [데이터 인자값....] 가 들어간다.
        this.clientToServer.sendTestDataToServer(RMI_ID.SERVER, RMI_Context.Reliable_AES_256, 0f, deviceUniqueId);
    }

    public IChannel getTCPChannel()
    {
        return tcpChannel;
    }

    public IChannel getUDPChannel()
    {
        return udpChannel;
    }

    //연결 종료하기.
    public void Disconnect()
    {
        if (currentState != TCPClientState.Disconnected)
        {
            Console.WriteLine("Disconnect() 호출됨.");

            currentRMIState = RMI_State.Disable;
            currentState = TCPClientState.Disconnected;
            
            try
            {
                if (tcpChannel != null)
                {
                    Task disconnectTCP = tcpChannel.CloseAsync();
                    
                    disconnectTCP.GetAwaiter().OnCompleted(() =>
                    {
                        tcpChannel = null;
                    });
                }

                if (udpChannel != null)
                {
                    Task disconnectUDP = udpChannel.CloseAsync();
                    
                    disconnectUDP.GetAwaiter().OnCompleted(() =>
                    {
                        udpChannel = null;
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Disconnect(error) : " + e.StackTrace);
            }
            finally
            {
                serverAddress_TCP = null;
                serverAddress_UDP = null;

                recv_Queue.Clear();
                RMI_ID.resetRMI_ID();

                Console.WriteLine("Disconnect() 종료됨.");
            }
        }
    }


    //서버로 접속
    public void ConnectServer()
    {
        try
        {
            if (currentState == TCPClientState.Disconnected) //현재 연결상태가 아니라면 연결 시도!
            {
                currentState = TCPClientState.Connecting; //접속 시도중이므로 Connecting 상태로 변경!

                if (this.type.Trim().Equals("ip"))
                {
                    serverAddress_TCP = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
                    serverAddress_UDP = new IPEndPoint(IPAddress.Parse(this.ip), 0);
                }
                else if (this.type.Trim().Equals("domain"))
                {
                    serverAddress_TCP = new IPEndPoint(Dns.GetHostEntry(this.domain).AddressList[0], this.port);
                    serverAddress_UDP = new IPEndPoint(Dns.GetHostEntry(this.domain).AddressList[0], 0);
                }
                else
                {
                    throw new Exception("Error: ConnectGameServer()의 첫번째 인자의 타입을 'ip' 또는 'domain' 중 하나만 입력하십시오.");
                }

                //TCP 연결 시도.
                Task<IChannel> tcpChannelConnectTask = tcpBootstrap.ConnectAsync(serverAddress_TCP);

                //작업이 완료되었을 경우 실행되는 부분.
                tcpChannelConnectTask.GetAwaiter().OnCompleted(() =>
                {
                    try
                    {
                        tcpChannel = tcpChannelConnectTask.Result;

                        //RMI 통신을 위한 Accept 요청 패킷 생성. 서버와의 프로토콜 버전체크.
                        RMI_ProtocolVersionCheck requestData = new RMI_ProtocolVersionCheck();
                        requestData.rmi_protocol_version = RMI.rmi_protocol_version; //프로토콜 버전값 세팅.
                        
                        //고정된 대칭키로 암호화!
                        byte[] requestData_ = RMI_EncryptManager.encryptAES_256(requestData.getBytes(),
                            "RMI_Connection_Protocol", "RMI_Connection_Protocol");


                        IByteBuffer sendData = PooledByteBufferAllocator.Default.DirectBuffer(32768);

                        sendData.WriteIntLE(requestData_.Length);
                        sendData.WriteIntLE(MYSELF.rmi_host_id);
                        sendData.WriteShortLE((short) RMI_Context.Reliable);
                        sendData.WriteShortLE((short) RMI_ConnectionPacketType.RMI_ProtocolVersionCheck);
                        sendData.WriteBytes(requestData_);

                        //요청 송신!
                        SendByte(sendData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("TCP Connect Error:" + e.StackTrace);
                        Disconnect();
                    }
                });
            }
            else if (currentState == TCPClientState.Connecting)
            {
                Console.WriteLine("Error: 이미 연결 시도중인 상태입니다.");
            }
            else //이미 연결중입니다 안내 메시지 표시!
            {
                Console.WriteLine("Error: 이미 연결된 상태입니다.");
            }
        }
        catch (AggregateException e)
        {
            Console.WriteLine("연결 시도중 에러! Error:" + e.StackTrace);
            Disconnect();
        }
        catch (Exception e)
        {
            Console.WriteLine("연결 시도중 에러! Error:" + e.StackTrace);
            Disconnect();
        }
    }

    //추후, UDP포트를 할당받으면 
    public void initUDPConnection(int setPort = -1)
    {
        try
        {
            //지정된 포트가 존재한다면 해당 포트로 연결 시도!
            if (setPort != -1)
                serverAddress_UDP.Port = setPort;

            Task<IChannel> udpBootstrapConnectTask = udpBootstrap.ConnectAsync(serverAddress_UDP);
            udpBootstrapConnectTask.GetAwaiter().OnCompleted(() =>
            {
                try
                {
                    udpChannel = udpBootstrapConnectTask.Result;

                    //UDP 통신상태 체크!
                    RMI_UDP_ConnectionConfirm checkUDP = new RMI_UDP_ConnectionConfirm();
                    checkUDP.checkUDP_Connection = ((IPEndPoint)getUDPChannel().LocalAddress).Port;
                    byte[] udpCheck = checkUDP.getBytes();

                    byte[] encryptedData = RMI_EncryptManager.encryptAES_256
                        (udpCheck, MYSELF.AESKey.AES_Key, MYSELF.AESKey.AES_IV);

                    //클라이언트의 AES키 암호화 데이터 송신 준비.
                    IByteBuffer sendData = alloc.DirectBuffer(32768);
                    sendData.WriteIntLE(encryptedData.Length);
                    sendData.WriteIntLE(MYSELF.rmi_host_id);
                    sendData.WriteShortLE((short)RMI_Context.UnReliable);
                    sendData.WriteShortLE((short)RMI_ConnectionPacketType.RMI_UDP_ConnectionConfirm);
                    sendData.WriteBytes(encryptedData);

                    //UDP 체크 패킷 송신.
                    udpSendByte(sendData.RetainedDuplicate());
                    udpSendByte(sendData);

                    currentRMIState = RMI_State.Available;

                    Console.WriteLine("[RMI 통신 준비완료]");

                    //RMI 연결이 끝나는 시점에 호출!
                    isConnectCompleted();
                }
                catch (Exception e)
                {
                    Disconnect();
                    Console.WriteLine("UDP Connect Error:" + e.StackTrace);
                }
            });
        }
        catch (Exception e)
        {
            Disconnect();
            Console.WriteLine("UDP Bind Error:" + e.StackTrace);
        }
    }

    //tcp 송신
    public void SendByte(IByteBuffer send_data)
    {
        if (send_data == null)
            return;
        try
        {
            //송신이 불가능한 상황이라면
            if (tcpChannel == null || tcpChannel.Active != true || tcpChannel.IsWritable != true ||
                currentState != TCPClientState.Connected)
            {
                send_data.Release();
                return;
            }
            //송신이 가능하다면
            else
            {
                //비동기 송신
                tcpChannel.WriteAndFlushAsync(send_data);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("NetworkSession SendByte" + e.StackTrace);
            Disconnect();
        }
    }

    //udp 송신
    public void udpSendByte(IByteBuffer send_data)
    {
        if (send_data == null)
            return;
        try
        {
            //송신이 불가능한 상황이라면
            if (udpChannel == null || udpChannel.Active != true || udpChannel.IsWritable != true ||
                currentState != TCPClientState.Connected || currentUDPState != UDPClientState.Connected)
            {
                send_data.Release();
                return;
            }
            //송신이 가능하다면
            else
            {
                DatagramPacket udp_data =
                    new DatagramPacket(send_data, udpChannel.LocalAddress, udpChannel.RemoteAddress);
                
                //비동기 송신
                udpChannel.WriteAndFlushAsync(udp_data);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("NetworkSession udpSendByte" + e.StackTrace);
            Disconnect();
        }
    }

    //이벤트 루프 그룹 설정.
    //cpu thread수 * 3배만큼 eventloop 할당. (thread 역할)
    public static IEventLoopGroup workerGroup = new MultithreadEventLoopGroup( Environment.ProcessorCount * 3 );
    
    //네트워크 작업용 스레드.
    public NetworkSession initDotNetty()
    {
        //네트워크 이벤트 처리용 부트스트랩.
        tcpBootstrap = new Bootstrap();
        udpBootstrap = new Bootstrap();

        try
        {
            tcpBootstrap
                .Group(NetworkSession.workerGroup)
                .Channel<TcpSocketChannel>() //TCP 소켓채널 지정.
                .Option(ChannelOption.SoLinger, 0) //소켓이 끊겼을 때, 소켓에 남아있는 데이터 처리방법지정.
                .Option(ChannelOption.TcpNodelay, true)

                .Option(ChannelOption.SoRcvbuf, 65536) //64KB 수신버퍼
                .Option(ChannelOption.SoSndbuf, 65536) //64KB 수신버퍼
                
                .Option(ChannelOption.RcvbufAllocator, new FixedRecvByteBufAllocator(32768)) //1회 수신시 최대크기 지정.
                .Option(ChannelOption.Allocator, PooledByteBufferAllocator.Default)
                //.Handler(new LoggingHandler())
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel => //ISocketChannel
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    //pipeline.AddLast(new LoggingHandler(LogLevel.INFO));
                    pipeline.AddLast( new TCP_InBoundHandler(this) );
                }));

            udpBootstrap
                .Group(NetworkSession.workerGroup)
                .Channel<SocketDatagramChannel>() //UDP 소켓채널 지정.
                .Option(ChannelOption.SoRcvbuf, 65536) //64KB 수신버퍼
                .Option(ChannelOption.SoSndbuf, 65536) //64KB 수신버퍼
                
                .Option(ChannelOption.RcvbufAllocator, new FixedRecvByteBufAllocator(32768)) //1회 수신시 최대크기 지정.
                .Option(ChannelOption.Allocator, PooledByteBufferAllocator.Default)
                //.Handler(new LoggingHandler())
                .Handler(new ActionChannelInitializer<IDatagramChannel>(channel => //IDatagramChannel
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    //pipeline.AddLast(new LoggingHandler(LogLevel.INFO));
                    pipeline.AddLast( new UDP_InBoundHandler(this) );
                }));

            Console.WriteLine("NetworkSession:DotNetty Initialize Complete");
        }
        catch (Exception e)
        {
            Console.WriteLine("NetworkSession Exception:" + e.StackTrace);
        }
        return this;
    }
} //클래스 종료 부분.


//클라이언트 상태지정을 위한 것들.
public enum TCPClientState
{
    Disconnected, //비연결 상태
    Connecting, //연결 시도중인 상태
    Connected
}

public enum UDPClientState
{
    Disconnected, //송수신 불가능 상태
    Connected //송수신 가능 상태
}

public enum RMI_State
{
    Available, //RMI 통신준비가 완료됨
    Disable //RMI 통신준비중
}


public class PacketQueue
{
    private readonly RingBuffer<IByteBuffer> _queue;

    public PacketQueue(int capacity = 2048)
    {
        this._queue = new RingBuffer<IByteBuffer>(capacity);
    }

    public void Enqueue(IByteBuffer data)
    {
        if (data == null)
            throw new NullReferenceException("PacketQueue Enqueue( IByteBuffer data ) == null");

        _queue.Enqueue(data);
    }

    public IByteBuffer Dequeue() //null을 반환할 수도 있음.
    {
        IByteBuffer data;

        if (_queue.TryDequeue(out data))
            return data;
        else
            return null;
    }

    public void Clear()
    {
        IByteBuffer data = null;

        //_queue 비우기.
        while (!(_queue.Count > 0))
        {
            if (_queue.TryDequeue(out data))
                data.Release(data.ReferenceCount);
        }
    }

    public int getCount()
    {
        return _queue.Count;
    }
}