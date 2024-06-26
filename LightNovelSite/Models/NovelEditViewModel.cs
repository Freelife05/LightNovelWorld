﻿namespace LightNovelSite.Models
{
    public class NovelEditViewModel
    {
        public Novel Novel { get; set; }
        public IEnumerable<NamesToLinks> ExistingLinkedWords { get; set; } // Existing linked words for the novel
        public List<string> NewLinkedWords { get; set; }
        public List<string> NewLinks { get; set; }// List of new words to be added (optional)
        public List<int> DeletedLinkedWordIds { get; set; } // List of IDs for linked words to be removed (optional)
    }
}
