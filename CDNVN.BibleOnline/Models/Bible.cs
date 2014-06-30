using System.Collections.Generic;

namespace CDNVN.BibleOnline.Models
{
    public class Bible
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}