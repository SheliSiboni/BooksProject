using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksReview.Models
{
    public class User : BaseModel
    {
        public Gender Gender { get; set; }
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [DisplayName("Administrator")]
        public bool IsAdmin { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public List<Review> Reviews { get; set; }

        public User()
        {
            Reviews = new List<Review>();
        }
    }
}