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
            FAIL,
            INVALID_MSG,
            NO_STX_MSG,
            NO_ETX_MSG
        };

        public int getIndexOfETX(byte[] szMsg)
        {
            return Array.IndexOf(szMsg, hxETX, 0);
        }

        public byte[] parseAsciiBytes(byte[] szMsg, ref int nCurIndx, int nStrLength, byte cPattern)
        {
            int nSepIndx = 0;
            if (nCurIndx >= nStrLength)
                return null;
            else
            {
                nSepIndx = Array.IndexOf(szMsg, cPattern, nCurIndx);
                byte[] szReturn = new byte[nSepIndx - nCurIndx];
                int i = 0;
                for(; nCurIndx < nSepIndx; nCurIndx++)
                {
                    szReturn[i] = szMsg[nCurIndx];
                    i++;
                }
                nCurIndx++;
                return szReturn;
            }
        }

        public int chkMsg(byte[] szMsg, int nMsgLength)
        {
            int nRetVal = (int)E_PROTOCOL_RET.FAIL;
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
