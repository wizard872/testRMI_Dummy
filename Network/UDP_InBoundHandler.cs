using System;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

class UDP_InBoundHandler : ChannelHandlerAdapter, IChannelHandler
{
    private NetworkSession _networkSession;

    public UDP_InBoundHandler(NetworkSession networkSession)
    {
        this._networkSession = networkSession;
    }

    public override void ChannelRegistered(IChannelHandlerContext context) { } //Console.WriteLine("ChannelRegistered");
    public override void ChannelUnregistered(IChannelHandlerContext context) { } //Console.WriteLine("ChannelUnregistered");   

    //UDP채널 활성화시 Dotnetty에서 호출되는 콜백 함수.
    void IChannelHandler.ChannelActive(IChannelHandlerContext context)
    {
        Console.WriteLine("NetworkManager:UDP ChannelActive! LocalAddress=" + context.Channel.LocalAddress + " / RemoteAddress=" + context.Channel.RemoteAddress);

        //활성화된 UDP채널을 networkManager의 RMI_ID_MYSELF 에 등록.
        this._networkSession.MYSELF.setUDP_Object(context.Channel);

        //활성화가 완료되었으므로 완료 상태로 전환함!
        this._networkSession.currentUDPState = UDPClientState.Connected;
    }

    //UDP채널 비활성화시 Dotnetty에서 호출되는 콜백 함수.
    void IChannelHandler.ChannelInactive(IChannelHandlerContext context)
    {
        Console.WriteLine("NetworkManager:UDP ChannelInactive!");
        
        this._networkSession.currentUDPState = UDPClientState.Disconnected;
    }


    //데이터 수신시 Dotnetty에서 호출되는 콜백 함수.
    void IChannelHandler.ChannelRead(IChannelHandlerContext context, object msg)
    {
        DatagramPacket received_datagram = (DatagramPacket)msg;

        //Console.WriteLine("received_datagram = " + received_datagram.Sender + "/" + received_datagram.Recipient);

        //받아온 데이터그램에서 ByteBuf를 가져온다.
        IByteBuffer receive_data = received_datagram.Content;

        //앞으로 받아올 패킷크기를 불러온다!
        int packet_size = receive_data.ReadIntLE();

        if (65535 < packet_size || packet_size < 0 || (packet_size + 8) != receive_data.ReadableBytes)
        {
            Console.WriteLine("[UDP_InBoundHandler] Packet손상!! packet_size = " + packet_size + "");

            //사용된 메모리를 반환한다!
            received_datagram.Release();
            return;
        }
        
        //헤더 데이터 파싱!
        //rmi host id(int)
        int rmi_id = receive_data.ReadIntLE();
        //rmi context (short)
        short rmi_ctx = receive_data.ReadShortLE();
        //rmi packet type (short)
        short packet_type = receive_data.ReadShortLE();

        //받아온 데이터.
        byte[] packet_data = new byte[packet_size];
        receive_data.ReadBytes(packet_data, 0, packet_size);

        received_datagram.Release();

        //RMI 수신로직 처리 부분.
        this._networkSession.rmiParser.recvByte(rmi_id, rmi_ctx, packet_type, packet_data, received_datagram.Sender, context);
    }

    //에러 발생시 Dotnetty에서 호출되는 콜백 함수.
    public override void ExceptionCaught(IChannelHandlerContext context, Exception e)
    {
        Console.WriteLine("UDP_InBoundHandler" + e.ToString() + "\n\n==========================================\n\n");
        context.Channel.CloseAsync();
    }
} //class 종료 부분!