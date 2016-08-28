using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AJI.Models;
using AJI.Data;

namespace AJI.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        //private BloggingContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // var result = _context.Posts
            //         .Include(post => post.Author)
            //         .OrderBy(post => post.ModifiedOn)
            //         .ToList();
    
            // if (result.Any())
            // {
            //     return View(result);
            // }
            // else
            // {
                return View();
            // }
        }
    }
}