using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            return View(db.Bibles.Where(v=>v.Version.ToLower()=="btt"));
        }

        [HttpPost]
        public ActionResult Index(string v = "", string q = "")
        {
            if (string.IsNullOrWhiteSpace(v) || string.IsNullOrWhiteSpace(q)) return View();
            return RedirectToAction("Index", new Regex(@"\d+$").IsMatch(q) ? "Read" : "Search", new {v = v, q = q});
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