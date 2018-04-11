using System;
using System.IO;
using System.Drawing;


namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        private string yearPath;
        private string monthPath;
        private string thumbYearPath;
        private string thumbMonthPath;
        private Image image;
        #endregion

        public ImageServiceModal (string output, int thumbnailSize)
        {
            this.m_OutputFolder = output;
            this.m_thumbnailSize = thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            string msg="";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    getDate(path);
                    if (!System.IO.Directory.Exists(yearPath))
                    {   
                        System.IO.Directory.CreateDirectory(yearPath);   
                    }
                     if (!System.IO.Directory.Exists(monthPath)) 
                    {
                        System.IO.Directory.CreateDirectory(monthPath);
                    }
                    if (!System.IO.File.Exists(monthPath + "\\" + Path.GetFileName(path)))
                    {
                        File.Copy(path, monthPath + "\\" + Path.GetFileName(path), true);
                        msg = "Added " + Path.GetFileName(path) + "to " + monthPath;
                    }

                    if (!System.IO.Directory.Exists(thumbYearPath))
                    {
                        System.IO.Directory.CreateDirectory(thumbYearPath);
                    }
                    if (!System.IO.Directory.Exists(thumbMonthPath))
                    {
                        System.IO.Directory.CreateDirectory(thumbMonthPath);
                    }

                    if (!System.IO.File.Exists(thumbMonthPath + "\\" + Path.GetFileName(path)))
                    {
                        Image thumb = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
                        thumb = (Image)(new Bitmap(thumb, new Size(m_thumbnailSize, m_thumbnailSize)));
                        thumb.Save(thumbMonthPath + "\\" + Path.GetFileName(path));
                        msg += " Added a thumbnail " + Path.GetFileName(path) + "to " + thumbMonthPath;
                        thumb.Dispose();
                    }
                    image.Dispose();
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

        private void getDate(string path)
        {
            image = Image.FromFile(path);
            DateTime dataValue = File.GetCreationTime(path);
            yearPath = m_OutputFolder + "\\" + dataValue.Year.ToString();
            monthPath = yearPath + "\\" + dataValue.Month.ToString();
            thumbYearPath = m_OutputFolder + "\\" + "Thumbnails" + "\\" + dataValue.Year.ToString();
            thumbMonthPath = thumbYearPath + "\\" + dataValue.Month.ToString();
        }

    }
}
