using ImageCommunication.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageCommunication
{
    public interface ITcpServer
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        event EventHandler<DataReceivedEventArgs> DataSend;

        TcpListener Listener { get; set; }
        /// <summary>
        /// Starts the server.
        /// </summary>
        void Start();
        /// <summary>
        /// Stops the server.
        /// </summary>
        void Stop();
        /// <summary>
        /// Sends data to the events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="d">The data we send.</param>
        void Send(Object sender, DataReceivedEventArgs d);
        /// <summary>
        /// Notify all the clients.
        /// </summary>
        /// <param name="path"></param>
        void NotifyAll(string path);
    }
}
