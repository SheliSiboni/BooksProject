using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksReview.Models
{
    public class Genere : BaseModel
    {
        [Required]
        [DisplayName("Genre Name")]
        public string Name { get; set; }

        public virtual List<Book> Books { get; set; }
    }
}