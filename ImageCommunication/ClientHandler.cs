using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ImageCommunication.Events;
using System.Threading.Tasks;
using System.Threading;

namespace ImageCommunication
{
    class ClientHandler : IClientHandler
    {
        private TcpClient m_client;
        private NetworkStream m_streamer;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;
        private static Mutex mut=new Mutex();

        public ClientHandler(TcpClient client)
        {
            if (client != null)
            {
                System.Diagnostics.Debug.WriteLine("Client Handler in");
                m_client = client;
                m_streamer = client.GetStream();
                m_reader = new BinaryReader(m_streamer, Encoding.ASCII);
                m_writer = new BinaryWriter(m_streamer, Encoding.ASCII);
            }
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public void Close()
        {
            if(m_client != null)
            {
                m_writer.Close();
                m_reader.Close();
                m_streamer.Close();
                m_client.Close();
                m_client = null;
            }
        }

        public void Start()
        {
            Task task = new Task(() =>
            {
                string msg;
                try
                {
                    while (m_client.Connected)
                    {
                        if ((msg = m_reader.ReadString()) != null)
                        {
                            DataReceivedEventArgs d = new DataReceivedEventArgs();
                            d.Message = msg;
                            DataReceived?.Invoke(this, d);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
            });
            task.Start();
        }

        public void Write(object sender, DataReceivedEventArgs data)
        {
            mut.WaitOne();
            try
            {
                m_writer.Write(data.Message.Trim());
                m_writer.Flush();
            } catch { }

            mut.ReleaseMutex();
        }

    }
}
