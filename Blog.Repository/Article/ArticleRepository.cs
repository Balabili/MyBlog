using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Utility;
using Blog.IRepository;

namespace Blog.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        Logger logger = Logger.GetInstance(typeof(ArticleRepository));

        public List<Article> GetArticleByCondition(FilterModel filterModel)
        {
            return this.context.Articles.Select(a => a)
                .Where(a => a.Status == Status.Submitted)
                .OrderByDescending(a => a.CreateTime).ToList();
        }

        public List<Article> GetAllArticles()
        {
            return this.context.Articles.Select(a => a)
                   .Where(a => a.Status == Status.Submitted)
                   .OrderByDescending(a => a.CreateTime).ToList();
        }

        public List<Article> GetArticlesByStatus(Status status)
        {
            return this.context.Articles
                       .Where(a => a.Status == status)
                       .OrderByDescending(a => a.CreateTime).ToList();
        }

        public List<Article> GetArticleByTag(String tag)
        {
            return this.context.Articles.Where(a => a.Tag.Equals(tag)).OrderByDescending(a => a.CreateTime).ToList();
        }

        public List<Article> GetArticleByCreateMonth(String monthAndYear)
        {
            return this.context.Articles
                .Select(a => a)
                .AsEnumerable()
                .Where(a => (new DateTime(a.CreateTime).Month + 1 + "/" + new DateTime(a.CreateTime).Year).Equals(monthAndYear))
                .OrderByDescending(a => a.CreateTime).ToList();
        }

        public Article GetArticleByTitle(String title)
        {
            try
            {
                return this.context.Articles.Select(a => a).Where(a => a.Title.Equals(title)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while GetArticleByTitle", ex);
                return null;
            }
        }

        public void SaveArticles(Article article)
        {
            try
            {
                this.context.Articles.Add(article);
                this.context.SaveChanges();
                this.context.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("An error occured while save articles.", ex);
            }
        }

        public void DeleteArticleById(Guid id)
        {
            var article = this.GetById(id);
            this.Delete(article);
        }

    }
}
