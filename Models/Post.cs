using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasicBlog.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [RequiredAttribute]
        public string Title { get; set; }
        public string Body { get; set; }
        public ApplicationUser Author { get; set;}

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modified On")]
        [DataType(DataType.Date)]        
        public DateTime ModifiedOn { get; set; }
    }
}