using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MessageFormat.Sockets
{
    public abstract class ServerService
    {
        private IPEndPoint _endPoint;
        private Socket _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public ServerService(int port)
        {
            StartServer(port);
        }

        private void StartServer(int port)
        {
            IPAddress ipAddress = IPAddress.Any;
            _endPoint = new IPEndPoint(ipAddress, port);

            try
            {
                _server.Bind(_endPoint);
                _server.Listen(100);

                while (true)
                {
                    Console.WriteLine("Waiting for connection");
                    Socket handler = _server.Accept();

                    Console.WriteLine("Client Connected");

                    // start a new thread
                    Thread thread = new Thread(() => ReceiveMessage(handler));
                    thread.Start();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        private void ReceiveMessage(Socket handler)
        {
            while (true)
            {

                try
                {
                    // first 2 byte represent the total length of message
                    byte[] lengthByte = new byte[2];
                    int bytesRec = handler.Receive(lengthByte);

                    string length = Encoding.ASCII.GetString(lengthByte, 0, bytesRec);

                    // convert hex to integer value
                    int lengthOfMessage = int.Parse(length, System.Globalization.NumberStyles.HexNumber);


                    // now read rest of the data
                    byte[] restDataByte = new byte[lengthOfMessage];
                    bytesRec = handler.Receive(restDataByte);
                    string message = Encoding.ASCII.GetString(restDataByte, 0, bytesRec);

                    // call the abstruct method with the parsed message
                    OnMessageReceived(message);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    break;
                }
            }
        }


        protected abstract void OnMessageReceived(string message);

        public bool SendMessage(string message, Socket handler)
        {
            int lengthOfMessage = message.Length;


            string lengthToAppend = lengthOfMessage.ToString("X").PadLeft(2, '0');
            message = lengthToAppend + message;

            // convert the message to byte[]
            byte[] msg = Encoding.ASCII.GetBytes(message);
            try
            {
                handler.Send(msg);


                // response send succesfully close the connection ? 
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
