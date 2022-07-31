using System;
using System.Messaging;

namespace MessageReceiverNetClassic
{
    internal class MsmqMessageReceiver
    {
        public static void DoWork()
        {  
            Console.WriteLine("Start receiver MSMQ");
            MessageQueue queue = new MessageQueue(".\\Private$\\wcfQueue");

            var enumerator = queue.GetMessageEnumerator2();
            while (true)
            {

                if (enumerator.MoveNext())
                {
                    var message = queue.Receive();
                    var wcfMessage = MsmqDecodeHelper.DecodeTransportDatagram(message.BodyStream);
                    Console.WriteLine(wcfMessage);
                }

                System.Threading.Thread.Sleep(100);
            }
        }
    }
}