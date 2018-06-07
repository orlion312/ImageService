using ImageCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    class MainWindowModel: IMainWindowModel
    {
        private ITcpClient m_client;
        public MainWindowModel()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("settings model");
                m_client = TcpClientChannel.ClientInstance;
            } catch (Exception e)
            {
                m_client = null;
            }
        }

        public string Color()
        {
            if (m_client != null && m_client.Connect())
            {
                return "White";
            }
            return "LightGray";
        }
    }
}
