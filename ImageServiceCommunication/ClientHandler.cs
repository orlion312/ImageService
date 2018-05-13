using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ImageServiceCommunication.Events;
using System.Threading.Tasks;
using System.Threading;

namespace ImageServiceCommunication
{
    class ClientHandler : IClientChannel
    {
        private TcpClient m_client;
        private NetworkStream m_streamer;
        private StreamReader m_reader;
        private StreamWriter m_writer;
        private CancellationTokenSource m_cancelToken;

        public ClientHandler(TcpClient client)
        {
            m_client = client;
            m_streamer = client.GetStream();
            m_reader = new StreamReader(m_streamer, Encoding.ASCII);
            m_writer = new StreamWriter(m_streamer, Encoding.ASCII);
            m_cancelToken = new CancellationTokenSource();

        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public void close()
        {
            m_cancelToken.Cancel();
        }

        public void start()
        {
            string msg;
            new Task(() =>
            {
                try
                {
                    while (true)
                    {
                        msg = m_reader.ReadLine();
                        if (msg != null)
                        {
                            DataReceivedEventArgs d = new DataReceivedEventArgs();
                            d.Message = msg;
                            DataReceived?.Invoke(this, d);
                        }
                    }
                }
                catch (Exception e)
                {
                    DataReceivedEventArgs d = new DataReceivedEventArgs();
                    d.Message = e.ToString();
                    DataReceived?.Invoke(this, d);
                }
            }, m_cancelToken.Token).Start();
        }


        //public void HandleClient(TcpClient client)
        //{
        //    new Task(() =>
        //    {
        //        using (NetworkStream stream = client.GetStream())
        //        using (StreamReader reader = new StreamReader(stream))
        //        using (StreamWriter writer = new StreamWriter(stream))
        //        {
        //            string commandLine = reader.ReadLine();
        //            Console.WriteLine("Got command: {0}", commandLine);
        //            string result = ExecuteCommand(commandLine, client);
        //            writer.Write(result);
        //        }
        //        client.Close();
        //    }).Start();
        //}
    }
}
