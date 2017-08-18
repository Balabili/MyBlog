using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Blog.Domain;
using Blog.Utility;
using Blog.IRepository;
using System.Net;
using Blog.Repository;
using AutoMapper;
using Blog.IService;
using Blog.Service;

namespace Blog.Controllers
{

    public class AdminController : Controller
    {
        Logger logger = Logger.GetInstance(typeof(AdminController));

        public IArticleRepository ArticleRepository;

        public IArticleService ArticleService;

        public AdminController()
        {
            ArticleRepository = new ArticleRepository();
            ArticleService = new ArticleService();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetArticles(Status status)
        {
            List<ArticleModel> articleList = new List<ArticleModel>();
            List<Article> articles = ArticleRepository.GetArticlesByStatus(status);
            try
            {
                foreach (var article in articles)
                {
                    var articleModel = Mapper.Map<Article, ArticleModel>(article);
                    articleList.Add(articleModel);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while GetArticles.", ex);
            }
            return Json(articleList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WriteArticle(Guid? id)
        {
            try
            {
                ArticleModel articleModel = new ArticleModel();
                if (id.HasValue)
                {
                    var atrticle = ArticleRepository.GetById(id.Value);
                    articleModel = Mapper.Map<Article, ArticleModel>(atrticle);
                    return View(articleModel);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while into WriteArticle page.", ex);
                return View();
            }
        }

        public void SubmitArticle(Guid id)
        {
            try
            {
                var article = ArticleRepository.GetById(id);
                article.Status = Status.Submitted;
                ArticleRepository.Update(article);
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while Submit Article.", ex);
            }
        }

        public void DeleteArticle(Guid id, Boolean isDelete)
        {
            try
            {
                if (isDelete)
                {
                    ArticleRepository.DeleteArticleById(id);
                }
                else
                {
                    var article = ArticleRepository.GetById(id);
                    article.Status = Status.Deleted;
                    ArticleRepository.Update(article);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while delete article.", ex);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public void SaveArticle(ArticleModel data)
        {
            try
            {
                if (data.OldStatus == Status.Draft || data.OldStatus == Status.Submitted)
                {
                    var articleItem = ArticleRepository.GetById(data.Id);
                    articleItem.Title = data.Title;
                    articleItem.Description = data.Description;
                    articleItem.Content = data.Content;
                    articleItem.Tag = data.Tag;
                    articleItem.ModifyTime = DateTime.UtcNow.Ticks;
                    articleItem.Status = data.Status == Status.Submitted ? Status.Submitted : Status.Draft;
                    ArticleRepository.Update(articleItem);
                }
                else
                {
                    var article = Mapper.Map<ArticleModel, Article>(data);
                    article.Id = Guid.NewGuid();
                    article.CreateTime = DateTime.UtcNow.Ticks;
                    ArticleRepository.Add(article);
                }
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while save articles.", ex);
            }
        }

        [HttpPost]
        public void ChangeArticleStatus(Guid id, Int32 status)
        {
            try
            {
                var article = ArticleRepository.GetById(id);
                var articleStatus = (Status)status;
                article.Status = articleStatus;
                ArticleRepository.Update(article);
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while ChangeArticleStatus.", ex);
            }
        }

        public JsonResult TitleValidation(String title)
        {
            try
            {
                var article = ArticleRepository.GetArticleByTitle(title);
                if (article == null || (article != null && article.Status == Status.Draft || article.Status == Status.Submitted))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while TitleValidation", ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}