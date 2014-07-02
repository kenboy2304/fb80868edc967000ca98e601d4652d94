using System.Collections.Generic;

namespace CDNVN.BibleOnline.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<Bible> Bibles { get; set; }
    }
}