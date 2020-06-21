using System;
using System.Threading;
using System.Collections.Generic;
using DotNetty.Transport.Channels;

//기본적으로 RMI_ID는 단일 지정!
public class RMI_ID
{
    //모든 RMI_Connection 값이 저장된 HashMap. 소켓 주소값을 기준으로 찾음.
    private static Dictionary<IChannel, RMI_ID> RMI_CONNECTION_List = new Dictionary<IChannel, RMI_ID>();

    //모든 RMI_HOST_ID값이 저장된 HashMap. id값을 기준으로 찾음.
    private static Dictionary<int, RMI_ID> RMI_HOST_ID_List = new Dictionary<int, RMI_ID>();

    //모든 RMI_UNIQUE_ID값이 저장된 HashMap. id값을 기준으로 찾음.
    private static Dictionary<int, RMI_ID> RMI_UNIQUE_ID_List = new Dictionary<int, RMI_ID>();
    


    //접속 계정마다 부여되는 고유id.
    private static int rmi_id_count = Int32.MinValue;

    //NONE일 경우, 송신하지 않음! 모든 인자가 null 값.
    public static RMI_ID NONE = new RMI_ID(0, -1, null, null);

    public static RMI_ID ALL = new RMI_ID(0, -2, null, null);

    //ALL일 경우, 서버에게 보낼 수 있는 정보 보유.
    public static RMI_ID SERVER = new RMI_ID(Int32.MaxValue, 0, null, null);

    public static RMI_ID MYSELF = new RMI_ID(0, -10, null, null);


    //고유 RMI_HOST_ID. 커넥션 별로 1개씩 고유값.
    public int rmi_host_id;

    //고유 Unique_ID; //캐릭터 고유 id등을 지정할 것.
    public int unique_id;

    //Channel에 직접쓰는것 보다, ChannelHandlerContext에 쓰는것이 퍼포먼스적으로 더 낫기때문에 추가된 변수.
    private IChannelHandlerContext socketTCPHandler; //클라이언트or서버 channelHandler
    private IChannelHandlerContext socketUDPHandler; //클라이언트or서버 channelHandler
    
    private IChannel socketTCP; //클라이언트or서버 channel
    private IChannel socketUDP; //클라이언트or서버 channel

    //RMI ID별로 가지고있는 Key정보.
    public RMI_EncryptManager.EncryptKeyInfo AESKey;

    //UDP연결작업이 완료되었는지 여부.
    public bool isUDPConnectionAvailable;

    public RMI_ID(int rmi_host_id, int unique_id, IChannel socketTCP, IChannel socketUDP)
    {
        this.rmi_host_id = rmi_host_id;
        this.unique_id = unique_id;
        this.socketTCP = socketTCP;
        this.socketUDP = socketUDP;
        this.AESKey = new RMI_EncryptManager.EncryptKeyInfo();
        this.isUDPConnectionAvailable = false;
    }

    public static void resetRMI_ID()
    {
        RMI_CONNECTION_List.Clear();
        RMI_HOST_ID_List.Clear();
        RMI_UNIQUE_ID_List.Clear();

        rmi_id_count = Int32.MinValue;

        NONE = null;
        ALL = null;
        SERVER = null;
        MYSELF = null;

        NONE = new RMI_ID(0, -1, null, null);
        ALL = new RMI_ID(0, -2, null, null);
        SERVER = new RMI_ID(Int32.MaxValue, 0, null, null);
        MYSELF = new RMI_ID(0, -10, null, null);
        Console.WriteLine("resetRMI_ID()");
    }

    public static int getNewRMI_HOST_ID()
    {
        int value = Interlocked.Increment(ref rmi_id_count);

        //값 초기화.
        if (value == Int32.MaxValue-1)
            rmi_id_count = Int32.MinValue;

        //0일시, 1 반환.
        if (value == 0)
            value = rmi_id_count++;
        return value;
    }

    public static RMI_ID[] getArray(Dictionary<int, RMI_ID>.ValueCollection values)
    {
        RMI_ID[] arr = new RMI_ID[values.Count];
        values.CopyTo(arr, 0);
        return arr;
    }

    public IChannelHandlerContext getTCP_ObjectHandler()
    {
        return this.socketTCPHandler;
    }

    public IChannelHandlerContext getUDP_ObjectHandler()
    {
        return this.socketUDPHandler;
    }
    
    public IChannel getTCP_Object()
    {
        return this.socketTCP;
    }
    public IChannel getUDP_Object()
    {
        return this.socketUDP;
    }

    public void setTCP_ObjectHandler(IChannelHandlerContext obj)
    {
        this.socketTCPHandler = obj;
    }
    public void setUDP_ObjectHandler(IChannelHandlerContext obj)
    {
        this.socketUDPHandler = obj;
    }
    
    public void setTCP_Object(IChannel obj)
    {
        this.socketTCP = obj;
    }
    public void setUDP_Object(IChannel obj)
    {
        this.socketUDP = obj;
    }


    public static RMI_ID createRMI_ID(IChannel newConnection)
    {
        if (RMI_CONNECTION_List.ContainsKey(newConnection))
            throw new Exception("createRMI_ID 잘못된 처리. 이미 RMI ID가 발급된 Key입니다.");

        int hostid = getNewRMI_HOST_ID();
        RMI_ID newRMI_ID = new RMI_ID(hostid, RMI_ID.NONE.unique_id, newConnection, null);
        RMI_CONNECTION_List.Add(newConnection, newRMI_ID);

        //새로 발급되었으므로 Host List에도 등록한다!
        setHostID(hostid, newRMI_ID);
        return newRMI_ID;
    }

    public static void setUniqueID(int unique_id, RMI_ID rmi_ID)
    {
        rmi_ID.unique_id = unique_id;
        if (RMI_UNIQUE_ID_List.ContainsKey(unique_id))
        {
            Console.WriteLine("RMI_Unique_ID : 이미 같은 Key가 존재함. 덮어씌움");
        }

        RMI_UNIQUE_ID_List.Add(unique_id, rmi_ID);
    }

    public static void setHostID(int rmi_host_id, RMI_ID rmi_ID)
    {
        rmi_ID.rmi_host_id = rmi_host_id;
        if (RMI_HOST_ID_List.ContainsKey(rmi_host_id))
        {
            Console.WriteLine("RMI_HOST_ID : 이미 같은 Key가 존재함. 덮어씌움");
        }

        RMI_HOST_ID_List.Add(rmi_host_id, rmi_ID);
    }


    public static RMI_ID findRMI_Connection(IChannel connection)
    {
        if (!RMI_CONNECTION_List.ContainsKey(connection))
        {
            Console.WriteLine("findRMI_Connection : Key가 존재하지 않음!");
            return null;
        }
        else
            return RMI_CONNECTION_List[connection];
    }

    public static RMI_ID findRMI_UNIQUE_ID(int unique_id)
    {
        if (!RMI_UNIQUE_ID_List.ContainsKey(unique_id))
        {
            Console.WriteLine("RMI_Unique_ID : Key가 존재하지 않음!");
            return null;
        }
        else
            return RMI_UNIQUE_ID_List[unique_id];
    }

    public static RMI_ID findRMI_HOST_ID(int rmi_host_id)
    {
        if (!RMI_UNIQUE_ID_List.ContainsKey(rmi_host_id))
        {
            Console.WriteLine("RMI_Unique_ID : Key가 존재하지 않음!");
            return null;
        }
        else
            return RMI_UNIQUE_ID_List[rmi_host_id];
    }

    public static void RemoveRMI_ID(IChannel connection)
    {
        if (RMI_CONNECTION_List.ContainsKey(connection))
        {
            RMI_ID rmi_id = RMI_CONNECTION_List[connection];

            if (RMI_UNIQUE_ID_List.ContainsKey(rmi_id.unique_id))
                RMI_UNIQUE_ID_List.Remove(rmi_id.unique_id);

            if (RMI_HOST_ID_List.ContainsKey(rmi_id.rmi_host_id))
                RMI_HOST_ID_List.Remove(rmi_id.rmi_host_id);

            RMI_CONNECTION_List.Remove(connection);
            rmi_id = null;
        }
        else
            throw new Exception("RemoveRMI_ID : Key가 존재하지 않음. 제거실패");
    }
}
