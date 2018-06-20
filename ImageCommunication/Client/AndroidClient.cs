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

        private AndroidClient() { start(); }
        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void start()
        {
            new Task(() =>
            {
                int port = 8001;
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

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
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
        /// Recieves the client.
        /// </summary>
        /// <param name="client">The client.</param>
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

                        // get the size of the picture
                        int bytesTransfered = stream.Read(bytes, 0, bytes.Length);
                        string picLen = Encoding.ASCII.GetString(bytes, 0, bytesTransfered);

                        if (picLen == "Stop Transfer\n")
                        {
                            //client.Close();
                            break;
                        }
                        bytes = new byte[int.Parse(picLen)];

                        //get the picture after knowing the size

                        bytesTransfered = stream.Read(bytes, 0, bytes.Length);
                        string pictureName = Encoding.ASCII.GetString(bytes, 0, bytesTransfered);

                        //gets the image.
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

                        Thread.Sleep(100); // Update information every 0.1 seconds
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }).Start();
        }
        /// <summary>
        /// Transfers the bytes.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="copy">The copy.</param>
        /// <param name="startPos">The start position.</param>
        private void transferBytes(byte[] original, byte[] copy, int startPos)
        {
            for (int i = startPos; i < original.Length; i++)
            {
                original[i] = copy[i - startPos];
            }
        }

        public void recieveCommand()
        {
            // TODO WHATTTTTT
            throw new NotImplementedException();
        }
    }
}
