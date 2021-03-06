﻿using System;
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
    public class BooksController : Controller
    {
        private BooksContext db = new BooksContext();

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Genere);
            return View(books.ToList());
        }

        // GET: Books/Details/5
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

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.GenereID = new SelectList(db.Generes, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,GenereID,ImageUrl,Author")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenereID = new SelectList(db.Generes, "Id", "Name", book.GenereID);
            return View(book);
        }

        // GET: Books/Edit/5
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
            ViewBag.GenereID = new SelectList(db.Generes, "Id", "Name", book.GenereID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,GenereID,ImageUrl,Author")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenereID = new SelectList(db.Generes, "Id", "Name", book.GenereID);
            return View(book);
        }

        // GET: Books/Delete/5
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
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

            db.Books.Remove(book);
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
        public ActionResult Search(string name, string genere)
        {
            return View(db.Books
                .Where(book =>
                    (!string.IsNullOrEmpty(name) && book.Name.ToLower().Contains(name.ToLower())) ||
                    !string.IsNullOrEmpty(genere) && book.Genere.Name.ToLower().Contains(genere.ToLower()))
                .ToList());
        }
    } 
}
