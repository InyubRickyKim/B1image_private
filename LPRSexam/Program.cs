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
using BerkSocketMgr;

namespace LPRSexam
{
    class Program
    {
        static void Main(string[] args)
        {
            wrkSocketMgr();
        }
        public static void wrkSocketMgr()
        {
            Socket objSrvSock = null;
            Socket objCliSock = null;
            IPEndPoint objEp = null;

            __RECOG_RESULT stRecogMsg = new __RECOG_RESULT();
            __STAT_INFO stStatMsg = new __STAT_INFO();

            RECOGRESULT objRecoRslt = new RECOGRESULT();
            STATINFO objStatInfo = new STATINFO();
            SOCKET_MGR objSocketMgr = new SOCKET_MGR();

            ArrayList lstListen = new ArrayList();
            //lstListen.Add(objSrvSock);
            ArrayList lstAccept = new ArrayList();
            //lstAccept.Add(objCliSock);

            int nBackLog = 5;
            int nRcvLength = 0;
            int lnPortNum = 7010;
            string strLocalIpAddr = "192.168.0.191";

            int nResult = 0;

            //gnPortNum = 7010;

            byte[] szRcvBuff = new byte[1024];

            string strBuff;

            //objSrvSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if ((nResult = objSocketMgr.impBerkSocket(ref objSrvSock,
                (int)AddressFamily.InterNetwork, (int)SocketType.Stream, (int)ProtocolType.Tcp)) != 0)
                return;

            if ((nResult = objSocketMgr.initEndPint(ref objEp, strLocalIpAddr, lnPortNum)) != 0)
                return;
            //objEp = new IPEndPoint(IPAddress.Any, lnPortNum);
            
            //objSrvSock.Bind(objEp);
            if ((nResult = objSocketMgr.bindLocalEp(ref objSrvSock, ref objEp)) != 0)
                return;

            objSrvSock.Listen(nBackLog);
            
            lstListen.Add(objSrvSock);
            //lstAccept.Add(objCliSock);
            //Socket.Select(lstListen, null, null, 1000);
            //Console.WriteLine("Now Listen socket!");
            //objCliSock = objSrvSock.Accept();

            //lstAccept.Add(objCliSock);

            Socket.Select(lstListen, null, null, 50000);
            objCliSock = objSrvSock.Accept();
            while (!Console.KeyAvailable)
            {
                try
                {
                    /*if (lstListen.Count == 0)
                    {
                        Console.WriteLine("lstListen.Count = {0}", lstListen.Count);
                        //lstListen.Add(objSrvSock);
                        break;
                    }*/
                    //Socket.Select(lstListen, null, null, 50000);
                   //lstAccept[0] = ((Socket)lstListen[0]).Accept();
                    //objCliSock = ((Socket)lstListen[0]).Accept();
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
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            objCliSock.Close();
            objSrvSock.Close();
        }
    }
}
