using Blog.Domain;
using Blog.IRepository;
using Blog.IService;
using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class ArticleService : IArticleService
    {
        public IArticleRepository ArticleRepository;

        public ArticleService()
        {
            this.ArticleRepository = new ArticleRepository();
        }

        public List<Article> GetArticlesWithCondition(FilterModel filter)
        {
            var articles = new List<Article>();
            if (!String.IsNullOrEmpty(filter.MainTable))
            {
                if (filter.MainTable.Equals("Statistic"))
                {
                    articles = ArticleRepository.GetArticleByTag(filter.Condition);
                }
                else
                {
                    articles = ArticleRepository.GetArticleByCreateMonth(filter.Condition);
                }
            }
            else
            {
                articles = ArticleRepository.GetAllArticles();
            }
            return articles;
        }


    }
}
