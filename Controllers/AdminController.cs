using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using AJI;
using AJI.Data;
using AJI.Models;
using AJI.Models.AccountViewModels;
using AJI.Services;
using Microsoft.EntityFrameworkCore;

namespace AJI.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var result = _context.Posts
                    .Include(post => post.Author)
                    .OrderBy(post => post.ModifiedOn)
                    .ToList();
    
            if (!result.Any())
            {
                return View();
            }
            
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPostAttribute]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Create(Post post) //[FromBodyAttribute]
        {
            post.Author = await GetCurrentUserAsync();
            post.CreatedOn = DateTime.Now;
            post.ModifiedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        public IActionResult Edit(int postId)
        {
            Post post = _context.Posts
                        .Where(p => p.PostId == postId)
                        .Single();

            return View(post);
        }

        [HttpPostAttribute]
        [ValidateAntiForgeryTokenAttribute]
        public IActionResult Edit(Post post) //[FromBodyAttribute]
        {
            post.ModifiedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Posts.Update(post);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        [HttpPostAttribute]
        public IActionResult Delete(Post post)
        {
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}