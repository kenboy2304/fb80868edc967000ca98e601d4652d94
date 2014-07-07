using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CDNVN.BibleOnline.Resolves;

namespace CDNVN.BibleOnline.Models
{
    public class BibleAddress
    {
        public BibleAddress()
        {
            
        }

        public BibleAddress(string str)
        {
            SetAdressFromStringAdress(str);
            SetFromToForAdress();
        }
        public string Full { get { return Book + " " + Adress; } }
        public string Book { get; set; }
        public string Adress { get; set; }
        public VerseAdress From { get; set; }
        public VerseAdress To { get; set; }

        public void SetFromToForAdress()
        {
            const int max = 999;
            var bibleFormat = new[]
            {
                @"\d+",             //0: n
                @"\d+-\d+",         //1: n-n
                @"\d+:\d+",         //2: n:n
                @"\d+:\d+-\d+",     //3: n:n-n
                @"\d+:\d+-+\d+:\d+" //4: n:n-n:n
            };
            var listResult = bibleFormat.Select(f => new Regex(f).IsMatch(Adress)).ToArray();
            for (int i = listResult.Length-1; i >=0 ; i--)
            {
                if (listResult[i])
                {
                    if (i == 4)
                    {
                        var r = new Regex(@"\W").Split(Adress);
                        From = new VerseAdress
                        {
                            Chapter = Int32.Parse(r[0]),
                            Verse = Int32.Parse(r[1])
                        };
                        To = new VerseAdress()
                        {
                            Chapter = Int32.Parse(r[2]),
                            Verse = Int32.Parse(r[3])
                        };
                        break;
                    }
                    if (i == 3)
                    {
                        var r = new Regex(@"\W").Split(Adress);
                        From = new VerseAdress
                        {
                            Chapter = Int32.Parse(r[0]),
                            Verse = Int32.Parse(r[1])
                        };
                        To = new VerseAdress()
                        {
                            Chapter = Int32.Parse(r[0]),
                            Verse = Int32.Parse(r[2])
                        };
                        break;
                    }
                    if (i == 2)
                    {
                        var r = new Regex(@"\W").Split(Adress);
                        From = new VerseAdress
                        {
                            Chapter = Int32.Parse(r[0]),
                            Verse = Int32.Parse(r[1])
                        };
                        To = new VerseAdress()
                        {
                            Chapter = Int32.Parse(r[0]),
                            Verse = Int32.Parse(r[1])
                        };
                        break;
                    }
                    if (i == 1)
                    {
                        var r = new Regex(@"\W").Split(Adress);
                        From = new VerseAdress
                        {
                            Chapter = Int32.Parse(r[0]),
                            Verse = 1
                        };
                        To = new VerseAdress()
                        {
                            Chapter = Int32.Parse(r[1]),
                            Verse = max
                        };
                        break;
                    }
                    if (i == 0)
                    {
                        From = new VerseAdress
                        {
                            Chapter = Int32.Parse(Adress),
                            Verse = 1
                        };
                        To = new VerseAdress()
                        {
                            Chapter = Int32.Parse(Adress),
                            Verse = max
                        };
                        break;
                    }

                    
                }
            }
        }

        private void SetAdressFromStringAdress(string str)
        {
            str = RemoveVietnamese.RemoveSpace(str);
            var book = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(str[i])&&i!=0)
                {
                    break;
                }
                book = book + str[i];
            }
            Book = RemoveVietnamese.RemoveSpace(book);
            Adress = new Regex(@"\s+").Replace(str.Replace(book, ""),"");
        }

    }
    public class VerseAdress
    {
        public string Full { get { return Book + " " + Chapter + ":" + Verse; } }
        public string Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
    }
}