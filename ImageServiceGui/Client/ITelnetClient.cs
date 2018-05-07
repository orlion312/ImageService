using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Client
{

    interface ITelnetClient
    {
        void Connent(string IP, int port);
        void Write(string command);
        string Read();
        void Disconnect();
    }
        
}
