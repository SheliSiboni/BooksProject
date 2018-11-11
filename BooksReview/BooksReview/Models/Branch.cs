using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace BooksReview.Models
{
    public class Branch : BaseModel
    {
        [Required]
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("LatCoordinate")]
        public double LatCoordinate { get; set; }

        [Required]
        [DisplayName("LatCoordinate")]
        public double LngCoordinate { get; set; }
    }
}