using System;
using System.Threading;

namespace RotMG.Utils
{
    public static class ThreadUtils
    {
        public static void StartNewThread(ThreadPriority priority, Action action)
        {
            var thread = new Thread(() => { action(); });
            thread.Priority = priority;
            thread.Start();
        }

        public static void StopCurrentThread()
        {
            var thread = Thread.CurrentThread;
            thread.Join();
        }
    }
}
