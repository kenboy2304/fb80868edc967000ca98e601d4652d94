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
        public ActionResult Index(string v="", string ad="")
        {
            if (string.IsNullOrWhiteSpace(v))
            {
                v = "btt";
            }
            if (string.IsNullOrWhiteSpace(ad)||new Regex("/d+$").IsMatch(ad))
            {
                return View();
            }
            var address = new BibleAddress(ad);
            ViewBag.Title = address.Book + " " + address.From.Chapter + ":" + address.From.Verse + "-" +
                            address.To.Chapter + ":" + address.To.Verse;
            var r = db.Contents.Where(c => c.Book.Bible.Version.ToLower() == v.ToLower())
                .Where(c => c.Book.CodeName.ToLower() == address.Book.ToLower() || c.Book.Name.ToLower().StartsWith(address.Book))
                .Where(c => c.Chapter >= address.From.Chapter && c.Chapter <= address.To.Chapter)
                .Where(c => !(c.Chapter == address.From.Chapter && c.Verse < address.From.Verse))
                .Where(c => !(c.Chapter == address.To.Chapter && c.Verse > address.To.Verse))
                .OrderBy(c=>c.Chapter).ThenBy(c=>c.Verse);
                
            ViewBag.BibleCode = new SelectList(db.Bibles, "Version", "Name", v);
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
