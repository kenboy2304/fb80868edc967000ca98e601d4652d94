using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDNVN.BibleOnline.Models;

namespace CDNVN.BibleOnline.Controllers
{
    public class SearchController : Controller
    {
        private BibleDBEntities db = new BibleDBEntities();
        //
        // GET: /Search/
        public ActionResult Index()
        {
            return View();
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
