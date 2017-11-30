using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<Link> Links { get; set; }
    }
}
