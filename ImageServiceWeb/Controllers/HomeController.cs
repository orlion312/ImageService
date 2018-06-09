using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        static ConfigModel config = new ConfigModel();
        static readonly LogsModel logs = new LogsModel();
        static string handler="";

        public ActionResult ImageWeb()
        {
            return View(new ImageWebModel());
        }

        public ActionResult Logs()
        {
            return View(logs);
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
            handler = dir;
            return View();
        }

        public ActionResult HandlerDelete(string dir)
        {
            config.RemoveHandler(handler);
            SpinWait.SpinUntil( () => config.isDelete);
            return RedirectToAction("Config","Home");
        }

        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }
    }
}