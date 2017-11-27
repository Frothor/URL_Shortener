using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortener.Models;

namespace URLShortener
{
    public class Context : DbContext
    {
        public Context(
            DbContextOptions<Context> options)
            : base(options)
        {

        }

        public DbSet<Link> Links { get; set; }
    }
}
