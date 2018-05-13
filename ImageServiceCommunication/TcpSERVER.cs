﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
     class TcpServer
    {
        private int port;
        private TcpListener listener;
        private IClientCommunicationChannel ch;

        public TcpServer(int port, IClientCommunicationChannel ch)
        {
            this.port = port;
            this.ch = ch;
        }

        public void Start()
        {
            IPEndPoint ep = new
            IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            listener.Start();
            Console.WriteLine("Waiting for connections...");

            Task task = new Task(() => {
                IClientChannel handler;
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        handler = new ClientHandler(client);
                        handler.start();


                        Console.WriteLine("Got new connection");
                        ch.HandleClient(client);
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
            listener.Stop();
        }
    }
}
}
