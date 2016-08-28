using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AJI;
using AJI.Data;
using AJI.Models;
using Microsoft.EntityFrameworkCore;

namespace AJI.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPostAttribute]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post) //[FromBodyAttribute]
        {
            // post.Author = _context.Users.Where(u => u.)
            post.ModifiedOn = DateTime.Now;
            post.CreatedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        public IActionResult Edit(Post post)
        {
            return View();
        }

        [HttpPostAttribute]
        public IActionResult Delete(Post post)
        {
            return View();
        }
    }
}