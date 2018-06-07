using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        static readonly ConfigModel config = new ConfigModel();
        //private string dir = "";

        public ActionResult ImageWeb()
        {
            return View();
        }

        public ActionResult Logs()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Config()
        {
            return View(config);
        }

        public ActionResult Photos()
        {
            return View();
        }

        public ActionResult ConfigRemove(string dir)
        {
            ViewBag.Test = dir;
            //ViewBag.RemoveHandler = new Func<string, ActionResult>(DeleteOK); 
            return View();
        }

        //public ActionResult DeleteOK(string dir)
        //{
        //    config.RemoveHandler(dir);
        //    return RedirectToAction("Config");
        //}

        //public ActionResult DeleteCancel()
        //{
        //    return RedirectToAction("Config");
        //}
    }
}