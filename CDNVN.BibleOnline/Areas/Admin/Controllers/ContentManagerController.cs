using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CDNVN.BibleOnline.Models;
using CDNVN.BibleOnline.Resolves;

namespace CDNVN.BibleOnline.Areas.Admin.Controllers
{
    public class ContentManagerController : Controller
    {
        private readonly BibleDBEntities _db = new BibleDBEntities();

        // GET: /Admin/Content/
        public ActionResult Index(string v,string ad)
        {
            if (string.IsNullOrEmpty(ad)||string.IsNullOrEmpty(v))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ad.IndexOf(' ')<0)
            {
                ad = ad + " 1";
            }
            var arr = ad.Split(' ');
            var contents =
                _db.Bibles.Single(bible => bible.Version.ToLower() == v.ToLower())
                    .Books.Single(book => book.CodeName.ToLower() == arr[0])
                    .Contents.Where(c => c.Chapter == Int32.Parse(arr[1]));
            return View(contents.ToList());
        }

        // GET: /Admin/Content/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = _db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: /Admin/Content/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(_db.Books, "Id", "CodeName");
            return View();
        }

        // POST: /Admin/Content/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Chapter,Verse,Title,Word,Tag,BookId")] Content content)
        {
            if (ModelState.IsValid)
            {
                _db.Contents.Add(content);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(_db.Books, "Id", "CodeName", content.BookId);
            return View(content);
        }

        // GET: /Admin/Content/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = _db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(_db.Books, "Id", "CodeName", content.BookId);
            return View(content);
        }

        // POST: /Admin/Content/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Chapter,Verse,Title,Word,Tag,BookId")] Content content)
        {
            if (ModelState.IsValid)
            {
                content.Tag = RemoveVietnamese.Change(content.Word);
                _db.Entry(content).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(_db.Books, "Id", "CodeName", content.BookId);
            return View(content);
        }

        // GET: /Admin/Content/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = _db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: /Admin/Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = _db.Contents.Find(id);
            _db.Contents.Remove(content);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetBible(string version)
        {
            var bible = _db.Bibles.Single(b => b.Version.ToLower() == version.ToLower());
            foreach (var book in bible.Books)
            {
                const string urlFormat = "http://www.vietchristian.com/cgi-bin/bibrowse2.exe?{1}&{2}&bn={0}";
                for (int i = 1; i <= book.TotalChapter;i++)
                {
                    var chapter = GetVerseFromUrl(string.Format(urlFormat, version, book.Order-1,i));
                    InsertChapter(book.Id, i, chapter);
                }
            }
            return Content("Thanh cong");
        }

        public void InsertChapter(int bookId, int chapter, string[] verses)
        {
            for (int i = 1; i <= verses.Length; i++)
            {
                InsertVerse(bookId,chapter,i,verses[i-1]);
            }
        }

        public void InsertVerse(int bookId, int chapter, int verse, string word)
        {
            _db.Contents.Add(new Content
            {
                BookId = bookId,
                Chapter = chapter,
                Verse = verse,
                Word = word,
                Tag = RemoveVietnamese.Change(word)
            });
            _db.SaveChanges();
        }


        public string[] GetVerseFromUrl(string url)
        {
            string html;
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                html = client.DownloadString(url);
                //...
            }
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            html = html.Remove(0,html.IndexOf("<a class='AVerse'"));

            html = html.Remove(html.IndexOf("<p><form id='form"), html.Length - html.IndexOf("<p><form id='form"));
            var htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            html = htmlRegex.Replace(html, "-");
           
            var i = 1;
            var listVerse = new List<String>();
            const string verseFormat = "-{0}-";
            while (true)
            {
                html = html.Remove(html.IndexOf(string.Format(verseFormat, i)), 4);
                if (html.IndexOf(string.Format(verseFormat, i + 1)) == -1)
                {
                    listVerse.Add(html);
                    break;
                }
                listVerse.Add(html.Substring(0, html.IndexOf(string.Format(verseFormat, i + 1))));
                html = html.Remove(0, html.IndexOf(string.Format(verseFormat, i + 1)));
                i++;
            }
            // ReSharper restore StringIndexOfIsCultureSpecific.1
            return listVerse.ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
