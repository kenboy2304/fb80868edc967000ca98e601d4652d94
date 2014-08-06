using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDNVN.BibleOnline.Models
{
    public class Gences
    {
        public Gences()
        {
            Init();
        }
        public List<Gence> List { get; set; }

        public void Init()
        {
            List = new List<Gence>();
            //CỰU ƯỚC 5+12+5+5+12
            //5 (1-5)
            List.Add(new Gence
            {
                OrderStart = 1,
                OrderEnd = 5,
                Name = "Ngũ Kinh Môi-se"
            });
            //12 (6-17)
            List.Add(new Gence
            {
                OrderStart = 6,
                OrderEnd = 17,
                Name = "Sách Lịch Sử"
            });
            //5 (18-22)
            List.Add(new Gence
            {
                OrderStart = 18,
                OrderEnd = 22,
                Name = "Sách Thơ Văn"
            });
            //5 (23-27)
            List.Add(new Gence
            {
                OrderStart = 23,
                OrderEnd = 27,
                Name = "Sách Đại Tiên Tri"
            });
            //12(28-39)
            List.Add(new Gence
            {
                OrderStart = 28,
                OrderEnd = 39,
                Name = "Sách Tiểu Tiên Tri"
            });


            //TÂN ƯỚC ƯỚC 4+1+13+8+1
            //4 (40-43)
            List.Add(new Gence
            {
                OrderStart = 40,
                OrderEnd = 43,
                Name = "Tinh Lành Cộng Quang"
            });
            //1 (44-44)
            List.Add(new Gence
            {
                OrderStart = 44,
                OrderEnd = 44,
                Name = "Lịch sử hội thánh"
            });
            //13 (45-57)
            List.Add(new Gence
            {
                OrderStart = 45,
                OrderEnd = 57,
                Name = "Thơ tín của Phao-lô"
            });
            //8 (58-65)
            List.Add(new Gence
            {
                OrderStart = 58,
                OrderEnd = 65,
                Name = "Thơ tín khác"
            });
            //1(66-66)
            List.Add(new Gence
            {
                OrderStart = 66,
                OrderEnd = 66,
                Name = "Khải thị"
            });
            
        }
    }
    public class Gence
    {
        public int OrderStart { get; set; }
        public int OrderEnd { get; set; }
        public string Name { get; set; }
    }
}