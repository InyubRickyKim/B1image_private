using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Common.BerkSocketMgr;

namespace Common.ThreadMgr
{
    class ThreadMgr
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

        void sendThread()
        {

        }
    }
}
