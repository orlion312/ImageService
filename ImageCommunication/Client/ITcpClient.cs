using ImageCommunication.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCommunication
{
    public interface ITcpClient
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        /// <summary>
        /// Close the client.
        /// </summary>
        void Close();
        /// <summary>
        /// Reads the messages from the service.
        /// </summary>
        void Read();
        /// <summary>
        /// Sends messages.
        /// </summary>
        /// <param name="data">The message we want to send.</param>
        void Send(string data);
        /// <summary>
        /// Checks if the client connected.
        /// </summary>
        /// <returns>True if connected and false if not.</returns>
        bool Connect();
    }
}
