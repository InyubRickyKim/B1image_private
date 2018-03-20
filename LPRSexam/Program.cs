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
using Common.QMgr;
using LPRSProtocol.Common;

namespace LPRSexam
{
    /*public struct ST_SESS_TIMER
    {
        public SOCK_INFO stSrvInfo;
        public SOCK_INFO stCliInfo;
        public System.Timers.Timer objSessChckTiemr;
        //reconnDelegate dlgReconn;
        //int nSessInterval;
    };*/
    class Program
    {
        public delegate void recvMsgDelegate(ref SOCK_INFO stSock);
        public delegate void sendMsgDelegate(ref SOCK_INFO stSock, byte[] szMsg, int nMsgLength);
        //reconnDelegate dlgReconn;
        //ST_SESS_TIMER stSessTimer = new ST_SESS_TIMER();
        //ST_SESS_TIMER stSessTimer;

        static private SOCK_INFO stSrvSock;
        static private SOCK_INFO stCliSock;

        static private SOCKET_MGR objSockMgr;
        static private Q_MGR objQMgr;

        static private ST_THRD_MGR stSndrThrd;

        //static private Thread objSndThrd;

        //static var objSndrQ;

        //static private object objSndrQ;

        static System.Timers.Timer objSessChkTimer;

        //int gnSessInterval;
        static void Main(string[] args)
        {
            //stSrvSock = new SOCK_INFO();
            //stCliSock = new SOCK_INFO();
            //stSndrThrd = new ST_THRD_MGR();

            objSockMgr = new SOCKET_MGR();
            objQMgr = new Q_MGR();

            THREAD_MGR objThrdMgr = new THREAD_MGR();

            //objSndrQ = new object();
            //var objSndrQ = new ConcurrentQueue<string>();
            stSndrThrd.objLock = new object();
            stSndrThrd.objQueue = new ConcurrentQueue<string>();
            stSndrThrd.stSock = stCliSock;

            object objSndThrd = new object();

            double ldInterval = 1000;           // 1000 msec

            stSrvSock.objLock = new object();
            stCliSock.objLock = new object();

            //objQMgr.initByteConcurrentQueue(ref objSndrQ);

            objSockMgr.connSocket(ref stSrvSock, ref stCliSock);

            setSessionCheckTimer(ldInterval);

            objThrdMgr.runSendThrd(stSndrThrd, ref objSndThrd);

            stSndrThrd.objThrd = objSndThrd;

            doMainThread();

            // working main thread
            objSockMgr.closeSocket(ref stSrvSock);
            objSockMgr.closeSocket(ref stCliSock);
        }
        
        //private static void runSendThrd(ST_THRD_MGR stThrd)

        private static void doMainThread()
        {
            __RECOG_RESULT stRecogResult = new __RECOG_RESULT();
            __STAT_INFO stStatInfo = new __STAT_INFO();

            PROTOCOL_COMMON objProtCmn = new PROTOCOL_COMMON();
            RECOGRESULT objRecogResult = new RECOGRESULT();
            STATINFO objStatInfo = new STATINFO();

            ConcurrentQueue<string> objSndrQ = (ConcurrentQueue < string > )stSndrThrd.objQueue;

            byte[] szRecvBuff = new byte[1024];

            int nRecvLength = 1024;
            int nRetVal = 0;
            string strRetStr;

            while (true)
            {
                if (stCliSock.bConn == false)
                {
                    Thread.Sleep(100);
                    continue;
                }

                Array.Clear(szRecvBuff, 0, nRecvLength);
                szRecvBuff = objSockMgr.recvByteMsg(ref stCliSock, ref nRecvLength);
                if (nRecvLength <= 0)
                {
                    Thread.Sleep(500);
                    Console.WriteLine("Receive NO-DATA{0}", nRecvLength);
                    continue;
                }
                lock (stCliSock.objLock)
                {
                    stCliSock.lnResetTime = DateTime.UtcNow.Ticks;
                    stCliSock.bConn = true;
                }
                nRetVal = objProtCmn.chkMsg(szRecvBuff, nRecvLength);
                if (nRetVal > (int)PROTOCOL_COMMON.E_PROTOCOL_RET.SUCCESS)
                {
                    //objLtoS.getLprsMsg(szRecvBuff, nRecvLength);
                    strRetStr = "";
                    strRetStr = chkInstruction(szRecvBuff, ref stCliSock);
                    if (strRetStr.Contains("LPRS|"))
                    {
                        objRecogResult.getRecogResult(ref stRecogResult, szRecvBuff);
                        objSndrQ.Enqueue("LPRS");
                    }
                    else if(strRetStr.Contains("ST|"))
                    {
                        objStatInfo.getStatInfo(ref stStatInfo, szRecvBuff);
                    }
                    else
                    {
                        Console.WriteLine(strRetStr);
                        //Console.WriteLine(Encoding.ASCII.GetString(szRecvBuff));
                    }
                }
                else
                {
                    Console.WriteLine(Encoding.ASCII.GetString(szRecvBuff));
                    continue;
                }
            }
        }

        private static string chkInstruction(byte[] szRecvBuff, ref SOCK_INFO stSock)
        {
            string strBuff;
            string strRet;
            strBuff = Encoding.ASCII.GetString(szRecvBuff);

            if (strBuff.Contains("LPRS|"))
            {
                //objRecogResult.getRecogResult(ref stRecogResult, ref szRecvBuff);
                strRet = "LPRS|";
            }
            else if (strBuff.Contains("ST|"))
            {
                // re-up coonnection check flag
                lock (stSock.objLock)
                {
                    stSock.lnResetTime = DateTime.UtcNow.Ticks;
                    stSock.bConn = true;
                }

                //objStatInfo.getStatInfo(ref stStatInfo, ref szRecvBuff);
                strRet = "ST|";
            }
            else
                strRet = "INVALID_STRUCTION";

            return strRet;
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
            //Console.WriteLine("Session Check Timer Start!");
        }

        //public void reconnTimer(object sender, ref SOCKET_MGR objSockMgr, ref SOCK_INFO stSrvSock, ref SOCK_INFO stCliSock)
        private static void reconnTimer(object sender, ElapsedEventArgs e)
        {
            objSockMgr.reconnSocket(ref stSrvSock, ref stCliSock);
            //Console.WriteLine("Session Check Timer Restart!(Flag : {0})", stCliSock.bConn == true ? "True" : "FALSE");
            objSessChkTimer.Start();
        }
    }
}
