using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

using LPRSProtocol.LPRSToServerMsg;
using LPRSProtocol.ServerToLPRSMsg;
using Common.BerkSocketMgr;
using Common.ThreadMgr;

namespace LPRSexam
{
    
    public struct ST_SESS_TIMER
    {
        public SOCK_INFO stSrvInfo;
        public SOCK_INFO stCliInfo;
        public System.Timers.Timer objSessChckTiemr;
        //reconnDelegate dlgReconn;
        //int nSessInterval;
    }
    class Program
    {
        public delegate void recvMsgDelegate(ref SOCK_INFO stSock);
        //reconnDelegate dlgReconn;
        //ST_SESS_TIMER stSessTimer = new ST_SESS_TIMER();
        //ST_SESS_TIMER stSessTimer;

        static private SOCK_INFO stSrvSock;
        static private SOCK_INFO stCliSock;
        static private SOCKET_MGR objSockMgr;
        static System.Timers.Timer objSessChkTimer;

        //int gnSessInterval;
        static void Main(string[] args)
        {
            //SOCK_INFO stSrvSock = new SOCK_INFO();
            //SOCK_INFO stCliSock = new SOCK_INFO();
            //SOCKET_MGR objSockMgr = new SOCKET_MGR();
            stSrvSock = new SOCK_INFO();
            stCliSock = new SOCK_INFO();
            objSockMgr = new SOCKET_MGR();
            

            double ldInterval = 1000 * 10;
            objSockMgr.connSocket(ref stSrvSock, ref stCliSock);

            setSessionCheckTimer(ldInterval);

            while (true)
            {
                //Thread.Sleep(100);
                objSockMgr.recvMsg(ref stCliSock);
            }
            // working main thread
            objSockMgr.closeSocket(ref stSrvSock);
            objSockMgr.closeSocket(ref stCliSock);
        }

        //private void setSessionCheckTimer(SOCK_INFO stSrvSock, SOCK_INFO stCliSock, int nInterval)
        private static void setSessionCheckTimer(double dInterval)
        {
            //public delegate void reconnDelegate(object sender, ref SOCKET_MGR objSockMgr, ref SOCK_INFO stSrvSock, ref SOCK_INFO stCliSock);
            objSessChkTimer = new System.Timers.Timer();

            objSessChkTimer.Interval = dInterval;
            objSessChkTimer.Elapsed += new ElapsedEventHandler(reconnTimer);
            objSessChkTimer.AutoReset = false;
            //dlgReconn = new reconnDelegate(objSockMgr.reconnSocket);
            objSessChkTimer.Start();
            Console.WriteLine("Session Check Timer Start!");
        }

        //public void reconnTimer(object sender, ref SOCKET_MGR objSockMgr, ref SOCK_INFO stSrvSock, ref SOCK_INFO stCliSock)
        private static void reconnTimer(object sender, ElapsedEventArgs e)
        {
            objSockMgr.reconnSocket(ref stSrvSock, ref stCliSock);
            Console.WriteLine("Session Check Timer Restart!(Flag : {0})", stCliSock.bConn == true ? "True" : "FALSE");
            objSessChkTimer.Start();
        }
    }
}
