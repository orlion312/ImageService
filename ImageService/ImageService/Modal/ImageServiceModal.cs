using System;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        public string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        private string yearPath;
        private string monthPath;
        private string thumbYearPath;
        private string thumbMonthPath;
        private Image image;
        private static Regex r = new Regex(":");
        #endregion

        /// <summary>
        /// the constructor of the class, get the output directory and the size of the thumbnile
        /// </summary>
        /// <param name="output">A string of the output directory</param>
        /// <param name="thumbnailSize">an Integer of the thumbnile size</param>
        public ImageServiceModal (string output, int thumbnailSize)
        {
            this.m_OutputFolder = output;
            this.m_thumbnailSize = thumbnailSize;
        }

        /// <summary>
        /// method of the class, get a path and a boolean, take the information data from the path file,
        /// create the directory that need, move the file, create the thumbnile and return a boolean and a string 
        /// </summary>
        /// <param name="path">A string that represent the file path</param>
        /// <param name="result">A boolean that represent the result of the model</param>
        /// <returns>a string that represent the msg to write in the logging and a 
        /// boolean that represent of transport succesed or not</returns>
        public string AddFile(string path, out bool result)
        {
            //create the directory Images and make it hidden 
            Directory.CreateDirectory(m_OutputFolder).Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            string msg="";
            string filePath = "";
            try
            {
                if (File.Exists(path))
                {
                    //call the function HasImageExtension to check if the file is an image
                    if(!HasImageExtension(path)) {
                        throw new Exception("File is not an image");
                    }
                    //call the function getDate to take the information of the file
                    getDate(path);
                    
                    if (!Directory.Exists(monthPath)) 
                    {
                        Directory.CreateDirectory(monthPath);
                    }

                    //move the file to the appropriate directory
                    filePath = SameName(path, monthPath);
                    File.Copy(path, filePath);
                    msg = "Added " + Path.GetFileName(path) + " to " + monthPath;

                    if (!Directory.Exists(thumbMonthPath))
                    {
                        Directory.CreateDirectory(thumbMonthPath);
                    }

                    //create the thumbnile
                    filePath = SameName(path, thumbMonthPath);
                    Image thumb = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
                    thumb = (Image)(new Bitmap(thumb, new Size(m_thumbnailSize, m_thumbnailSize)));
                    thumb.Save(filePath);
                    msg += " and Added a thumbnail " + Path.GetFileName(path) + " to " + thumbMonthPath;
                    thumb.Dispose();

                    image.Dispose();
                    File.Delete(path);
                    result = true;
                    return msg;
                } else
                {
                    throw new Exception("File doesn't exists");
                }
            } catch (Exception e) {
                result = false;
                return e.ToString();
            }
        }

        /// <summary>
        /// the method get a path of file and take the information from it,
        /// and create the paths of the other directiries, the year path, the month path.
        /// </summary>
        /// <param name="path">A string that represent the file path</param>
        private void getDate(string path)
        {
            image = Image.FromFile(path);
            //call the function GetDateTakenFromImage to get the information
            try
            {
                DateTime dataValue = GetDateTakenFromImage(path);
                yearPath = m_OutputFolder + "\\" + dataValue.Year.ToString();
                monthPath = yearPath + "\\" + dataValue.Month.ToString();
                thumbYearPath = m_OutputFolder + "\\" + "Thumbnails" + "\\" + dataValue.Year.ToString();
                thumbMonthPath = thumbYearPath + "\\" + dataValue.Month.ToString();
            } catch(Exception e)
            {
                DateTime dataValue = File.GetCreationTime(path);
                yearPath = m_OutputFolder + "\\" + dataValue.Year.ToString();
                monthPath = yearPath + "\\" + dataValue.Month.ToString();
                thumbYearPath = m_OutputFolder + "\\" + "Thumbnails" + "\\" + dataValue.Year.ToString();
                thumbMonthPath = thumbYearPath + "\\" + dataValue.Month.ToString();
            }
        }

        /// <summary>
        /// the method get a file path and get the date information from the file
        /// </summary>
        /// <param name="path">A string that represent the file path</param>
        /// <returns>a DteTime that includes the detailes from the file</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        /// <summary>
        /// the method get a file path and check if the file is an image
        /// </summary>
        /// <param name="path">A string that represent the file path</param>
        /// <returns>a boolean that represent true if the file is an image and false otherwise</returns>
        private bool HasImageExtension(string path)
        {
            path = path.ToLower();
            return (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".gif") || path.EndsWith(".bmp"));
        }

        /// <summary>
        /// the methood check if the file is already exists in the output
        /// directory and if it does it change his name
        /// </summary>
        /// <param name="pathFile">A string that represent the file path</param>
        /// <param name="pathDir">A string that represent the output directory</param>
        /// <returns>A string that represnt the name of the file</returns>
        private string SameName(string pathFile, string pathDir)
        {
            int counter = 0;
            string fileNamePath = pathDir + "\\" + Path.GetFileName(pathFile);

                while (File.Exists(fileNamePath))
                {
                    counter++;
                    fileNamePath = pathDir + "\\" + Path.GetFileNameWithoutExtension(pathFile) + "(" + counter.ToString() + ")" + Path.GetExtension(pathFile);
            }
            return fileNamePath;
        }

        /// <summary>
        /// the method responsible to count the number of photos in the output folder
        /// </summary>
        /// <returns>an Integer that represent the number of photos</returns>
        public int CountImages()
        {
            int numImages = 0;
            string[] yearDirs = Directory.GetDirectories(m_OutputFolder);
            foreach(string dir in yearDirs)
            {
                if(dir.Contains("Thumbnails"))
                {
                    continue;
                }

                string[] monthDirs= Directory.GetDirectories(dir);
                foreach(string subDir in monthDirs)
                {
                    numImages += Directory.GetFiles(subDir).Length;
                }
            }
            return numImages;
        }
    }
}
