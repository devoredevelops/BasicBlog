using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AJI.Data;
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
