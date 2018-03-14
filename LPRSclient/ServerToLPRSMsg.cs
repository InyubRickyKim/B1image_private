using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LPRSProtocol.ServerToLPRSMsg
{
    public struct __ST_TIME_SYNC
    {
        public byte        hxSTX;
        public byte[]      szInstruction;
        public byte[]      szTime;
        public byte        hxETX;
    };

    public struct __ST_RES_MSG
    {
        public byte        hxSTX;
        public byte[]      strResMsg;
        public byte        hxETX;
    };

    public class RESMSG
    {
        public void setResMsg(ref __ST_RES_MSG stResMsg, string strResMsg)
        {
            stResMsg.hxSTX = 0x02;
            stResMsg.strResMsg = Encoding.ASCII.GetBytes(strResMsg);
            //stResMsg.strResMsg = BitConverter.GetBytes(strResMsg);
            //Buffer.SetByte(stResMsg.strResMsg, strResMsg.Length, (char)"|");
            stResMsg.hxETX = 0x03;
        }

        public void makeResMsg(ref __ST_RES_MSG stResMsg, ref byte[] szMsg)
        {
            int nDstIndx;
            int nSrcLen;

            //byte[] szBuff = new byte[512];

            nDstIndx = 0;
            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)stResMsg.hxSTX);
            nDstIndx = nSrcLen;

            nSrcLen = stResMsg.strResMsg.Length;
            Buffer.BlockCopy(stResMsg.strResMsg, 0, szMsg, nDstIndx, nSrcLen);
            nDstIndx += nSrcLen;

            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)stResMsg.hxETX);

            //if(BitConverter.IsLittleEndian)

            //Array.Reverse(szMsg);

            //szMsg = IPAddress.HostToNetworkOrder(szMsg);

            //szMsg = BitConverter.GetBytes(szMsg);

            //
            //szBuff = BitConverter.GetBytes(szMsg);

            //Console.WriteLine("{0}", Encoding.ASCII.GetString(szMsg));
            //Buffer.BlockCopy(stTimeSync.hxSTX, 0, )
        }
    }

    public class TIMESYNC
    {

        public void setTimeSync(ref __ST_TIME_SYNC stTimeSync, string strInstruction, string strTime)
        {
            stTimeSync.hxSTX = 0x02;
            stTimeSync.szInstruction = Encoding.ASCII.GetBytes(strInstruction);
            stTimeSync.szTime = Encoding.ASCII.GetBytes(strTime);
            stTimeSync.hxSTX = 0x03;
        }

        public void makeTimeSyncMsg(ref __ST_TIME_SYNC stTimeSync, ref byte[] szMsg)
        {
            int nDstIndx;
            int nSrcLen;

            // need before progress Byte-Odering
            nDstIndx = 0;
            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)stTimeSync.hxSTX);
            nDstIndx = nSrcLen;

            nSrcLen = stTimeSync.szInstruction.Length;
            Buffer.BlockCopy(stTimeSync.szInstruction, 0, szMsg, nDstIndx, nSrcLen);
            nDstIndx += nSrcLen;

            nSrcLen = stTimeSync.szTime.Length;
            Buffer.BlockCopy(stTimeSync.szTime, 0, szMsg, nDstIndx, nSrcLen);
            nDstIndx += nSrcLen;

            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)stTimeSync.hxETX);

            //Console.WriteLine("{0}", Encoding.ASCII.GetString(szMsg));
            //Buffer.BlockCopy(stTimeSync.hxSTX, 0, )
        }

    }
}
