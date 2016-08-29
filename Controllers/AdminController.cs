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
            var result = await _context.Posts
                    .Include(p => p.Author)
                    .OrderByDescending(p => p.ModifiedOn)
                    .ToListAsync();
    
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
            
            // find existing post
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
        public async Task<IActionResult> Edit(int id, [BindAttribute("Title,Body")] Post post)
        {
            // Look up existing post
            Post oldPost = _context.Posts
                            .Include(p => p.Author)
                            .SingleOrDefault(p => p.PostId == id);
            
            // Take data from Form and apply to existing post
            oldPost.Title = post.Title;
            oldPost.Body = post.Body;
            oldPost.ModifiedOn = DateTime.Now;

            // Update post
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Posts.Update(oldPost);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction("Index");
            }

            return View(post);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(p => p.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.PostId == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}