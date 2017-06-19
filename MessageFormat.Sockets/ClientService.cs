using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MessageFormat.Sockets
{
    public abstract class ClientService
    {

        private readonly IPEndPoint _endPoint;
        private Socket _client;

        /// <summary>
        /// Constructor for the ClientService abstract class that will try to connect to the TCP server with provided IPAddress and
        /// port
        /// </summary>
        /// <param name="ipAddress">IP Address of the TCP Server</param>
        /// <param name="port">Listening port of the TCP server to connect</param>
        public ClientService(string ipAddress, int port)
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            StartClient();
        }


        /// <summary>
        /// This method will be called when the socket client received any message
        /// </summary>
        /// <param name="message">Data received by the socket client</param>
        public virtual void OnMessageReceived(string message)
        {
            // need to implement by derived class
        }

        public bool SendMessage(string message)
        {
            if(!_client.Connected)
            {
                Console.WriteLine("Socket got disconnected");
                return false;
            }
            // TODO: Send message to ther server if the connection is establised
            // find the length of the message
            int lengthOfMessage = message.Length;


            string lengthToAppend = lengthOfMessage.ToString("X").PadLeft(2, '0');
            message = lengthToAppend + message;


            // convert the message to byte[]
            byte[] msg = Encoding.ASCII.GetBytes(message);

            // Now send the message using socket connection
            try
            {
                _client.Send(msg);
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }

        private void StartClient()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _client.Connect(_endPoint);

        }
    }
}
