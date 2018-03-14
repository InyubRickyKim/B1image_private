using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace BerkSocketMgr
{
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
    }
}
