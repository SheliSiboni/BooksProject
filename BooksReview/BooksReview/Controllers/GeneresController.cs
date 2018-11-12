using BooksReview.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BooksReview.Controllers
{
    public class GeneresController : Controller
    {
        private BooksContext db = new BooksContext();

        // GET: Generes
        public ActionResult Index()
        {
            return View(db.Generes.ToList());
        }

        // GET: Generes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genere genere = db.Generes.Find(id);
            if (genere == null)
            {
                return HttpNotFound();
            }
            return View(genere);
        }

        // GET: Generes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Genere genere)
        {
            if (ModelState.IsValid)
            {
                db.Generes.Add(genere);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genere);
        }

        // GET: Generes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genere genere = db.Generes.Find(id);
            if (genere == null)
            {
                return HttpNotFound();
            }
            return View(genere);
        }

        // POST: Generes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Genere genere)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genere).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genere);
        }

        // GET: Generes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genere genere = db.Generes.Find(id);
            if (genere == null)
            {
                return HttpNotFound();
            }
            return View(genere);
        }

        // POST: Generes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Genere genere = db.Generes.Find(id);

            foreach (Book book in genere.Books)
            {
                if (book.Reviews != null && book.Reviews.Count > 0)
                {
                    foreach (Review review in book.Reviews)
                    {
                        if (review.Comments != null && review.Comments.Count > 0)
                        {
                            db.Comments.RemoveRange(review.Comments);
                        }
                    }

                    db.Reviews.RemoveRange(book.Reviews);
                }
            }

            db.Books.RemoveRange(genere.Books);            

            db.Generes.Remove(genere);
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
