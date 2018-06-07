using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageCommunication;
using ImageCommunication.Events;

namespace ImageCommunication
{
     public class TcpServer: ITcpServer
    {
        private int port;
        private string ip;
        private List<IClientHandler> clientList;

        #region
        public TcpListener Listener { get; set; }
        #endregion


        public TcpServer()
        {
            this.ip = "127.0.0.1";
            this.port = 8001;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            Listener = new TcpListener(ep);
            this.clientList = new List<IClientHandler>();
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<DataReceivedEventArgs> DataSend;

        public void Start()
        {
            Listener.Start();
            Console.WriteLine("Waiting for connections...");

            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = Listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        IClientHandler handler = new ClientHandler(client);    
                        handler.DataReceived += this.DataReceived;
                        this.DataSend += handler.Write;
                        this.clientList.Add(handler);
                        handler.Start();

                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        public void Stop()
        {
            Listener.Stop();
        }

        public void Send(Object sender, DataReceivedEventArgs d)
        {
            this.DataSend?.Invoke(this, d);
        }

        public void NotifyAll(string path)
        {
            foreach (IClientHandler handler in clientList)
            {
                try
                {
                    DataReceivedEventArgs d = new DataReceivedEventArgs();
                    d.Message = path;
                    handler.Write(this, d);
                }
                catch (Exception e) { }
            }
        }
    }
}
