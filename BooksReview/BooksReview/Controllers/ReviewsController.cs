using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BooksReview.Models;

namespace BooksReview.Controllers
{
    public class ReviewsController : Controller
    {
        private BooksContext db = new BooksContext();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Book).Include(r => r.User);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.Books, "Id", "Name");
            ViewBag.UserID = new SelectList(db.Users, "Id", "Username");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,CreationDate,UserID,BookID")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.CreationDate = DateTime.Now;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookID = new SelectList(db.Books, "Id", "Name", review.BookID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "Username", review.UserID);
            return View(review);
        }

        public ActionResult PostComment(int userId, int reviewId, string content)
        {
            db.Comments.Add(new Comment
            {
                Content = content,
                UserID = userId,
                ReviewID = reviewId,
                CreationDate = DateTime.Now
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.Books, "Id", "Name", review.BookID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "Username", review.UserID);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,CreationDate,UserID,BookID")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.CreationDate = DateTime.Now;
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.Books, "Id", "Name", review.BookID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "Username", review.UserID);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Search(string content, string title, DateTime? date, string bookName)
        {
            var dayAfterDate = date?.AddDays(1);

            return View(db.Reviews
                .Where(review =>
                    (!string.IsNullOrEmpty(content) && review.Content.ToLower().Contains(content.ToLower())) ||
                    !string.IsNullOrEmpty(bookName) && review.Book.Name.ToLower().Contains(bookName.ToLower()) ||
                    (!string.IsNullOrEmpty(title) && review.Title.ToLower().Contains(title.ToLower())) ||
                    (date.HasValue && dayAfterDate.HasValue && date < review.CreationDate && review.CreationDate < dayAfterDate))
                .OrderByDescending(x => x.CreationDate)
                .ToList());
        }
    }
}
