using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class LayoutComponentModel
    {
        public List<StatisticModel> Statistics { get; set; } = new List<StatisticModel>();

        public List<WriteMonthModel> WriteMonths { get; set; } = new List<WriteMonthModel>();
    }
}