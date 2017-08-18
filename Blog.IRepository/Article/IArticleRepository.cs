using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.IRepository
{
    public interface IArticleRepository : IRepository<Article>
    {
        Article GetArticleByTitle(String title);

        void DeleteArticleById(Guid id);

        List<Article> GetArticleByTag(String tag);

        List<Article> GetArticleByCreateMonth(String monthAndYear);

        List<Article> GetAllArticles();

        List<Article> GetArticleByCondition(FilterModel filterModel);

        List<Article> GetArticlesByStatus(Status status);
    }
}
