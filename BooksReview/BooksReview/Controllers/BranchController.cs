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
    public class BranchController : Controller
    {
        private BooksContext db = new BooksContext();

        // GET: Branches
        public ActionResult Index()
        {
            var branches = db.Branches;
            return View(branches.ToList());
        }




    }
}
