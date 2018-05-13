using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Events;

namespace ImageServiceCommunication
{
    class TcpClientChannel : IClientCommunication
    {
        private int port;
        private string ip;
        private TcpClient client;
        private IClientCommunication handler;

        public TcpClientChannel(string ip, int port)
        {
            this.port = port;
            this.ip = ip;
            this.handler = null;
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public void close()
        {
            throw new NotImplementedException();
        }

        public void HandleClient(TcpClient client)
        {
            throw new NotImplementedException();
        }

        public int send(string data)
        {
            throw new NotImplementedException();
        }

        public void start()
        {
            throw new NotImplementedException();
        }
    }
}
