﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDNVN.BibleOnline.Models;

namespace CDNVN.BibleOnline.Controllers
{
    public class ReadController : Controller
    {
        //
        // GET: /Read/
        private BibleDBEntities db = new BibleDBEntities();
        public ActionResult Index(string v="", string ad="")
        {
            ViewBag.BibleCode = new SelectList(db.Bibles, "Version", "Name", v);
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
