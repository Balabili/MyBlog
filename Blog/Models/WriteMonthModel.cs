using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class WriteMonthModel
    {
        public DateTime WriteDate { get; set; }

        public Int32 ArticleCount { get; set; }

        public String MonthAndCountDisplay
        {
            get
            {
                return $"{this.WriteDate.Year}年{this.WriteDate.Month + 1}月 ({this.ArticleCount})";
            }
        }
    }
}