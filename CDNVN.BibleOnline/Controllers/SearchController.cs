using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using CDNVN.BibleOnline.Models;
using CDNVN.BibleOnline.Resolves;
using PagedList;

namespace CDNVN.BibleOnline.Controllers
{
    public class SearchController : Controller
    {
        private BibleDBEntities db = new BibleDBEntities();
        //
        // GET: /Search/
        public ActionResult Index(string v = "", string q = "", int from = 1, int to = 66, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(v))
                v = "btt";
            ViewBag.Bible = new SelectList(db.Bibles, "Version", "Name", v);
            ViewBag.FromBook = new SelectList(db.Books.Where(b => b.Bible.Version.ToLower() == v), "Order", "Name", from);
            ViewBag.ToBook = new SelectList(db.Books.Where(b => b.Bible.Version.ToLower() == v), "Order", "Name", to);
            return View();
        }

        public ActionResult Json(string v = "", string q = "", int from = 1, int to = 66, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(v) || string.IsNullOrWhiteSpace(q))
            {
                return null;
            }
            q = HttpUtility.HtmlDecode(q);
            if (string.IsNullOrWhiteSpace(v))
                v = "btt";
            if (string.IsNullOrWhiteSpace(q)) return Json(null, JsonRequestBehavior.AllowGet);
            var key = RemoveVietnamese.Change(q).ToLower();
            var contents =
                db.Contents.Where(c => c.Book.Bible.Version.ToLower() == v.ToLower())
                    .Where(c => c.Book.Order >= from && c.Book.Order <= to)
                    .Where(c => c.Tag.ToLower().Contains(key))
                    .OrderBy(c => c.BookId).ThenBy(c => c.Chapter).ThenBy(c => c.Verse).ToPagedList(page, 15);

            return Json(new
            {
                end = true,
                total = contents.TotalItemCount,
                contents = contents.Select(c => new
                {
                    Url = Url.Action("Index", "Read", new {v = v, q = c.Book.Name + " " + c.Chapter + ":" + c.Verse}),
                    BookName = c.Book.Name,
                    Adress = c.Chapter + ":" + c.Verse,
                    Word =
                        c.Word.Replace(c.Word.Substring(c.Tag.ToLower().IndexOf(key.ToLower()), q.Length),
                            "<strong style='color:red;'>" +
                            c.Word.Substring(c.Tag.ToLower().IndexOf(key.ToLower()), q.Length) + "</strong>")
                }),
            },

        JsonRequestBehavior.AllowGet);
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
