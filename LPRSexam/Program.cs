using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using LPRSProtocol.LPRSToServerMsg;
using LPRSProtocol.ServerToLPRSMsg;
using Common.BerkSocketMgr;
using Common.ThreadMgr;

namespace LPRSexam
{
    class Program
    {
        //private object objLock = new object();
        static void Main(string[] args)
        {
            SOCK_INFO stSrvSock = new SOCK_INFO();
            SOCK_INFO stCliSock = new SOCK_INFO();
            //Socket objSrvSock = null;
            //Socket objCliSock = null;

            SOCKET_MGR objSockMgr = new SOCKET_MGR();

            objSockMgr.connSocket(ref stSrvSock, ref stCliSock);

            objSockMgr.closeSocket(ref stSrvSock);
            objSockMgr.closeSocket(ref stCliSock);
        }
    }
}
