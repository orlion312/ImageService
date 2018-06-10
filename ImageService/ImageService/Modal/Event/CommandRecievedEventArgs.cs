using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ImageService.Modal
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory
        ///</summary>
         //  constructor
        /// </summary>
        /// <param name="id">command id</param name
        /// <param name="args">args for the command</param name
        /// <param name="path">path of requasted dir</param name
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }

        /// <summary>
        /// the method take the paraneters of the class, change them into json and return it
        /// </summary>
        /// <returns>a json that represent the all parameters</returns>
        public string ToJson()
        {
            try
            {
                JObject jStr = new JObject();
                jStr["CommandID"] = CommandID;
                jStr["RequestDirPath"] = RequestDirPath;
                jStr["Args"] = new JArray(Args);
                return jStr.ToString().Replace(Environment.NewLine, " ");
            } catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// the method get a string, split it to the parameters,
        /// convert it to CommandRecievedEventArgs object and return it.
        /// </summary>
        /// <param name="jStr">the string to split</param>
        /// <returns>a CommandRecievedEventArgs object</returns>
        public static CommandRecievedEventArgs FromJson(string jStr)
        {
            try
            {
                JObject jObject = JObject.Parse(jStr);
                int id = (int)jObject["CommandID"];
                JArray args = (JArray)jObject["Args"];
                string[] argsArr = args.Select(c => (string)c).ToArray();
                return new CommandRecievedEventArgs(id, argsArr, null);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return null;
            }
        }

    }
}