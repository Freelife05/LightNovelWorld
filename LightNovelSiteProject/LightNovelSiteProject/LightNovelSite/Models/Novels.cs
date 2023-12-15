using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class Novels
    {
        [Key]
        public string Title { get; set; }
        public int Chapters { get; set; }
        public int CurrentChapter { get; set; }

    }
}