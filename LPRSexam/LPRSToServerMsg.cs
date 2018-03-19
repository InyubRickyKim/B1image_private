﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LPRSProtocol.Common;

namespace LPRSProtocol.LPRSToServerMsg
{
    public struct __RECOG_RESULT
    {
        public byte hxSTX;
        public byte[] szMNum;
        public byte[] szInstr;
        public byte[] szTimeIndx;
        public byte[] szCogResult;
        public byte[] szPhoto;
        public byte[] szSpeed;
        public byte[] szNumLocation;
        public byte[] szIndx;
        public byte[] szColor;
        public byte[] szCogPhoto;
        public byte[] szLane;
        public byte hxETX;
    };

    public struct __STAT_INFO
    {
        public byte hxSTX;
        public byte[] szMNum;
        public byte[] szInstr;
        public byte[] szDoorStat;
        public byte[] szLoop;
        public byte[] szMTemp;
        public byte[] szUnused;
        public byte[] szIsFanUse;
        public byte[] szIsHeatUse;
        public byte[] szIsLampUse;
        public byte[] szVer;
        public byte[] szLane;
        public byte[] szCurPT;
        public byte[] szIsActive;
        public byte hxETX;
    };

    public class RECOGRESULT
    {

        public void getRecogResult(ref __RECOG_RESULT stRecogResult, byte[] szMsg)
        {
            int nCurIndex = 0;
            int nIndexOfETX = 0;

            PROTOCOL_COMMON objProtCmn = new PROTOCOL_COMMON();

            nIndexOfETX = objProtCmn.getIndexOfETX(szMsg);
            if (nIndexOfETX <= 1)
                return;

            // Set STX, ETX
            stRecogResult.hxSTX = szMsg[nCurIndex];
            stRecogResult.hxETX = szMsg[nIndexOfETX];

            nCurIndex++;

            stRecogResult.szMNum = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szInstr = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szTimeIndx = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szCogResult = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szPhoto = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szSpeed = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szNumLocation = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szIndx = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szColor = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szCogPhoto = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stRecogResult.szLane = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);

            Console.WriteLine(stRecogResult.hxSTX);
            Console.WriteLine(BitConverter.ToString(stRecogResult.szMNum));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szInstr));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szTimeIndx));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szCogResult));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szPhoto));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szSpeed));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szNumLocation));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szIndx));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szColor));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szCogPhoto));
            Console.WriteLine(BitConverter.ToString(stRecogResult.szLane));
            Console.WriteLine(stRecogResult.hxETX);
            //BitConverter.ToChar(stRecogResult.hxSTX, 0);

            return;
        }

        /*public void getRecogResult(ref __RECOG_RESULT stDstMsg, ref byte[] szMsg)
        {
            int nCurIndx = 0;
            int nSepIndx = 0;
            int nStrLength = 0;

            string szBuf = Encoding.ASCII.GetString(szMsg);
            nStrLength = szBuf.Length;

            stDstMsg.hxSTX = szMsg[nCurIndx];
            nCurIndx += sizeof(byte);

            Console.WriteLine(szBuf + " : Length{0}", nStrLength);
            return;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szMNum = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szInstr = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szTimeIndx = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szCogResult = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szPhoto = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szSpeed = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szNumLocation = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szIndx = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szColor = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szCogPhoto = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szLane = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            stDstMsg.hxETX = szMsg[nCurIndx];
            nCurIndx += sizeof(byte);

            Console.WriteLine(Encoding.ASCII.GetString(stDstMsg.szLane));
            Console.WriteLine(stDstMsg.hxETX);
        }*/

        public void mskeRecogResult(ref byte[] szMsg)
        {
            int nDstIndx;
            int nSrcLen;

            byte[] szTestMsg = Encoding.ASCII.GetBytes("F0001|C|20170215-165857-000158|서울12가3456|T1-T2-T3|501|0-636-0-636|0001900511|1-0|RT=T1|LN=1|");

            nDstIndx = 0;

            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)0x02);
            nDstIndx += nSrcLen;

            nSrcLen = szTestMsg.Length;
            Buffer.BlockCopy(szTestMsg, 0, szMsg, nDstIndx, nSrcLen);
            nDstIndx += nSrcLen;

            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)0x03);
        }
    }

    public class STATINFO
    {
        public void makeStatInfo(ref byte[] szMsg)
        {
            int nDstIndx;
            int nSrcLen;

            byte[] szTestMsg = Encoding.ASCII.GetBytes("F0001|ST|DS=0|LO=1|TM=27|TP=0|FS=1|HS=0|LS=1|SW=v2.16-170205b|LN=1|PT=1-1-2-0-0|CS=1|");

            nDstIndx = 0;

            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)0x02);
            nDstIndx += nSrcLen;

            nSrcLen = szTestMsg.Length;
            Buffer.BlockCopy(szTestMsg, 0, szMsg, nDstIndx, nSrcLen);
            nDstIndx += nSrcLen;

            nSrcLen = sizeof(byte);
            Buffer.SetByte(szMsg, nDstIndx, (byte)0x03);

            return;
        }

        public void getStatInfo(ref __STAT_INFO stStatInfo, byte[] szMsg)
        {
            int nCurIndex = 0;
            int nIndexOfETX = 0;

            PROTOCOL_COMMON objProtCmn = new PROTOCOL_COMMON();

            nIndexOfETX = objProtCmn.getIndexOfETX(szMsg);
            if (nIndexOfETX <= 1)
                return;

            // Set STX, ETX
            stStatInfo.hxSTX = szMsg[nCurIndex];
            stStatInfo.hxETX = szMsg[nIndexOfETX];

            nCurIndex++;

            stStatInfo.szMNum = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szInstr = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szDoorStat = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szLoop = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szMTemp = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szUnused = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szIsFanUse = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szIsHeatUse = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szIsLampUse = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szVer = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szLane = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szCurPT = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);
            stStatInfo.szIsActive = objProtCmn.parseAsciiBytes(szMsg, ref nCurIndex, nIndexOfETX, 0x7C);

            return;
        }

        /*public void getStatInfo(ref __STAT_INFO stDstMsg, ref byte[] szMsg)
        {
            int nCurIndx = 0;
            int nSepIndx = 0;
            int nStrLength = 0;

            string szBuf = Encoding.ASCII.GetString(szMsg);

            nStrLength = szBuf.Length;

            Console.WriteLine(szBuf + " : Length{0}", nStrLength);
            return;

            stDstMsg.hxSTX = szMsg[nCurIndx];
            nCurIndx += sizeof(byte);

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szMNum = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;
            //Console.WriteLine(Encoding.Default.GetString(stDstMsg.szMNum));
            //Console.WriteLine("nSepIndx : {0}, nCurIndx : {1}", nSepIndx, nCurIndx);

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szInstr = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szDoorStat = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szLoop = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szMTemp = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szUnused = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szIsFanUse = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szIsHeatUse = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szIsLampUse = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szVer = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szLane = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            stDstMsg.szCurPT = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            nSepIndx = szBuf.IndexOf("|", nCurIndx);
            //stDstMsg.szIsActive = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            stDstMsg.szIsActive = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(szMsg, nCurIndx, (nSepIndx - nCurIndx)));
            nCurIndx += (nSepIndx - nCurIndx);
            nCurIndx++;

            stDstMsg.hxETX = szMsg[nCurIndx];
            nCurIndx += sizeof(byte);

            Console.WriteLine(stDstMsg.hxSTX);
            Console.WriteLine(Encoding.ASCII.GetString(stDstMsg.szCurPT));
            Console.WriteLine(Encoding.ASCII.GetString(stDstMsg.szIsActive));
            Console.WriteLine(stDstMsg.hxETX);
        }*/
    }
}
