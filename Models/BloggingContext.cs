using System;
using Microsoft.EntityFrameworkCore;
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