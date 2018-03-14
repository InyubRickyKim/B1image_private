using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;

using LPRSProtocol.LPRSToServerMsg;

namespace Common.BerkSocketMgr
{
    public struct SOCK_INFO
    {
        public Socket objSock;
        public object objLock;
        public bool isConn;
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

            stSrvSock.isConn = false;
            stCliSock.isConn = false;

            if ((stSrvSock.objSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) == null)
                return;

            if ((objEp = new IPEndPoint(IPAddress.Parse(strLocalIpAddr), lnPortNum)) == null)
                return;

            stSrvSock.objSock.Bind(objEp);

            stSrvSock.objSock.Listen(nBackLog);

            //lstListen.Add(objSrvSock);

            //Socket.Select(lstListen, null, null, 50000);
            Console.WriteLine("Now waiting CLIENT connect!");
            stCliSock.objSock = stSrvSock.objSock.Accept();

            // up to connection flag
            stCliSock.isConn = true;

            Console.WriteLine("Socket accept SUCCESS!");
            return;
        }

        public void reconnSocket(ref SOCK_INFO stSrvSock, ref SOCK_INFO stCliSock)
        {
            if(stCliSock.isConn == false)
            {
                closeSocket(ref stCliSock);
                lock (stCliSock.objLock)
                {
                    stCliSock.objSock = stSrvSock.objSock.Accept();
                }
                Console.WriteLine("Socket re-accept SUCCESS!");
            }
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

        public int recvMsg(ref SOCK_INFO stSock)
        {
            __RECOG_RESULT stRecogResult = new __RECOG_RESULT();
            __STAT_INFO stStatInfo = new __STAT_INFO();

            RECOGRESULT objRecogResult = new RECOGRESULT();
            STATINFO objStatInfo = new STATINFO();

            int nRecvLength = 0;

            byte[] szRcvBuff = new byte[1024];
            string strBuff;

            if (stSock.objSock == null)
                return (int)ENUM_VALUE_SOCKETMGR.FAILED_NULL_SOCKET;

            nRecvLength = stSock.objSock.Receive(szRcvBuff);
            if (nRecvLength <= 0)
            {
                Thread.Sleep(500);
                return (int)ENUM_VALUE_SOCKETMGR.RECV_NO_DATA;
            }
            else
            {
                strBuff = Encoding.ASCII.GetString(szRcvBuff, 0, nRecvLength);
                //Console.WriteLine(strBuff);
                if (szRcvBuff[nRecvLength - 1] != 0x03)
                {
                    Console.WriteLine("Invalid ETX in receive message!");
                }
                else if (strBuff.Contains("LPRS|"))
                {
                    objRecogResult.getRecogResult(ref stRecogResult, ref szRcvBuff);
                }
                else if (strBuff.Contains("ST|"))
                {
                    // re-up coonnection check flag
                    lock (stSock.objLock)
                    {
                        stSock.isConn = true;
                    }

                    objStatInfo.getStatInfo(ref stStatInfo, ref szRcvBuff);
                }
                else
                {
                    Console.WriteLine("Invalid Instruction!");
                }
                return (int)ENUM_VALUE_SOCKETMGR.SUCCESS;
                //nRecvLength = 0;
            }
        }
    }
}
