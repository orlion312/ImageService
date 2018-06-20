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
        event PictureHandle handelPicture;
        void recieveCommand();
        bool ClientConnected { get; set; }
    }
}
