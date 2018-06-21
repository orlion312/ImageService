using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCommunication.Client
{
    public delegate void PictureHandle(string name, byte[] byteArray);

    interface IAndroidClient
    {
        event PictureHandle handlePicture;
        bool ClientConnected { get; set; }
    }
}
