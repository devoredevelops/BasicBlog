using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BasicBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace BasicBlog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
