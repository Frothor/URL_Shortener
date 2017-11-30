﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortener.Models;

namespace URLShortener
{
    public class Context : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public Context(
            DbContextOptions<Context> options)
            : base(options)
        {

        }
       
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
