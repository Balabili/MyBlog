using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class HomePageModel
    {

        public List<ArticleModel> Articles { get; set; }

        public Int32 ArticleCount { get; set; }

    }
}