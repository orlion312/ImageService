using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    //the controller of the web
    public class HomeController : Controller
    {
        static ConfigModel config = new ConfigModel();
        static readonly LogsModel logs = new LogsModel();
        static string handler="";
        static string pathPic = ""; 
        static ImageWebModel imageWeb = new ImageWebModel();
        public PhotosModel photoModel= new PhotosModel(config);

        //the method responsible for the ImageWeb page
        public ActionResult ImageWeb()
        {
            imageWeb.sendCommand();
            return View(imageWeb);
        }

        //the method responsible for the logs page
        public ActionResult Logs()
        {
            logs.tryConnect();
            return View(logs);
        }

        //the method responsible for the config page
        public ActionResult Config()
        {
            config.sendCommand();
            return View(config);
        }

        //the method responsible for the photos page
        public ActionResult Photos()
        {
            return View(photoModel);
        }

        //the method responsible for the hadler remove page
        public ActionResult ConfigRemove(string dir)
        {
            ViewBag.Test = dir;
            handler = dir;
            return View();
        }

        //the method responsible for the delete the handler
        public ActionResult HandlerDelete(string dir)
        {
            config.RemoveHandler(handler);
            //wait till we get an approve from the service
            SpinWait.SpinUntil( () => config.isDelete);
            return RedirectToAction("Config","Home");
        }
        //the method responsible for photo view
        public ActionResult ViewPhoto(string pic, string name, string date, string thumb)
        {
            ViewBag.Pic = pic;
            ViewBag.Name = name;
            ViewBag.Date = date;
            ViewBag.Thumb = thumb;
            pathPic=thumb;
            return View();
        }
        //the method responsible for removal of the picture
        public ActionResult PhotoRemove(string dir)
        {
            ViewBag.picPath = dir;
            pathPic = dir;
            return View();
        }
        //removes the picture if the user press OK
        public ActionResult PictureDeleteOK()
        {
            photoModel.RemovePicture(pathPic);
            return RedirectToAction("Photos");
        }
        //links back to photos when the user press Cancel
        public ActionResult PicturDeleteCancel()
        {
            return RedirectToAction("Photos");
        }

    }
}