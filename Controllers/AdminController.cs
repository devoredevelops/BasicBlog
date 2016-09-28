using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BasicBlog.Data;
using BasicBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicBlog.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            // find existing post
            Post post = _context.Posts
                        .Include(p => p.Author)
                        .SingleOrDefault(p => p.PostId == id);

            // If current user is not post's author, then return 401
            ApplicationUser currentUser = await GetCurrentUserAsync();
            if (currentUser != post.Author)
            {
                return Unauthorized();
            }

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
            
            // If current user is not post's author, then return 401
            ApplicationUser currentUser = await GetCurrentUserAsync();
            if (currentUser != oldPost.Author)
            {
                return Unauthorized();
            }
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
            // If current user is not post's author, then return 401
            ApplicationUser currentUser = await GetCurrentUserAsync();
            if (currentUser != post.Author)
            {
                return Unauthorized();
            }

            return View(post);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.PostId == id);

            // If current user is not post's author, then return 401
            ApplicationUser currentUser = await GetCurrentUserAsync();
            if (currentUser != post.Author)
            {
                return Unauthorized();
            }
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