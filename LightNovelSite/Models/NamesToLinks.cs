using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class NamesToLinks
    {
        [Key]
        public int ID { get; set; }
        public string NovelTitle { get; set; }
        public string Word { get; set; }
        public string Link { get; set; }
        public NamesToLinks(string novelTitle,string word,string link)
        {
            NovelTitle = novelTitle;
            Word = word;
            Link = link;
        }
    }
}
