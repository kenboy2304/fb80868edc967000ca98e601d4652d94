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
        public ActionResult Index(string v ="btt")
        {
            return View(db.Books.Where(book=>book.Bible.Version.ToLower()==v.ToLower()));
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
            if (string.IsNullOrWhiteSpace(v)) v = "btt";
            ViewBag.BibleCode = new SelectList(db.Bibles, "Version", "Name",v);
            return PartialView("_SearchForm");
        }

        public ActionResult AutoComplate(string q = "")
        {

            if (string.IsNullOrWhiteSpace(q))
                return null;
            q = q.ToLower().Replace("-", " ");
            var data = db.Books.Where(b => b.Name.Replace("-"," ").ToLower().StartsWith(q) || b.CodeName.ToLower().StartsWith(q));
            var dataArray = new List<string>();
            foreach (var book in data)
            {
                if (!dataArray.Exists(t => t.ToLower() == book.CodeName.ToLower()))
                {
                    dataArray.Add(book.CodeName);
                    dataArray.Add(book.Name);
                }
  
            }
            return Json(dataArray, JsonRequestBehavior.AllowGet);
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