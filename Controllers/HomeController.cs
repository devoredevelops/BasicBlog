using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AJI;
using AJI.Data;
using AJI.Models;
using Microsoft.EntityFrameworkCore;

namespace AJI.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }


    }
}
