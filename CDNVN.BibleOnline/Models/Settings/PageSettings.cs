using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDNVN.BibleOnline.Models
{
    public class PageSettings
    {
        public PageSettings()
        {
            
        }

        public PageSettings(string title, string keyword, string description)
        {
            
        }
        public string Title { get; set; }
        public string MetaKeyword { get; set; }
        public string MeataDescription { get; set; }
    }
}