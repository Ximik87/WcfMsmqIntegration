using System;
using System.Messaging;
using System.ServiceModel;
using Contracts;

namespace Msmq.Wcf.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var host = new ServiceHost(typeof(TestService)))
            {
                string queueName = ".\\private$\\wcfQueue";
                if(!MessageQueue.Exists(queueName))
                {
                    MessageQueue.Create(queueName);
                }

                host.Open();
                Console.WriteLine("Server is up and running on port 32578");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
