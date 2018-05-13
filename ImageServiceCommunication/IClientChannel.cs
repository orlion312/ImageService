using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Events;

namespace ImageServiceCommunication
{
    interface IClientChannel
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        void start();
        void close();
    }
}
