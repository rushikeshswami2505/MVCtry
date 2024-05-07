using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCtry.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string itemtype { get; set; }
        public int itemsize { get; set; }
        public int itempiece{ get; set; }
        public int itemprice { get; set; }

    }
}