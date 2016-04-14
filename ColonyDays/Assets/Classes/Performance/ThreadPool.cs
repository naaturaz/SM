using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


class ThreadPool
{
    List<Thread> _pool = new List<Thread>();





    public static void RunThis(Person p)
    {
        //var th = new Thread(p.Body.WalkHanderCheck);
        //th.Start();
    }

    private static Thread GiveMeUnusedThread()
    {
        throw new NotImplementedException();
    }
}

