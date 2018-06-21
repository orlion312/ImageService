using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageCommunication.Client
{
    public class AndroidClient: IAndroidClient
    {
        #region Members
        private bool m_clientConnected;
        private bool stopped;
        private static AndroidClient instance;
        #endregion

        public event PictureHandle handlePicture;

        /// <summary>
        /// the constructor of the class
        /// </summary>
        private AndroidClient() { start(); }
        
        /// <summary>
        /// the method starts this instance- strats the class.
        /// </summary>
        private void start()
        {
            new Task(() =>
            {
                int port = 8500;
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                TcpListener listener = new TcpListener(ep);
                listener.Start();
                stopped = false;
                while (!stopped)
                {
                    try
                    {
                        // accepting clients
                        TcpClient client = listener.AcceptTcpClient();
                        m_clientConnected = true;
                        recieveClient(client);
                    }
                    catch (SocketException e)
                    {
                        Debug.WriteLine("Android client stopped: " + e.Message);
                    }
                }
            }).Start();

        }

        public static AndroidClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AndroidClient();
                }
                return instance;
            }
        }

        public bool ClientConnected { get { return m_clientConnected; } set { m_clientConnected = value; } }

        private const int BUFFER_SIZE = 1024;
        
        /// <summary>
        /// The method get a parama to recieves the client.
        /// </summary>
        /// <param name="client">The client to receive</param>
        private void recieveClient(TcpClient client)
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();

                while (!stopped)
                {
                    try
                    {
                        byte[] bytes = new byte[4096];

                        int bytesTransfered = stream.Read(bytes, 0, bytes.Length);
                        string picLen = Encoding.ASCII.GetString(bytes, 0, bytesTransfered);

                        if (picLen == "Stop Transfer\n")
                        {
                            //client.Close();
                            break;
                        }
                        bytes = new byte[int.Parse(picLen)];

                        bytesTransfered = stream.Read(bytes, 0, bytes.Length);
                        string pictureName = Encoding.ASCII.GetString(bytes, 0, bytesTransfered);

                        int bytesReadFirst = stream.Read(bytes, 0, bytes.Length);
                        int tempBytes = bytesReadFirst;
                        byte[] bytesCurrent;
                        while (tempBytes < bytes.Length)
                        {
                            bytesCurrent = new byte[int.Parse(picLen)];
                            bytesTransfered = stream.Read(bytesCurrent, 0, bytesCurrent.Length);
                            transferBytes(bytes, bytesCurrent, tempBytes);
                            tempBytes += bytesTransfered;
                        }

                        handlePicture?.Invoke(pictureName, bytes);

                        Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }).Start();
        }
        /// <summary>
        /// The mthod get 3 param and transfers the bytes.
        /// </summary>
        /// <param name="original">The original array bytes</param>
        /// <param name="copy">The copy array bytes</param>
        /// <param name="startPos">an Integer that represent the start position</param>
        private void transferBytes(byte[] original, byte[] copy, int startPos)
        {
            for (int i = startPos; i < original.Length; i++)
            {
                original[i] = copy[i - startPos];
            }
        }
    }
}
