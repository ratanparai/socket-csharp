using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageFormat.Sockets.Server
{
    class Program 
    {

        static void Main(string[] args)
        {
            Server ser = new Server();
            Console.WriteLine("You shouldn't see this message"); 
            Console.ReadKey();
        }

    }

    class Server : ServerService
    {
        public Server() : base(11000)
        {

        }

        protected override void OnMessageReceived(string message)
        {
            Console.WriteLine(message);
        }
    }
}
