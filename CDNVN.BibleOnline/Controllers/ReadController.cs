using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public ActionResult Index(string v="", string q="", int from=1, int to=66)
        {
            if (string.IsNullOrWhiteSpace(v))
            {
                v = "btt";
            }
            v = v.ToLower();
            ViewBag.Bible = new SelectList(db.Bibles, "Version", "Name", v);
            ViewBag.FromBook = new SelectList(db.Books.Where(b=>b.Bible.Version.ToLower()==v), "Order", "Name", from);
            ViewBag.ToBook = new SelectList(db.Books.Where(b => b.Bible.Version.ToLower() == v), "Order", "Name", to);
            if (string.IsNullOrWhiteSpace(q)||new Regex("/d+$").IsMatch(q))
            {
                return View();
            }
            var address = new BibleAddress(q);
            var r = db.Contents.Where(c => c.Book.Bible.Version.ToLower() == v.ToLower())
                .Where(c => c.Book.CodeName.ToLower() == address.Book.ToLower() || c.Book.Name.ToLower().StartsWith(address.Book))
                .Where(c => c.Chapter >= address.From.Chapter && c.Chapter <= address.To.Chapter)
                .Where(c => !(c.Chapter == address.From.Chapter && c.Verse < address.From.Verse))
                .Where(c => !(c.Chapter == address.To.Chapter && c.Verse > address.To.Verse))
                .OrderBy(c=>c.Chapter).ThenBy(c=>c.Verse);
            var book = r.First().Book.Name;
            ViewBag.Title = book + " " + address.Adress;
                

            return View(r.ToList());
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
