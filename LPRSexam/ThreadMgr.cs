using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Common.BerkSocketMgr;
using LPRSProtocol.ServerToLPRSMsg;

namespace Common.ThreadMgr
{
    public struct ST_THRD_MGR
    {
        public object objLock;
        public object objQueue;
        public object stSock;
        public object objThrd;
    };
    class THREAD_MGR
    {
        public delegate void RecvDelegate(ref SOCK_INFO stCliSock);
        public void recvThread(ref SOCKET_MGR objSockMgr, ref SOCK_INFO stCliSock, bool bBackGround)
        {
            /*RecvDelegate dlgRecv = new RecvDelegate(objSockMgr.recvMsg);
            //Thread objRecvThrd = new Thread(new ParameterizedThreadStart(dlgRecv));
            Thread objRecvThrd = new Thread(() => dlgRecv(ref stCliSock));
            if (bBackGround == true)
                objRecvThrd.IsBackground = bBackGround;
            objRecvThrd.Start(ref stCliSock);*/
        }

        public void runSendThrd(ST_THRD_MGR stThrd, ref object objThrd)
        {
            //sendMsgDelegate dlgSendMsg = new sendMsgDelegate(objSockMgr.sendByteMsg);
            Thread objSndThrd = new Thread(new ParameterizedThreadStart(doSendThrd));
            //objThrd = new Thread(delegate() { doSendThrd(ref stThrd); });
            objSndThrd.IsBackground = true;
            objSndThrd.Start(stThrd);

            objThrd = objSndThrd;

            return;
        }

        static void doSendThrd(object stThrd)
        {
            ST_THRD_MGR stSndThrd = (ST_THRD_MGR)stThrd;
            ConcurrentQueue<string> objQue = (ConcurrentQueue<string>)stSndThrd.objQueue;

            SOCKET_MGR objSock = new SOCKET_MGR();
            RESMSG objResMsg = new RESMSG();

            SOCK_INFO stCliSock = (SOCK_INFO)stSndThrd.stSock;
            __ST_RES_MSG stResMsg = new __ST_RES_MSG();

            Thread objCurThrd = (Thread)stSndThrd.objThrd;

            //byte[] szDeQue = new byte[1024];
            string szDeQue;
            byte[] szMsg = new byte[1024];
            int nMsgLength = 1024;

            while(true)
            {
                szDeQue = "";
                if(objQue.TryDequeue(out szDeQue))
                {
                    if (string.Equals(szDeQue, "LPRS"))
                    {
                        Array.Clear(szMsg, 0, nMsgLength);
                        nMsgLength = 0;
                        //set response message
                        objResMsg.setResMsg(ref stResMsg, "OK|");
                        objResMsg.makeResMsg(ref stResMsg, ref szMsg);
                        nMsgLength = Array.IndexOf(szMsg, stResMsg.hxETX);
                        //stResMsg
                    }
                    else
                        continue;

                    // send message to LPRS client
                    if(stCliSock.bConn == true)
                    {
                        objSock.sendByteMsg(ref stCliSock, szMsg, nMsgLength);
                        Console.WriteLine("Send message to LPRS : {0}", Encoding.ASCII.GetString(szMsg));
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    //objCurThrd.Yield();
                    continue;
                }
            }
        }
    }
}
