using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageFormat.Sockets.Client
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Sending message");
            Server ser = new Server();
            while(true)
            {
                string messageToSend = Console.ReadLine();
                if(ser.SendMessage(messageToSend))
                {
                    Console.WriteLine("Message Successfully send. Wanna send another one?");
                } else
                {
                    Console.WriteLine("Message Sending faild. Quiting...");
                    break;
                }
            }

            Console.ReadKey();
            
            
        }

        class Server : ClientService
        {
            public Server() : base ( "127.0.0.1", 11000)
            {

            }

            public override void OnMessageReceived(string message)
            {
                Console.WriteLine("Response received: ");
                Console.WriteLine(message);

            }

            private void Send(string message)
            {
                SendMessage(message);
            }
        }
    }
}
