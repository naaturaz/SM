using System;
using System.Collections.Generic;
using System.Threading;

internal class ThreadPool
{
    private List<Thread> _pool = new List<Thread>();

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