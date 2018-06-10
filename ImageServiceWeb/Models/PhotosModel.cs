using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        private static string outputDir { get; set; }
        private string[] thumbnailArray { get; set; }
        public List<Dictionary<string, string>> dictionary { get; set; }
        private string thumbnailSize { get; set; }
        private static Image image;
        private static Regex r = new Regex(":");

        public PhotosModel(ConfigModel configModel)
        {
            try
            {
                outputDir = configModel.m_outputDirectory;
                thumbnailSize = configModel.m_ThumbnailSize;
                dictionary = new List<Dictionary<string, string>>();
                string thumbnailsPath = Path.Combine(outputDir, "Thumbnails");
                thumbnailArray = Directory.GetFiles(thumbnailsPath, "*", SearchOption.AllDirectories);
                string tempThumb;
                string tempPicture;
                DateTime date;

                foreach (string s in thumbnailArray)
                {
                    tempThumb = s.Replace(thumbnailsPath, "Images\\Thumbnails");
                    tempPicture = s.Replace(thumbnailsPath, "Images");
                    // Take the date from photos
                    byte[] imageBytes = File.ReadAllBytes(s.Replace(thumbnailsPath, outputDir));
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var image = Image.FromStream(ms);
                    }
                    try
                    {
                        date = GetDateTakenFromImage(s.Replace(thumbnailsPath, outputDir));
                    }
                    catch (Exception e)
                    {
                        date = File.GetCreationTime(s.Replace(thumbnailsPath, outputDir));
                    }
                    // crates a dictionary of dictionaries with Thumbnails, Picture, Name, Date as keys
                    // and a right string as value
                    dictionary.Add(new Dictionary<string, string>
                {
                    {"Thumbnails",  tempThumb}, {"Picture", tempPicture}, {"Name" , Path.GetFileNameWithoutExtension(tempPicture)}, {"Date", date.ToString()}
                });

                }
            }
            catch { }
        }
        /// <summary>
        /// The function removes the picture and thumbnail picture from the output directory.
        /// </summary>
        /// <param name="path">A string that represent the file path </param>
        public void RemovePicture(string path)
        {
            path = path.Replace("/", "\\");
            string fullPath = Path.Combine(outputDir.Replace("Images", ""), path);
            fullPath = fullPath.Replace("\\Thumbnails", "");
            File.Delete(fullPath);
            fullPath = Path.Combine(outputDir.Replace("Images", ""), path);
            File.Delete(fullPath);
        }


        /// <summary>
        /// The method gets a file path and gets the date information from the file
        /// </summary>
        /// <param name="path">A string that represent the file path</param>
        /// <returns>a DateTime that includes the detailes from the file</returns>
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
    }
}