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
                    .Include(p => p.Author)
                    .OrderBy(p => p.ModifiedOn)
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

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post post = _context.Posts
                        .Include(p => p.Author)
                        .SingleOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPostAttribute]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Edit(int id, [BindAttribute("Title,Body")] Post post) //[FromBodyAttribute]
        {
            Post oldPost = _context.Posts
                            .Include(p => p.Author)
                            .SingleOrDefault(p => p.PostId == id);

            post.Author = oldPost.Author;
            post.CreatedOn = oldPost.CreatedOn;
            post.ModifiedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Posts.Update(post).Context.Update
                    // _context.Update(post);
                    _context.Posts.Update(post);
                    // await _context.SaveChangesAsync();
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (oldPost == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // _context.Entry(post).State = EntityState.Modified;
                // _context.Posts.Update(post);
                // _context.Update(post);
                // _context.SaveChanges();
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