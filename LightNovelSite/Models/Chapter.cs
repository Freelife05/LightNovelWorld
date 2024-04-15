using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        public string ChapterTitle { get; set; }
        public string Content { get; set; }
        public string OriginalContent { get; set; }
        public int ChapterNumber { get; set; }
        public int NovelId { get; set; }
        public Novel? Novel { get; set; }
        public List<ChapterComments> Comments { get; set; } = new();
    }
}
