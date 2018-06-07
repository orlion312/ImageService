using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageCommunication.Events;

namespace ImageCommunication
{
    public class TcpClientChannel : ITcpClient
    {
        private int port;
        private string ip;
        private TcpClient client;
        private static TcpClientChannel instance;
        private IPEndPoint endPoint;
        private NetworkStream m_streamer;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;
        /// <summary>
        /// Singleton of the client.
        /// </summary>
        public static TcpClientChannel ClientInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TcpClientChannel();
                }
                return instance;
            }
        }
        /// <summary>
        /// C'tor.
        /// </summary>
        private TcpClientChannel()
        {
            this.ip = "127.0.0.1";
            this.port = 8001;
            this.client = new TcpClient();
            this.endPoint = new IPEndPoint(IPAddress.Parse(ip),port);
            try
            {
                this.client.Connect(this.endPoint);
                this.m_streamer = client.GetStream();
                this.m_reader = new BinaryReader(m_streamer, Encoding.ASCII);
                this.m_writer = new BinaryWriter(m_streamer, Encoding.ASCII);
                Console.Write("You connected successfully");
                Read();
            }
            catch (Exception e)
            {
                Console.Write("can't connect");
                Close();
            }
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public void Close()
        {
            if(client != null )
            {
                this.m_reader.Close();
                this.m_writer.Close();
                this.m_streamer.Close();
                this.client.Close();
                client = null;
            }
        }

        public void Read()
        {
            new Task(() => {
                 while(client.Connected)
                {
                    try
                    {
                        string msg;
                        if ((msg = m_reader.ReadString()) != null)
                        {
                            DataReceivedEventArgs d = new DataReceivedEventArgs();
                            d.Message = msg;
                            DataReceived?.Invoke(this, d);
                        }
                    } catch (Exception e) { }

                }   
            }).Start();
        }
        public void Send(string data)
        {
            try
            {
                m_writer.Write(data.Trim());
                m_writer.Flush();
            } catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
        public bool Connect()
        {
            if(client == null)
            {
                return false;
            }
            return client.Connected;
        }
    }
}
 