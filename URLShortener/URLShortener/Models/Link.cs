using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Models
{
    public class Link
    {
        public int Id { get; set; }

        [Required]
        [Url]
        public string Long { get; set; }


        public string Short { get; set; }

        public long NumberOfClicks { get; set; }
    }
}
