using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Msmq.Wcf.ConsoleClientLoad.WcfService;

namespace Msmq.Wcf.ConsoleClientLoad
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            Console.WriteLine("Ready ???");
            Console.ReadLine();

            var tasks = new List<Task>();
            var counter = new RpsCounter();
            for (int i = 0; i < 8; i++) 
            {
                tasks.Add(Task.Factory.StartNew(() => MultiThread(counter), TaskCreationOptions.LongRunning));
            }

            Task.WaitAll(tasks.ToArray());
            return Task.CompletedTask;
        }

        private static void MultiThread(RpsCounter count)
        {
            var client = new TestContractClient();
            while (true)
            {
                count.Reset();

                var guid = Guid.NewGuid();
                client.Create(guid.ToString());
                count.Inc();
                Thread.Sleep(1);
            }
        }
    }

    internal class RpsCounter
    {
        private readonly object _sync = new object();
        private readonly Stopwatch _watch = Stopwatch.StartNew();
        private int _count;

        public void Inc()
        {
            Interlocked.Increment(ref _count);
        }

        public void Reset()
        {
            if (_watch.ElapsedMilliseconds <= 1000)
                return;

            Console.WriteLine("Send message RPS:" + _count);
            lock (_sync)
            {
                _watch.Restart();
                _count = 0;
            }
        }
    }
}