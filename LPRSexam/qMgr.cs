using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.QMgr
{
    class Q_MGR
    {
        public void initByteConcurrentQueue(ref object objQ)
        {
            objQ = new ConcurrentQueue<byte[]>();

            return;
        }

        /*public bool tryStringDeque(ConcurrentQueue<string> objQ, ref object strOut)
        {
            bool objQ.TryDequeue(out strOut);
        }*/
    }
}
