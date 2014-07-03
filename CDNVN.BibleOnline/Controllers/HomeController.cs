using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDNVN.BibleOnline.Models;

namespace CDNVN.BibleOnline.Controllers
{
    public class HomeController : Controller
    {
        private BibleDBEntities db = new BibleDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string v = "", string ad = "")
        {
            if (!string.IsNullOrWhiteSpace(v) && !string.IsNullOrWhiteSpace(ad))
            {
                return RedirectToAction("Index", "Read", new {v = v, ad = ad});
            }
            return View();
        }
        public ActionResult _SearchForm()
        {
            var v = Request.QueryString["v"];
            ViewBag.BibleCode = new SelectList(db.Bibles, "Version", "Name",v);
            return PartialView("_SearchForm");
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