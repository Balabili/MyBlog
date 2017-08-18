using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IService
{
    public interface IArticleService
    {
        List<Article> GetArticlesWithCondition(FilterModel filter);
    }
}
