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
                    .Books.Single(book => book.CodeName.ToLower() == arr[0].ToLower())
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
        public ActionResult GetHtml(string version)
        {
            var abb = "Gen,Exod,Lev,Num,Deut,Josh,Judg,Ruth,1Sam,2Sam,1Kgs,2Kgs,1Chr,2Chr,Ezra,Neh,Esth,Job,Ps,Prov,Eccl,Song,Isa,Jer,Lam,Ezek,Dan,Hos,Joel,Amos,Obad,Jonah,Mic,Nah,Hab,Zeph,Hag,Zech,Mal,Matt,Mark,Luke,John,Acts,Rom,1 Cor,2 Cor,Gal,Eph,Phil,Col,1Thess,2Thess,1Tim,2Tim,Titus,Phlm,Heb,Jas,1Pet,2Pet,1John,2John,3John,Jude,Rev".Replace(" ", "").Split(',');

            const string urlFormat = "http://bibles.org/vie-RVV11/{0}/{1}";
            var url = string.Format(urlFormat, abb[33],1);
            var html = "";
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                html = client.DownloadString(url);
                //...
            }

            // ReSharper disable StringIndexOfIsCultureSpecific.1
            html = html.Remove(0, html.IndexOf("class=\"chapter\">") + "class=\"chapter\">".Length);
            html = html.Remove(html.IndexOf("<div class=\"copyright-statement\">"), html.Length - html.IndexOf("<div class=\"copyright-statement\">"));

            html = new Regex(@"<.h[0-9]*?>").Replace(html, "%");
            html = new Regex(@"<h[0-9]...........").Replace(html, "%");
            html = new Regex(@"<.*?>").Replace(html, "~");
            html = new Regex(@"\s~|~\s").Replace(html, "~");
            html = new Regex(@"~+").Replace(html, "~");
            html = html.Replace("~+", " ");
            html = html.Replace("%>", "%");
            html = html.Replace("~%", "~0~%");
            html = new Regex(@"%\s").Replace(html, "");
            html = new Regex(@"\s+~").Replace(html, "~");
            html = new Regex(@"~$").Replace(html, "");
            //return new Regex(@"-\d+-").Split(html);

            var bibleGayway = "Sáng Thế,Xuất Hành,Lê-vi,Dân Số,Phục Truyền Luật Lệ,Giô-sua,Các Thủ Lãnh,Ru-tơ,I Sa-mu-ên,II Sa-mu-ên,I Các Vua,II Các Vua,I Sử Ký,II Sử Ký,Ê-xơ-ra,Nê-hê-mi-a,Ê-xơ-tê,Gióp,Thánh Thi,Châm Ngôn,Giảng Sư,Nhã Ca,I-sai-a,Giê-rê-mi-a,Ai Ca,Ê-xê-ki-ên,Ða-ni-ên,Hô-sê-a,Giô-ên,A-mốt,Ô-ba-đi-a,Giô-na,Mi-ca,Na-hum,Ha-ba-cúc,Xê-pha-ni-a,Ha-gai,Xê-ca-ri-a,Ma-la-ki,Ma-thi-ơ,Mác,Lu-ca,Giăng,Công Vụ Các Sứ đồ,Rô-ma,I Cô-rinh-tô,II Cô-rinh-tô,Ga-la-ti,Ê-phê-sô,Phi-líp,Cô-lô-se,I Thê-sa-lô-ni-ca,II Thê-sa-lô-ni-ca,I Ti-mô-thê,II Ti-mô-thê,Tít,Phi-lê-môn,Hê-bơ-rơ,Gia-cơ,I Phi-rơ,II Phi-rơ,I Giăng,II Giăng,III Giăng,Giu-đe,Khải Huyền".Split(',');
            //var urlFormat = "https://www.biblegateway.com/passage/?search={1}+{2}&version=VIET";
            //var html = "";
            //using (var client = new WebClient())
            //{
            //    client.Encoding = System.Text.Encoding.UTF8;
            //    html = client.DownloadString("https://www.biblegateway.com/passage/?search=Sáng Thế+2&version=Viet");
            //    //...
            //}
            //html = html.Remove(0, html.IndexOf("<p class=\"chapter"));
            //html = html.Remove(html.IndexOf("<div class=\"publisher-info-bottom\">"), html.Length - html.IndexOf("<div class=\"publisher-info-bottom\">"));

            return View((object)html);
        }

        public ActionResult GetBibleA(string version)
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
