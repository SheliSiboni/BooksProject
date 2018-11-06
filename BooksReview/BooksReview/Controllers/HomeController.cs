using Accord.MachineLearning.Bayes;
using BooksReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooksReview.Controllers
{
    public class HomeController : Controller
    {
        private BooksContext db = new BooksContext();

        public ActionResult Index()
        {
            Book recomendedBook = RecommendBook();

            return View(recomendedBook);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public Book RecommendBook()
        {
            Random rnd = new Random();
            Book selectedBook = null;
            List<Book> allBooksInGenere = null;
            int BooksCount = db.Books.Count();

            if (BooksCount == 0)
            {
                return new Book();
            }

            if (Session["User"] == null)
            {

                selectedBook = db.Books.OrderBy(r => Guid.NewGuid()).First();
            }
            else
            {
                int userID = ((User)Session["User"]).Id;

                int[] selectedGeneres = db.Reviews.Where(review => review.UserID == userID).Select(review => review.Book.GenereID).ToArray();

                if (selectedGeneres.Length == 0)
                {
                    selectedBook = db.Books.OrderBy(r => Guid.NewGuid()).First();
                }
                else if (selectedGeneres.Distinct().Count() == 1)
                {
                    int selectedGenereId = selectedGeneres[0];

                    allBooksInGenere = db.Generes.First(x => x.Id == selectedGenereId).Books;

                    selectedBook = allBooksInGenere[rnd.Next(allBooksInGenere.Count)];
                }
                else
                {
                    int[][] dataset = new int[selectedGeneres.Length][];

                    Dictionary<int, int> mapper = new Dictionary<int, int>();
                    Dictionary<int, int> mapperOpsite = new Dictionary<int, int>();

                    int counter = 0;

                    for (int genereIndex = 0; genereIndex < selectedGeneres.Length; genereIndex++)
                    {
                        dataset[genereIndex] = new int[1];

                        if (!mapper.ContainsKey(selectedGeneres[genereIndex]))
                        {
                            mapper[selectedGeneres[genereIndex]] = counter;
                            mapperOpsite[counter] = selectedGeneres[genereIndex];

                            counter++;
                        }

                    }

                    int[] mappedLabels = new int[selectedGeneres.Length];

                    for (int i = 0; i < selectedGeneres.Length; i++)
                    {
                        mappedLabels[i] = mapper[selectedGeneres[i]];
                    }


                    var learner = new NaiveBayesLearning();
                    NaiveBayes nb = learner.Learn(dataset, mappedLabels);

                    int[] prediction = new int[] { default(int) };

                    int selectedGenereMapped = nb.Decide(prediction);

                    int selectedIndex = mapperOpsite[selectedGenereMapped];

                    allBooksInGenere = db.Generes.First(x => x.Id == selectedIndex).Books;

                    selectedBook = allBooksInGenere[rnd.Next(allBooksInGenere.Count)];
                }
            }

            return selectedBook;
        }
    }
}