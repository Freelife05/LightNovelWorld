using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class NamesToLinks
    {
        [Key]
        public int ID { get; set; }
        public int NovelId { get; set; } // Foreign key for linking to Novel
        public string Word { get; set; }
        public string Link { get; set; }
        public NamesToLinks(string word,string link,int novelId)
        {
            NovelId = novelId;
            Word = word;
            Link = link;
        }
    }
}
