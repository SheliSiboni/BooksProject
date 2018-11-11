﻿using System.Collections.Generic;
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
        public string Id { get; set; }

        [Required]
        [DisplayName("Location")]
        public string Location { get; set; }

    }
}