using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCtry.Models
{
    public class ProductModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string itemtype { get; set; }

        [Required]
        public int itemsize { get; set; }

        [Required]
        public int itempiece { get; set; }

        [Required]
        public int itemprice { get; set; }

    }
}