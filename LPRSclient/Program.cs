using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using LPRSProtocol.LPRSToServerMsg;
using LPRSProtocol.ServerToLPRSMsg;

namespace LPRSclient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket objLocalSock;
            IPEndPoint objEp;

            string strCmd = string.Empty;

            objLocalSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            objEp = new IPEndPoint(IPAddress.Parse("192.168.0.191"), 7010);
            objLocalSock.Connect(objEp);

            Console.Write("Command\n" + "Q : 종료" + "L : 인식결과 전송" + "S : 상태정보 전송");
            while(true)
            {
                strCmd = Console.ReadLine();
                if (strCmd == "Q")
                    break;
                else if( strCmd == "L" )
                {

                }
                else if(strCmd == "S")
                {

                }
                objLocalSock.Send();

                strCmd = string.Empty;
                Console.Write("Command\n" + "Q : 종료" + "L : 인식결과 전송" + "S : 상태정보 전송");
            }
        }
    }
}
