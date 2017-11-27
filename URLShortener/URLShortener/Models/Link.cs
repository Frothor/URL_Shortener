using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Models
{
    public class Link
    {
        public int Id { get; set; }
        public string Long { get; set; }
        public string Short { get; set; }
    }
}
