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
    public class BookManagerController : Controller
    {
        private BibleDBEntities db = new BibleDBEntities();

        // GET: /Admin/BookManager/
        public ActionResult Index(string v = "")
        {
            if (string.IsNullOrEmpty(v)) v = db.Bibles.First().Version;
            ViewBag.BibleCode = new SelectList(db.Bibles, "Version", "Name", v);
            var books = db.Books.Where(book => book.Bible.Version == v);
            return View(books.ToList());
        }

        // GET: /Admin/BookManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: /Admin/BookManager/Create
        public ActionResult Create()
        {
            ViewBag.BibleId = new SelectList(db.Bibles, "Id", "Version");
            return View();
        }

        // POST: /Admin/BookManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CodeName,Name,Order,BibleId")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BibleId = new SelectList(db.Bibles, "Id", "Version", book.BibleId);
            return View(book);
        }

        // GET: /Admin/BookManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.BibleId = new SelectList(db.Bibles, "Id", "Version", book.BibleId);
            return View(book);
        }

        // POST: /Admin/BookManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CodeName,Name,Order,BibleId")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BibleId = new SelectList(db.Bibles, "Id", "Version", book.BibleId);
            return View(book);
        }

        // GET: /Admin/BookManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: /Admin/BookManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Insert(int id, string lang)
        {
            dynamic objvn = new
            {
                Books ="Sáng-thế Ký,Xuất Ê-díp-tô Ký,Lê-vi Ký,Dân-số Ký,Phục-truyền Luật-lệ Ký,Giô-suê,Các Quan Xét,Ru-tơ,1 Sa-mu-ên,2 Sa-mu-ên,1 Các Vua,2 Các Vua,1 Sử-ký,2 Sử-ký,Ê-xơ-ra,Nê-hê-mi,Ê-xơ-tê,Gióp,Thi-thiên,Châm-ngôn,Truyền-đạo,Nhã-ca,Ê-sai,Giê-rê-mi,Ca-thương,Ê-xê-chi-ên,Đa-ni-ên,Ô-sê,Giô-ên,A-mốt,Áp-đia,Giô-na,Mi-chê,Na-hum,Ha-ba-cúc,Sô-phô-ni,A-ghê,Xa-cha-ri,Ma-la-chi,Ma-thi-ơ,Mác,Lu-ca,Giăng,Công-vụ các Sứ-đồ,Rô-ma,1 Cô-rinh-tô,2 Cô-rinh-tô,Ga-la-ti,Ê-phê-sô,Phi-líp,Cô-lô-se,1 Tê-sa-lô-ni-ca,2 Tê-sa-lô-ni-ca,1 Ti-mô-thê,2 Ti-mô-thê,Tít,Phi-lê-môn,Hê-bơ-rơ,Gia-cơ,1 Phi-e-rơ,2 Phi-e-rơ,1 Giăng,2 Giăng,3 Giăng,Giu-đe,Khải-huyền".Split(','),
                Abbreviations = "Sa,Xu,Le,Dan,Phu,Gios,Cac,Ru,1Sa,2Sa,1Vua,2Vua,1Su,2Su,Exo,Ne,Et,Giop,Thi,Ch,Tr,Nha,Es,Gie,Ca,Exe,Da,Os,Gio,Am,Ap,Gion,Mi,Na,Ha,So,Ag,Xa,Ma,Mat,Mac,Lu,Gi,Cong,Ro,1Co,2Co,Ga,Eph,Phi,Co,1Te,2Te,1Ti,2Ti,Tit,Phil,He,Gia,1Phi,2Phi,1Gi,2Gi,3Gi,Giu,Kh".Split(',')
            };
            dynamic objen = new 
            {
                Books ="Genesis,Exodus,Leviticus,Numbers,Deuteronomy,Joshua,Judges,Ruth,1 Samuel,2 Samuel,1 Kings,2 Kings,1 Chronicles,2 Chronicles,Ezra,Nehemiah,Esther,Job,Psalms,Proverbs,Ecclesiastes,Song of Solomon,Isaiah,Jeremiah,Lamentations,Ezekiel,Daniel,Hosea,Joel,Amos,Obadiah,Jonah,Micah,Nahum,Habakkuk,Zephaniah,Haggai,Zechariah,Malachi,Matthew,Mark,Luke,John,Acts,Romans,1 Corinthians,2 Corinthians,Galatians,Ephesians,Philippians,Colossians,1 Thessalonians,2 Thessalonians,1 Timothy,2 Timothy,Titus,Philemon,Hebrews,James,1 Peter,2 Peter,1 John,2 John,3 John,Jude,Revelation".Split(','),
                Abbreviations = "Gen,Exod,Lev,Num,Deut,Josh,Judg,Ruth,1 Sam,2 Sam,1 Kgs,2 Kgs,1 Chr,2 Chr,Ezra,Neh,Esth,Job,Ps(s),Prov,Eccl,Song,Isa,Jer,Lam,Ezek,Dan,Hos,Joel,Amos,Obad,Jonah,Mic,Nah,Hab,Zeph,Hag,Zech,Mal,Matt,Mark,Luke,John,Acts,Rom,1 Cor,2 Cor,Gal,Eph,Phil,Col,1 Thess,2 Thess,1 Tim,2 Tim,Titus,Phlm,Heb,Jas,1 Pet,2 Pet,1 John,2 John,3 John,Jude,Rev".Replace(" ","").Split(',')
            };
            dynamic obj = new object();
            var arrTotalChapter ="50,40,27,36,34,24,21,4,31,24,22,25,29,36,10,13,10,42,150,31,12,8,66,52,5,48,12,14,3,9,1,4,7,3,3,3,2,14,4,28,16,24,21,28,16,16,13,6,6,4,4,5,3,6,4,3,1,13,5,5,3,5,1,1,1,22".Split(',');
            if (lang == "vi" || lang == "vn")
            {
                obj = objvn;
            }
            if (lang == "en")
            {
                obj = objen;
            }
            var i = 1;

            foreach (var b in obj.Books)
            {
                db.Books.Add(new Book
                {
                    Name = b,
                    Order = i,
                    BibleId = id,
                    TotalChapter = Int32.Parse(arrTotalChapter[i - 1]),
                    CodeName = obj.Abbreviations[i - 1],
                    Group = i<=39?1:2
                });
                db.SaveChanges();
                i++;
            }

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
