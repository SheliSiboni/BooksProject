using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BooksReview.Models;
using BooksReview.Models.ViewModels;

namespace BooksReview.Controllers
{
    public class UsersController : Controller
    {
        private BooksContext db = new BooksContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult FailedLogin()
        {
            return View();
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Gender,Username,FirstName,LastName,Password,IsAdmin")] User user)
        {
            var oRegisterRedirect = SaveUser(user, RedirectToAction("Index", "Home"));

            Session.Add("User", user);

            return oRegisterRedirect;
        }

        private ActionResult SaveUser(User user, RedirectToRouteResult redirectOnSucess)
        {
            if (!ModelState.IsValid || db.Users.Any(x => x.Username == user.Username))
            {
                return View(user);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return redirectOnSucess;
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Gender,Username,FirstName,LastName,Password,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login([Bind(Include = "Username,Password")] User loginCredentials)
        {
            var user = db.Users.SingleOrDefault(u => u.Username.Equals(loginCredentials.Username) && u.Password.Equals(loginCredentials.Password));

            if (user == null)
            {
                return RedirectToAction("FailedLogin", "Users");
            }

            Session.Add("User", user);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetGroupByGender()
        {
            var genderToUsers = db.Users.GroupBy(x => x.Gender, user => user, (gender, users) => new
            {
                Name = gender.ToString(),
                Count = users.Count()
            });

            return Json(genderToUsers, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Stats()
        {
            var userReviewsViewModels = (from user in db.Users
                                         join review in db.Reviews on user.Id equals review.User.Id
                                         select new
                                         {
                                             Id = user.Id,
                                             UserName = user.Username,
                                             FirstName = user.FirstName,
                                             LastName = user.LastName,
                                             Review = review
                                         }).GroupBy(x => x.Id).ToList()
                .Select(x => new UserReviewsViewModel
                {
                    UserName = x.First().UserName,
                    FirstName = x.First().FirstName,
                    LastName = x.First().LastName,
                    Reviews = x.Select(y => y.Review)
                });

            return View(userReviewsViewModels);
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
