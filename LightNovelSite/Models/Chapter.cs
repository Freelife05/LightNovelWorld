using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        public int NovelId { get; set; }
        public string ChapterTitle { get; set; }
        public string Content { get; set; }
        public int ChapterNumber { get; set; }
    }
}
