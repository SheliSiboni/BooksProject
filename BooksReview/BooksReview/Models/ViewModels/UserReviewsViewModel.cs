using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BooksReview.Models.ViewModels
{
    public class UserReviewsViewModel : BaseViewModel
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Reviews")]
        public IEnumerable<Review> Reviews { get; set; }
    }
}