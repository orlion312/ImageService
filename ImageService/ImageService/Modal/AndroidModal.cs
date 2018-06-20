﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageCommunication.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Modal
{
    public class AndroidModal
    {
        private string handler;
        private string outputDir;
        private AndroidClient androidClient;

        public AndroidModal(String oneHandler, String output)
        {
            androidClient = AndroidClient.Instance;
            androidClient.handlePicture += getPicture;
            handler = oneHandler;
            outputDir = output;
        }
        /// <summary>
        /// Gets the picture.
        /// </summary>
        /// <param name="picName">Name of the pic.</param>
        /// <param name="byteArray">The byte array.</param>
        private void getPicture(string picName, byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                checkIfExists(picName);
                File.WriteAllBytes(handler + "\\" + picName, byteArray);
            }
        }
        /// <summary>
        /// Checks if exists.
        /// </summary>
        /// <param name="picName">Name of the pic.</param>
        private void checkIfExists(string picName)
        {
            DirectoryInfo outputD = new DirectoryInfo(outputDir);
            foreach (DirectoryInfo year in outputD.EnumerateDirectories())
            {
                foreach (DirectoryInfo month in year.EnumerateDirectories())
                {
                    foreach (FileInfo file in month.EnumerateFiles())
                    {
                        if (file.Name.Equals(picName))
                        {
                            try
                            {
                                File.Delete(file.FullName);
                                String thumbnailsPath = outputDir + "\\" + "Thumbnails" + "\\"
                                    + year.Name + "\\" + month.Name + "\\" + picName;
                                File.Delete(thumbnailsPath);
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.Message);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
