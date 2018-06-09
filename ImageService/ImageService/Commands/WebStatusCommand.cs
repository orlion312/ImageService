using ImageService.Commands;
using ImageService.Modal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Commands
{
    class WebStatusCommand:ICommand
    {
        private IImageServiceModal m_modal;
        private string serviceStatus;
        private int imageCounter;

        public WebStatusCommand(string servicestatus, IImageServiceModal modal)
        {
            m_modal = modal;
            this.serviceStatus = servicestatus;

        }
        /// <summary>
        /// Execute the GetConfigCommand.
        /// </summary>
        /// <param name="args">The parametrs value of each setting</param>
        /// <param name="result"> True if we succeed the excute</param>
        /// <returns>string of all the parametrs.</returns>
        public string Execute(string[] args, out bool result)
        {
            this.imageCounter = m_modal.CountImages();
            try
            {
                JObject j = new JObject();
                j["ServiceStatus"] = this.serviceStatus;
                j["ImageCounter"] = this.imageCounter;
                result = true;
                string ret = "Web " + j.ToString().Replace(Environment.NewLine, " ");
                return ret;
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
