using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDNVN.BibleOnline.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }

    }
}