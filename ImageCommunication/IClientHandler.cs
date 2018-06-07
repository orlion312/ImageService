using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageCommunication.Events;

namespace ImageCommunication
{
    public interface IClientHandler
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        /// <summary>
        /// Starts the client.
        /// </summary>
        void Start();
        /// <summary>
        /// Closes the client.
        /// </summary>
        void Close();
        /// <summary>
        /// Writes the data we want.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        void Write(object sender, DataReceivedEventArgs data);
    }
}
