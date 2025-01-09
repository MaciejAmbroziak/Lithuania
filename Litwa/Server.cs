using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Litwa
{
    internal class Server
    {

        public int Port { get; set; } = 8080;
        public string IPAddressString { get; set; } = "192.168.0.150";
        public TcpListener TCPServer { get; set; }
        public Server()
        {
            TCPServer = new TcpListener(IPAddress.Parse(IPAddressString), Port);
        }

        public Server(int port, string iPAddressString, TcpListener tCPServer)
        {
            Port = port;
            IPAddressString = iPAddressString;
            TCPServer = new TcpListener(IPAddress.Parse(IPAddressString), Port);
        }

        public void ServerUp(string localAddress, int port)
        {
            IPAddress localAddr = IPAddress.Parse(localAddress);
            TCPServer.Start();
        }
        public bool ServerDown()
        {
            TCPServer.Stop();
            TCPServer.Dispose();
            if (TCPServer == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public TcpClient Connect()
        {
            bool flag = true;
            TcpClient client = null;
            ServerUp(IPAddressString, Port);
            if (flag)
            {
                while (flag)
                {
                    Console.WriteLine("Waiting for connection");
                    client = TCPServer.AcceptTcpClient();
                    flag = false;
                }
            }
            Console.WriteLine("Connection successfull");
            return client;
        }

    }
}
