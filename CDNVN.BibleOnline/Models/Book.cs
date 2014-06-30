using System.Collections.Generic;

namespace CDNVN.BibleOnline.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }
        public int Group { get; set; }
        public int TotalChapter { get; set; }
        public int Order { get; set; }
        public int BibleId { get; set; }
        public virtual Bible Bible { get; set; }
        public virtual List<Content> Contents   { get; set; }
        

    }
}