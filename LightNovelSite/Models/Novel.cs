using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class Novel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int ChapterCount { get; set; }
        public int CurrentChapter { get; set; }
        [Required]
        public string ImageURL { get; set; }
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}