using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace BooksReview.Models
{
    public class Book : BaseModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Genere")]
        [ForeignKey("Genere")]
        public int GenereID { get; set; }
        public virtual Genere Genere { get; set; }

        [Required]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }

        [Required]
        [DisplayName("Author")]
        public string Author { get; set; }

        public virtual List<Review> Reviews { get; set; }
    }
}