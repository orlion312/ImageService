using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Controller;
using ImageService.Server;
using ImageService.Modal;
using ImageService.Logging;
using System.Configuration;
using ImageService.Controller.Handlers;
using ImageService.Logging.Modal;

public enum ServiceState
{
    SERVICE_STOPPED = 0x00000001,
    SERVICE_START_PENDING = 0x00000002,
    SERVICE_STOP_PENDING = 0x00000003,
    SERVICE_RUNNING = 0x00000004,
    SERVICE_CONTINUE_PENDING = 0x00000005,
    SERVICE_PAUSE_PENDING = 0x00000006,
    SERVICE_PAUSED = 0x00000007,
}

[StructLayout(LayoutKind.Sequential)]
public struct ServiceStatus
{
    public int dwServiceType;
    public ServiceState dwCurrentState;
    public int dwControlsAccepted;
    public int dwWin32ExitCode;
    public int dwServiceSpecificExitCode;
    public int dwCheckPoint;
    public int dwWaitHint;
};

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        private EventLog eventLog1;
        private ILoggingService logger;
        private int thumbnailSize;
        private int eventId = 1;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        private ImageServer m_imageServer;
        private readonly string outputDir;
        private readonly string handledDir;


        public ImageService(string[] args) {
            InitializeComponent();
            string eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            outputDir = ConfigurationManager.AppSettings["OutputDir"];
            handledDir = ConfigurationManager.AppSettings["HandledDir"];
            if (!int.TryParse(ConfigurationManager.AppSettings["ThumbnailSize"], out thumbnailSize))
            {
                // Sets default thumbnail size
                thumbnailSize = 12;
            }
            
            eventLog1 = new System.Diagnostics.EventLog();
            try
            {
                if (!EventLog.SourceExists(eventSourceName))
                {
                    EventLog.CreateEventSource(eventSourceName, logName);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        protected override void OnStart(string[] args) {
            IImageServiceModal imageServiceModal = new ImageServiceModal(outputDir, thumbnailSize);
            ImageController imageController = new ImageController(imageServiceModal);
            logger = new LoggingService();
            logger.MessageRecieved += onMessage;

            IDirectoryHandler handler = new DirectoyHandler(logger, imageController);
            m_imageServer = new ImageServer(imageController, logger);

            logger.Log("On start", Logging.Modal.MessageTypeEnum.INFO);
           

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            //OnPending(serviceStatus);

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // create logger, controller , modal ,server.
             
        }

        private void onMessage(object sender, MessageRecievedEventArgs e)
        {
            eventLog1.WriteEntry(e.Status + ":" + e.Message);
        }

        protected override void OnStop()
        {
            logger.Log("On stop", Logging.Modal.MessageTypeEnum.INFO); ;

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            //OnPending(serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            m_imageServer.onCloseService();
        }


        /* protected override void OnShutdown()
         {
             eventLog1.WriteEntry("In OnShutdown.");
         }

         protected override void OnPause()
         {
             eventLog1.WriteEntry("In OnPause.");
         }

         private void OnPending(ServiceStatus serviceStatus)
         {
             eventLog1.WriteEntry("In Pending.");

             serviceStatus.dwWaitHint = 100000;
             SetServiceStatus(this.ServiceHandle, ref serviceStatus);
         }
         */
    }
}
