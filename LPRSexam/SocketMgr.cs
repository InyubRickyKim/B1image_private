using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Reflection;
//using System.DateTime;

using LPRSProtocol.LPRSToServerMsg;

namespace Common.BerkSocketMgr
{
    public struct SOCK_INFO
    {
        public Socket objSock;
        public object objLock;
        public bool bConn;
        public long lnResetTime;
    }
    class SOCKET_MGR
    {
        enum ENUM_VALUE_SOCKETMGR : sbyte {
            SUCCESS = 0,
            FAILED_IMPLEMENT_SOCKET,
            FAILED_INIT_ENDPOINT,
            FAILED_BIND,
            RECV_NO_DATA,
            FAILED_NULL_SOCKET
        };
        public int impBerkSocket(ref Socket objSocket, int nAddressFamily, int nSocketType, int nProtocolType)
        {
            try
            {
                objSocket = new Socket((AddressFamily)nAddressFamily,
                    (SocketType)nSocketType, (ProtocolType)nProtocolType);
            }
            catch (SocketException eSock)
            {
                Console.WriteLine("SocketException!! Source : {0}, Message : {1}",
                    eSock.Source, eSock.Message);
                return (int)ENUM_VALUE_SOCKETMGR.FAILED_IMPLEMENT_SOCKET;
            }
            return (int)ENUM_VALUE_SOCKETMGR.SUCCESS;
        }

        public int initEndPint(ref IPEndPoint objEp, string strAddr, int nPortNum)
        {
            try
            {
                objEp = new IPEndPoint(IPAddress.Parse(strAddr), nPortNum);
            }
            catch (ArgumentNullException eArgNull)
            {
                Console.WriteLine("ArgumentNullException!! Source : {0}, Message : {1}",
                    eArgNull.Source, eArgNull.Message);
                return (int)ENUM_VALUE_SOCKETMGR.FAILED_INIT_ENDPOINT;
            }
            catch (ArgumentOutOfRangeException eArgORange)
            {
                Console.WriteLine("ArgumentOutOfRangeException!! Source : {0}, Message : {1}",
                    eArgORange.Source, eArgORange.Message);
                return (int)ENUM_VALUE_SOCKETMGR.FAILED_INIT_ENDPOINT;
            }
            return (int)ENUM_VALUE_SOCKETMGR.SUCCESS;
        }

        public int bindLocalEp(ref Socket objSock, ref IPEndPoint objEp)
        {
            try
            {
                objSock.Bind(objEp);
            }
            catch (Exception eCmn)
            {
                Console.WriteLine("Exception!! Source : {0}, Message : {1}",
                    eCmn.Source, eCmn.Message);
                return (int)ENUM_VALUE_SOCKETMGR.FAILED_BIND;
            }

            return (int)ENUM_VALUE_SOCKETMGR.SUCCESS;
        }

        //public void SocketMgr(ref Socket objSrvSock, ref Socket objCliSock)
        //public void connSocket(ref Socket objSrvSock, ref Socket objCliSock)
        public void connSocket(ref SOCK_INFO stSrvSock, ref SOCK_INFO stCliSock)
        {
            IPEndPoint objEp = null;

            SOCKET_MGR objSocketMgr = new SOCKET_MGR();

            //ArrayList lstListen = new ArrayList();
            //ArrayList lstAccept = new ArrayList();

            int nBackLog = 5;
            int lnPortNum = 7007;
            string strLocalIpAddr = "192.168.0.191";

            //stSrvSock.bConn = false;
            stCliSock.bConn = false;

            if ((stSrvSock.objSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) == null)
                return;

            if ((objEp = new IPEndPoint(IPAddress.Parse(strLocalIpAddr), lnPortNum)) == null)
                return;

            stSrvSock.objSock.Bind(objEp);

            stSrvSock.objSock.Listen(nBackLog);

            //Socket.Select(lstListen, null, null, 50000);
            Console.WriteLine("Now waiting CLIENT connect!");
            stCliSock.objSock = stSrvSock.objSock.Accept();

            // up to connection flag
            stCliSock.bConn = true;
            stSrvSock.bConn = true;

            stCliSock.lnResetTime = DateTime.UtcNow.Ticks;
            stSrvSock.lnResetTime = DateTime.UtcNow.Ticks;
            //stCliSock.lnResetTime = new DateTime.
            Console.WriteLine("Socket accept SUCCESS! {0}", stCliSock.lnResetTime);
            return;
        }

        public void reconnSocket(ref SOCK_INFO stSrvSock, ref SOCK_INFO stCliSock)
        {
            double dTimeGap;
            dTimeGap = (DateTime.UtcNow.Ticks - stCliSock.lnResetTime) / 10000.0f;
            //Console.WriteLine("Timer Ticks : {0}", DateTime.UtcNow.Ticks);
            if (dTimeGap > 11000)
            {
                Console.WriteLine("Client Disconnected! TimeGap : {0} milli-second", dTimeGap);
                lock (stCliSock.objLock)
                {
                    stCliSock.bConn = false;
                }
            }
            //Console.WriteLine("Session Flag : {0}", stCliSock.bConn == true ? "TRUE" : "FALSE");
            if (stCliSock.bConn == false)
            {
                lock (stCliSock.objLock)
                {
                    closeSocket(ref stCliSock);
                    //Console.WriteLine("Now wait client connection...");
                    stCliSock.objSock = stSrvSock.objSock.Accept();
                    stCliSock.lnResetTime = DateTime.UtcNow.Ticks;
                }
                stCliSock.bConn = true;
                Console.WriteLine("Socket re-accept SUCCESS!");
            }
            //stCliSock.lnResetTime = DateTime.UtcNow.Ticks;
            //return (int)ENUM_VALUE_SOCKETMGR.SUCCESS;
            return;
        }

        //public void closeSocket(ref Socket objSocket, ref object objLock)
        public void closeSocket(ref SOCK_INFO stSock)
        {
            //stSock.objSock != null ?  : Console.WriteLine("Try close null socket!");
            if (stSock.objSock != null)
            {
                lock (stSock.objSock)
                {
                    //stSock.isConn = false;
                    stSock.objSock.Close();
                }
            }

            return;
        }

        public byte[] recvByteMsg(ref SOCK_INFO stSock, ref int nMsgLength)
        {
            byte[] szRcvBuff = new byte[1024];
            //byte[] szRcvBuff;
            try
            {
                nMsgLength = stSock.objSock.Receive(szRcvBuff);
            }
            catch(Exception eCmn)
            {
                Thread.Sleep(500);
                Console.WriteLine("Exception!! Source : {0}, Message : {1}",
                   eCmn.Source, eCmn.Message);
            }
            return szRcvBuff;
        }

        public void sendByteMsg(ref SOCK_INFO stSock, byte[] szMsg, int nMsgLength)
        {
            try
            {
                stSock.objSock.Send(szMsg, 0, nMsgLength, SocketFlags.None);
            }
            catch(Exception eCmn)
            {
                Thread.Sleep(500);
                Console.WriteLine("{0} : {1}", MethodBase.GetCurrentMethod().Name, eCmn.Message);
            }
            return;
        }
        public void recvMsg(ref SOCK_INFO stSock)
        {
            __RECOG_RESULT stRecogResult = new __RECOG_RESULT();
            __STAT_INFO stStatInfo = new __STAT_INFO();

            RECOGRESULT objRecogResult = new RECOGRESULT();
            STATINFO objStatInfo = new STATINFO();

            int nRecvLength = 0;

            byte[] szRcvBuff = new byte[1024];
            string strBuff;

            if (stSock.bConn == false)
            {
                //return (int)ENUM_VALUE_SOCKETMGR.FAILED_NULL_SOCKET;
                Thread.Sleep(500);              // just for TEST
                //Console.WriteLine("Now disconnected to client!");
                return;
            }
            //stSock.lnResetTime = DateTime.UtcNow.Ticks; // just for TEST
            try
            {
                nRecvLength = stSock.objSock.Receive(szRcvBuff);
            }
            catch(Exception eCmn)
            {
                Thread.Sleep(500);
                Console.WriteLine("Exception!! Source : {0}, Message : {1}",
                   eCmn.Source, eCmn.Message);
                return;
            }
            if (nRecvLength <= 0)
            {
                Thread.Sleep(500);
                Console.WriteLine("Receive NO-DATA{0}", nRecvLength);
                //return (int)ENUM_VALUE_SOCKETMGR.RECV_NO_DATA;
                return;
            }
            else
            {
                strBuff = Encoding.ASCII.GetString(szRcvBuff, 0, nRecvLength);
                Console.WriteLine(strBuff);
                if (szRcvBuff[nRecvLength - 1] != 0x03)
                {
                    Console.WriteLine("Invalid ETX in receive message!");
                }
                else if (strBuff.Contains("LPRS|"))
                {
                    objRecogResult.getRecogResult(ref stRecogResult, szRcvBuff);
                }
                else if (strBuff.Contains("ST|"))
                {
                    // re-up coonnection check flag
                    lock (stSock.objLock)
                    {
                        stSock.lnResetTime = DateTime.UtcNow.Ticks;
                        stSock.bConn = true;
                    }

                    objStatInfo.getStatInfo(ref stStatInfo, szRcvBuff);
                }
                else
                {
                    Console.WriteLine("Invalid Instruction!");
                }
                //return (int)ENUM_VALUE_SOCKETMGR.SUCCESS;
                return;
                //nRecvLength = 0;
            }
        }
    }
}
