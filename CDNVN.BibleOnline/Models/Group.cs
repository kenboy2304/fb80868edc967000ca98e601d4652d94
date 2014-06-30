using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDNVN.BibleOnline.Models
{
    public class GroupBook
    {
        public static string[] GetGroupBookForLanguage(string lang)
        {
            return lang == "en" ? new[] { "Old Testament", "New Testament" } : new[] { "Cựu ước", "Tân ước"};
        }
    }
}