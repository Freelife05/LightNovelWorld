using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class ChapterComments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int ChapterId { get; set; }
        [Required]
        public string Content { get; set; }
        public Chapter Chapter { get; set; }

    }
}
