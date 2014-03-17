﻿using System;
using System.Net;
using System.Linq;
using System.Web.Mvc;

namespace Kendo.Controllers
{
    public class IntegrationController : BaseController
    {
        private void SetDebug() {
#if DEBUG
            ViewBag.Debug = true;
#else
            ViewBag.Debug = false;
#endif
        }

        public ActionResult Sushi()
        {
            SetDebug();
            return View();
        }

        public ActionResult Simulator()
        {
            return RedirectPermanent(Url.RouteUrl("MobileApplication", new { app = "sushi" }));
        }

        public ActionResult Bootstrap()
        {
            SetDebug();
            return View();
        }
    }
}
