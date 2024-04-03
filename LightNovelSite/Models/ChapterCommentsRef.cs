using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class ChapterCommentsRef
    {
        [Required]
        public int ChapterId { get; set; }
        public IEnumerable<ChapterComments> Comments { get; set; }
        [Required]
        public string AddComment { get; set; }

        public int NovelId { get; set; }

    }
}
