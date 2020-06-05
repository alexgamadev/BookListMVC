using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Models
{
    public class Book
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string Author { get; set; }
        [Range(1, 5)] public int Rating { get; set; }
    }
}
