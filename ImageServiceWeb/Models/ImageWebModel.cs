﻿using ImageCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageCommunication.Events;
using System.ComponentModel.DataAnnotations;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;

namespace ImageServiceWeb.Models
{
    public class ImageWebModel
    {
        private ITcpClient m_client;
        public List<Student> students;
        private bool stop;

        public ImageWebModel()
        {
            string path = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data", "StudentsDetails.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            stop = false;
            string[] s = lines[0].Split(' ');
            string[] s1 = lines[1].Split(' ');
            students = new List<Student>()
            {
                new Student {FirstName = s[0], LastName = s[1], ID = s[2]},
                new Student {FirstName = s1[0], LastName = s1[1], ID = s1[2]}
            };
            student = students;

            try
            {
                m_client = TcpClientChannel.ClientInstance;
                m_client.DataReceived += GetMessageFromClient;

                m_client.Send(new CommandRecievedEventArgs((int)CommandEnum.WebStatusCommand, null, null).ToJson());
                SpinWait.SpinUntil(() => stop);
            }
            catch (Exception e)
            {
                m_status = "Offline";
                imageCount = "0";
                Console.Write(e.ToString());
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Image Service Status: ")]
        public string m_status { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students: ")]
        public List<Student> student { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public Student yarin { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Image Service Photos Count: ")]
        public string imageCount { get; set; }

        private void GetMessageFromClient(object sender, DataReceivedEventArgs data)
        {
            string message = data.Message;
            if (message.Contains("Web "))
            {
                System.Diagnostics.Debug.WriteLine("Working on config...");
                int i = message.IndexOf(" ") + 1;
                message = message.Substring(i);
                JObject json = JObject.Parse(message);
                m_status = (string)json["ServiceStatus"];
                imageCount = (string)json["ImageCounter"];
                System.Diagnostics.Debug.WriteLine("Done!");
            }
            stop = true;
        }
    }
}