using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Client
{
    class Client : ITelnetClient
    {
        private TcpClient client;

        public Client()
        {
            client = new TcpClient();
        }

        public void Connent(string IP, int port)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public string Read()
        {
            throw new NotImplementedException();
        }

        public void Write(string command)
        {
            throw new NotImplementedException();
        }
    }
}
