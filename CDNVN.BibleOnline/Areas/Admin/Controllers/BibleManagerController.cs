using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CDNVN.BibleOnline.Models;

namespace CDNVN.BibleOnline.Areas.Admin.Controllers
{
    public class BibleManagerController : Controller
    {
        private BibleDBEntities db = new BibleDBEntities();

        // GET: /Admin/BibleManager/
        public ActionResult Index()
        {
            var bibles = db.Bibles.Include(b => b.Language);
            return View(bibles.ToList());
        }

        public ActionResult CheckBible()
        {
            var bibles = db.Bibles.Include(b => b.Language);
            return View(bibles.ToList());
        }

        // GET: /Admin/BibleManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bible bible = db.Bibles.Find(id);
            if (bible == null)
            {
                return HttpNotFound();
            }
            return View(bible);
        }

        // GET: /Admin/BibleManager/Create
        public ActionResult Create()
        {
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Code");
            return View();
        }

        // POST: /Admin/BibleManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Version,Name,LanguageId")] Bible bible)
        {
            if (ModelState.IsValid)
            {
                db.Bibles.Add(bible);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Code", bible.LanguageId);
            return View(bible);
        }

        // GET: /Admin/BibleManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bible bible = db.Bibles.Find(id);
            if (bible == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Code", bible.LanguageId);
            return View(bible);
        }

        // POST: /Admin/BibleManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Version,Name,LanguageId")] Bible bible)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bible).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Code", bible.LanguageId);
            return View(bible);
        }

        // GET: /Admin/BibleManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bible bible = db.Bibles.Find(id);
            if (bible == null)
            {
                return HttpNotFound();
            }
            return View(bible);
        }

        // POST: /Admin/BibleManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bible bible = db.Bibles.Find(id);
            db.Bibles.Remove(bible);
            db.SaveChanges();
            return RedirectToAction("Index");
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
