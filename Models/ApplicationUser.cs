using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasicBlog.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // [Key]
        // public int ID { get; set; }
        // [Required]
        // public string UserName { get; set; }
        // [Required]
        // public string FullName { get; set;}
        // [Required]
        // [EmailAddressAttribute]
        // [Display(Name = "Email Address")]
        // public string Email { get; set;}
        // [Required]
        // [DataType(DataType.Password)]
        // public string Password { get; set; }
    }
}
