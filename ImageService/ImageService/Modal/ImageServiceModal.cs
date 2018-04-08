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

        public string AddFile(string path, out bool result)
        {
            string msg="";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    Image image = Image.FromFile(path);
                    DateTime dataValue = (Convert.ToDateTime(path));
                    string yearPath = m_OutputFolder + "\\" + dataValue.Year.ToString();
                    string monthPath = yearPath + "\\" + dataValue.Month.ToString();
                    string thumbYearPath = m_OutputFolder + "\\" + "Thumbnails" + "\\" + dataValue.Year.ToString();
                    string thumbMonthPath = thumbYearPath + "\\" + dataValue.Month.ToString();
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
                        File.Copy(path, monthPath + "\\");
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
                        Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                        thumb.Save(thumbMonthPath + "\\" + Path.GetFileName(path));
                        msg += " Added a thumbnaile " + Path.GetFileName(path) + "to " + thumbMonthPath;
                    }

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

        #endregion

    }
}
