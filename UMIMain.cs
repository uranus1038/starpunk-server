using System;
using System.Threading;

namespace UMI.Network.Server
{
    class UmiRuntimeService
    {
        private static bool isRunnig = false;
        public const int TICK_PER_SEC = 80;
        public const int MS_PER_TICK = 1000 / TICK_PER_SEC;
        static void Main(string[] args)
        {
            Console.Title = "StarpunkGameServer";
            isRunnig = true;
            Thread mainThread = new Thread(new ThreadStart(_mainThread));
            mainThread.Start();
            UMIServerListener.Start(4, 8882);
        }
        private static void _mainThread()
        {
            DateTime _next_Loop = DateTime.Now;
            while (isRunnig)
            {
                while (_next_Loop < DateTime.Now)
                {
                    UMIGameLogic.UMIUpdate();
                    UMIThreadManager.UMIMain();
                    _next_Loop = _next_Loop.AddMilliseconds(MS_PER_TICK);
                }
                if (_next_Loop > DateTime.Now)
                {
                    Thread.Sleep(_next_Loop - DateTime.Now);
                }
            }
        }
    }
}