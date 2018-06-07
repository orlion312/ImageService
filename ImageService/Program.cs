using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //ImageService1 image = new ImageService1();
            //image.onDebug();
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                new ImageService1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
