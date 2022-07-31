using System;

namespace Msmq.Wcf.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("type text:");
                var answer = Console.ReadLine();

                if (answer == "n")
                    break;

                var client = new WcfService.TestContractClient();
                client.Create(answer);
            }

            Console.WriteLine("All record sent successfully");
            Console.ReadLine();

        }
    }
}
