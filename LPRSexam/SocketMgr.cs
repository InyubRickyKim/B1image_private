using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Common.BerkSocketMgr
{
    public struct SOCK_INFO
    {
        public Socket objSock;
        public object objLock;
    }
    class SOCKET_MGR
    {
        enum ENUM_VALUE_SOCKETMGR : sbyte {
            SUCCESS = 0,
            FAILED_IMPLEMENT_SOCKET,
            FAILED_INIT_ENDPOINT,
            FAILED_BIND
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

            // init lock object

            int nBackLog = 5;
            int lnPortNum = 7010;
            string strLocalIpAddr = "192.168.0.191";

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
            /*while (!Console.KeyAvailable)
            {
                nRcvLength = objCliSock.Receive(szRcvBuff);
                if (nRcvLength <= 0)
                {
                    Thread.Sleep(500);
                    nRcvLength = 0;
                    continue;
                }
                else
                {
                    strBuff = Encoding.ASCII.GetString(szRcvBuff, 0, nRcvLength);
                    //Console.WriteLine(strBuff);
                    if (szRcvBuff[nRcvLength - 1] != 0x03)
                    {
                        Console.WriteLine("Invalid ETX in receive message!");
                    }
                    else if (strBuff.Contains("LPRS|"))
                    {
                        objRecoRslt.getRecogResult(ref stRecogMsg, ref szRcvBuff);
                    }
                    else if (strBuff.Contains("ST|"))
                    {
                        objStatInfo.getStatInfo(ref stStatMsg, ref szRcvBuff);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Instruction!");
                    }
                    nRcvLength = 0;
                }
            }*/

            //objCliSock.Close();
            //objSrvSock.Close();
            Console.WriteLine("Socket accept SUCCESS!");
            return;
        }

        //public void closeSocket(ref Socket objSocket, ref object objLock)
        public void closeSocket(ref SOCK_INFO stSock)
        {
            //stSock.objSock != null ?  : Console.WriteLine("Try close null socket!");
            if(stSock.objSock != null)
                lock (stSock.objSock)
                    stSock.objSock.Close();

            return;
        }
    }
}
