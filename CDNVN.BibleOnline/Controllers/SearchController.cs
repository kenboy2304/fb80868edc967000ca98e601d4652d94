using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDNVN.BibleOnline.Models;
using PagedList;

namespace CDNVN.BibleOnline.Controllers
{
    public class SearchController : Controller
    {
        private BibleDBEntities db = new BibleDBEntities();
        //
        // GET: /Search/
        public ActionResult Index(string v="", string q="", int page =1)
        {
            if (string.IsNullOrWhiteSpace(v))
                v = "btt";
            if (string.IsNullOrWhiteSpace(q)) return View();

            var contents =
                db.Contents.Where(c => c.Book.Bible.Version.ToLower() == v.ToLower()).Where(c => c.Tag.Contains(q))
                .OrderBy(c=>c.Verse).ThenBy(c=>c.Chapter).ThenBy(c=>c.BookId).ToPagedList(page,15);


            return View(contents);
        }


        //
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
