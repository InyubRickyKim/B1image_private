using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPRSProtocol.Common
{
    public class PROTOCOL_COMMON
    {
        const byte hxSTX = 0x02;
        const byte hxETX = 0x03;
        public enum E_PROTOCOL_RET : int
        {
            SUCCESS = 0,
            INVALID_MSG,
            NO_STX_MSG,
            NO_ETX_MSG
        };

        public byte[] parseAsciiBytes(byte[] szMsg, string szBuf, ref int nCurIndx, ref int nSepIndx, int nStrLength)
        {
            byte[] szReturn;
            if (nCurIndx > nStrLength)
                return null;
            else
            {
                nSepIndx = szBuf.IndexOf("|", nCurIndx);
                szReturn = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
                nCurIndx += (nSepIndx - nCurIndx);
                nCurIndx++;
                return szReturn;
            }
        }

        public int chkMsg(byte[] szMsg, int nMsgLength)
        {
            int nRetVal = (int)E_PROTOCOL_RET.SUCCESS;
            if (szMsg[0] != hxSTX)
            {
                if (szMsg[nMsgLength - 1] != hxETX)
                    nRetVal = (int)E_PROTOCOL_RET.INVALID_MSG;
                else
                    nRetVal = (int)E_PROTOCOL_RET.NO_STX_MSG;
            }
            else
            {
                if (szMsg[nMsgLength - 1] != hxETX)
                    nRetVal = (int)E_PROTOCOL_RET.NO_ETX_MSG;
                else
                    nRetVal = (int)E_PROTOCOL_RET.SUCCESS;
            }
            return nRetVal;
        }
    }
}
