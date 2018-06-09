using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using ImageCommunication;
using ImageCommunication.Events;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace ImageServiceWeb.Models
{
    public class ConfigDelete
    {

        public ConfigDelete(ConfigModel config)
        {
            m_outputDirectory = config.m_outputDirectory;
            m_logName = config.m_logName;
            m_sourceName = config.m_sourceName;
            m_ThumbnailSize = config.m_ThumbnailSize;
            m_handlers = config.m_handlers;
        }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory: ")]
        public string m_outputDirectory { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name: ")]
        public string m_sourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name: ")]
        public string m_logName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Size: ")]
        public string m_ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers: ")]
        public ObservableCollection<string> m_handlers { get; set; }        //string m_OutputDirectory

        public void varSet(string output, string log, string source, string thunbmail, ObservableCollection<string> handlers)
        {
            m_outputDirectory = output;
            m_logName = log;
            m_sourceName = source;
            m_ThumbnailSize = thunbmail;
            m_handlers = handlers;
        }

    }
}