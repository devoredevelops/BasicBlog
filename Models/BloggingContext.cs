using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using AJI.Models;

namespace AJIsite.Models
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options) : base(options) 
        { }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<ApplicationUser> Authors { get; set; }
    }
}