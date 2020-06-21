using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

class Program
{
    static List<NetworkSession> sessionList = new List<NetworkSession>();
    static int dummyClients;

    static void Main(string[] args)
    {
        
        //개수 지정 (default 100)
        Console.Write("dummyClients 개수 = ");
        string data = Console.ReadLine();

        //잘못된 값이 들어왔다면 100개로 설정
        if (!int.TryParse(data, out dummyClients))
            dummyClients = 100;

        Console.WriteLine("설정된 dummyClients = " + dummyClients);



        
        //ip 지정 (default localhost)
        Console.Write("접속ip = ");
        string ip = Console.ReadLine();

        IPAddress ipcheck;
        if (ip == "" || !IPAddress.TryParse(ip, out ipcheck))
            ip = "127.0.0.1";

        Console.WriteLine("설정된 ip = " + ip);
        
        
        
        //port 지정 (default tcp & udp 12321)
        Console.Write("접속 port = ");

        string ports = Console.ReadLine();

        int port;
        if (!int.TryParse(ports, out port))
            port = 12321;

        Console.WriteLine("설정된 port = " + port);
        Console.WriteLine("press Enter Key");

        Console.ReadLine();

        
        //설정된 개수만큼 생성.
        for (int i = 0; i < dummyClients; i++)
        {
            sessionList.Add(new NetworkSession(ip, port).initDotNetty());
        }

        //3초간 대기.
        Thread.Sleep(3000);
        
        //일괄 초기화 진행.
        for (int i = 0; i < dummyClients; i++)
        {
            sessionList[i].ConnectServer();
        }

        Console.WriteLine("\n\n\n\n[dummyClients = " + dummyClients + " 활성화]\n\n\n\n");
        
        //MainThread 블로킹.
        Thread.CurrentThread.Join();
    }
}