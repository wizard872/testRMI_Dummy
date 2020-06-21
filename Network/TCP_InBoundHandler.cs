using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

public class TCP_InBoundHandler : ByteToMessageDecoder, IChannelHandler
{
    private NetworkSession _networkSession;

    public TCP_InBoundHandler(NetworkSession networkSession)
    {
        this._networkSession = networkSession;
    }
    
    public override void ChannelRegistered(IChannelHandlerContext context) { } //Console.WriteLine("ChannelRegistered");
    public override void ChannelUnregistered(IChannelHandlerContext context) { } //Console.WriteLine("ChannelUnregistered");   

    //TCP채널 활성화시 Dotnetty에서 호출되는 콜백 함수.
    void IChannelHandler.ChannelActive(IChannelHandlerContext context)
    {
        Console.WriteLine("NetworkManager:TCP ChannelActive! LocalAddress=" + context.Channel.LocalAddress + " / RemoteAddress=" + context.Channel.RemoteAddress);

        //활성화된 TCP채널을 networkManager의 RMI_ID_MYSELF 에 등록.
        this._networkSession.MYSELF.setTCP_Object(context.Channel);

        //활성화가 완료되었으므로 완료 상태로 전환함!
        this._networkSession.currentState = TCPClientState.Connected;

        //유저가 접속했을때를 나타내는 이벤트 함수 실행
        OnConnected.onConnected(context);
    }

    //TCP채널 비활성화시 Dotnetty에서 호출되는 콜백 함수.
    void IChannelHandler.ChannelInactive(IChannelHandlerContext context)
    {
        Console.WriteLine("NetworkManager:TCP ChannelInactive!");

        this._networkSession.Disconnect();

        //유저가 접속해제했을때를 나타내는 이벤트 함수 실행
        OnDisconnected.onDisconnected(context);
    }

    //Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> list) 에서,
    //IByteBuffer msg 는 ByteToMessageDecoder가 내부적으로 가지고 있는 누적버퍼이다.
    //데이터 수신시 Dotnetty에서 호출되는 콜백 함수.
    protected internal override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> list)
    {
        //헤더 데이터 부분(12byte)도 다 못읽어왔을 경우!
        if (msg.ReadableBytes < 12)
        {
            return; //다시 되돌려서 패킷을 이어서 더 받아오게끔 하는 부분!
        }

        //readerIndex 저장.
        msg.MarkReaderIndex();

        //앞으로 받아올 패킷크기를 불러온다!
        int packet_size = msg.ReadIntLE();

        //비정상적인 데이터가 도달하였다면.
        if (!isAvailablePacketSize(packet_size))
        {
            Console.WriteLine("[TCP_InBoundHandler] Packet손상!! packet_size = " + packet_size + "");

            //연결 종료
            ctx.Channel.CloseAsync(); //채널이 닫힌다면 msg는 release 된다.
            return;
        }

        //아직 버퍼에 데이터가 충분하지 않다면 다시 되돌려서 패킷을 이어서 더 받아오게끔 하는 부분!
        if (msg.ReadableBytes < packet_size + 8)
        {
            //정상적으로 데이터를 다 읽은것이 아니므로 readerIndex 4 -> 0로 초기화함.
            msg.ResetReaderIndex();
            return;
        }
        //패킷을 완성할만큼 데이터를 받아온 경우
        else
        {
            //패킷 완성후, 다음 데이터가 없는지 체크
            if (!isCompleteSizePacket(packet_size, ctx, msg))
            {
                //다음 데이터가 없다면 다시 이어서 받는다.
                return;
            }
        }
    } //Decode 메소드 종료 부분.


    //패킷 완성후, 다음 데이터가 없는지 체크하는 함수.
    //패킷 완성후, 다시 자기자신을 호출하여 다음 데이터가 없을때까지 계속 자기자신을 호출한다.
    bool isCompleteSizePacket(int packet_size, IChannelHandlerContext ctx, IByteBuffer msg)
    {
        //아직 버퍼에 데이터가 충분하지 않다면 다시 되돌려서 패킷을 이어서 더 받아오게끔 하는 부분!
        if (msg.ReadableBytes < packet_size + 8)
        {
            //정상적으로 데이터를 다 읽은것이 아니므로 readerIndex 4 -> 0로 초기화함.
            msg.ResetReaderIndex();
            return false; //다시 되돌려서 패킷을 이어서 더 받아오게끔 하는 부분!
        }
        //패킷이 뭉쳐서 데이터를 더 받아왔을 경우! 원래 받아야 할 부분만 받고, 나머지는 다시 되돌려서 뒤이어 오는 패킷과 합쳐서 받을 것!
        else if (msg.ReadableBytes > packet_size + 8)
        {
            //헤더 데이터 파싱!

            //rmi host id(int)
            int rmi_id = msg.ReadIntLE();
            //rmi context (short)
            short rmi_ctx = msg.ReadShortLE();
            //rmi packet type (short)
            short packet_type = msg.ReadShortLE();

            //받아온 데이터.
            byte[] packet_data = new byte[packet_size];
            msg.ReadBytes(packet_data, 0, packet_size);

            //RMI 수신로직 처리 부분.
            _networkSession.rmiParser.recvByte(rmi_id, rmi_ctx, packet_type, packet_data, null, ctx); //TCP데이터이므로 UDP Data는 null값!

            //패킷 완성후 남은 데이터가 헤더크기인 12byte보다 작을 경우.
            if (msg.ReadableBytes < 12)
            {
                return false;
            }
            else
            {
                //readerIndex 저장.
                msg.MarkReaderIndex();

                //다음 패킷의 사이즈를 구한다.
                int nextPacketSize = msg.ReadIntLE();

                //비정상적인 데이터가 도달하였는지 여부 판별.
                if (!isAvailablePacketSize(nextPacketSize))
                {
                    //연결 종료
                    ctx.CloseAsync();  //채널이 닫힌다면 msg는 release 된다.
                    return false;
                }

                return isCompleteSizePacket(nextPacketSize, ctx, msg); //재귀함수 처리
                //패킷 완성후, 남은데이터가 없을때까지 계속 반복한다.
            }
        }
        else //정확한 데이터일 경우. [msg.readableBytes() + 4 == packet_size + 12]
        {
            //헤더 데이터 파싱!

            //rmi host id(int)
            int rmi_id = msg.ReadIntLE();
            //rmi context (short)
            short rmi_ctx = msg.ReadShortLE();
            //rmi packet type (short)
            short packet_type = msg.ReadShortLE();

            //받아온 데이터.
            byte[] packet_data = new byte[packet_size];
            msg.ReadBytes(packet_data, 0, packet_size);

            //버퍼에 읽어와야 할 데이터가 남아있지 않으므로 readerIndex, writerIndex를 각각 0으로 초기화 한다.
            //내부 버퍼 데이터를 변경하지 않고 index값만 변경하므로 자원을 적게 소모한다.
            msg.Clear();

            //RMI 수신로직 처리 부분.
            _networkSession.rmiParser.recvByte(rmi_id, rmi_ctx, packet_type, packet_data, null, ctx); //TCP데이터이므로 UDP Data는 null값!

            //다시 되돌려서 패킷을 이어서 더 받아오게끔 하는 부분!
            return false;
        }
    }

    //비정상적인 데이터가 도달하였는지 여부 판별.
    bool isAvailablePacketSize(int packet_size)
    {
        //비정상적인 패킷이라면 false
        if (0 >= packet_size || packet_size > 32768)
            return false;

        //정상적인 패킷이라면 true
        else
            return true;
    }

    //에러 발생시 Dotnetty에서 호출되는 콜백 함수.
    public override void ExceptionCaught(IChannelHandlerContext context, Exception e)
    {
        Console.WriteLine("TCP_InBoundHandler:ExceptionCaught = " + e.ToString());
        context.Channel.CloseAsync();
    }

}//class 종료 부분!
