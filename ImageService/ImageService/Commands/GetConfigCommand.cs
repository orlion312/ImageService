using ImageService.Commands;
using ImageService.Modal;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace ImageService.ImageService.Commands
{
    public class GetConfigCommand : ICommand
    {
        /// <summary>
        /// Execute the GetConfigCommand.
        /// </summary>
        /// <param name="args">The parametrs value of each setting</param>
        /// <param name="result"> True if we succeed the excute</param>
        /// <returns>string of all the parametrs.</returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                JObject j = new JObject();
                j["SourceName"] = ConfigurationManager.AppSettings["SourceName"];
                j["ThumbnailSize"] = ConfigurationManager.AppSettings["ThumbnailSize"];
                j["LogName"] = ConfigurationManager.AppSettings["LogName"];
                j["Handler"] = ConfigurationManager.AppSettings["Handler"];
                j["OutputDir"] = ConfigurationManager.AppSettings["OutputDir"];
                result = true;
                string ret = "Config " + j.ToString().Replace(Environment.NewLine, " ");
                return ret;
            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}

