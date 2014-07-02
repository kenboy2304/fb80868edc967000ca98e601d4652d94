using System.ComponentModel.DataAnnotations;

namespace CDNVN.BibleOnline.Models
{
    public class Content
    {
        public int Id { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Word { get; set; }

        [DataType(DataType.MultilineText)]
        public string Tag { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}