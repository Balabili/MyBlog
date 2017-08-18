using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Utility;
using Blog.Models;
using Blog.Domain;
using Blog.IRepository;
using Blog.Repository;
using Blog.Common;
using AutoMapper;
using System.Net;
using System.Web.Security;
using Blog.IService;
using Blog.Service;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        Logger logger = Logger.GetInstance(typeof(HomeController));

        public IArticleRepository ArticleRepository;

        public ICommentRepository CommentRepository;

        public IUserRepository UserRepository;

        public IHistoryRepository HistoryRepository;

        public IArticleService ArticleService;

        public CommonWebService CommonWebService;

        public HomeController()
        {
            ArticleRepository = new ArticleRepository();
            CommentRepository = new CommentRepository();
            UserRepository = new UserRepository();
            HistoryRepository = new HistoryRepository();
            ArticleService = new ArticleService();
            CommonWebService = new CommonWebService();
        }
        // GET: Default
        public ActionResult Index(String maintable, String condition)
        {
            HomePageModel viewModel = new HomePageModel();
            viewModel.Articles = new List<ArticleModel>();
            try
            {
                var articles = new List<Article>();
                if (String.IsNullOrEmpty(maintable))
                {
                    articles = ArticleRepository.GetAllArticles();
                }
                else
                {
                    FilterModel filterModel = new FilterModel();
                    filterModel.MainTable = maintable;
                    filterModel.Condition = condition;
                    articles = ArticleService.GetArticlesWithCondition(filterModel);
                }
                viewModel.ArticleCount = articles.Count();
                var articlesModels = articles.Take(10);
                foreach (var articleItem in articlesModels)
                {
                    ArticleModel article = Mapper.Map<Article, ArticleModel>(articleItem);
                    viewModel.Articles.Add(article);
                }
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while input home index page", ex);
            }
            return View(viewModel);
        }

        public ActionResult ArticleDetails(String title)
        {
            ArticleModel articleItem = new ArticleModel();
            try
            {
                var article = ArticleRepository.GetArticleByTitle(title);
                AddArticlePageView(article);
                articleItem = Mapper.Map<Article, ArticleModel>(article);
                articleItem.Comments = articleItem.Comments.OrderBy(c => c.CreateTime).ToList();
                var userCookie = System.Web.HttpContext.Current.Request.Cookies["viewerCookie"];
                if (userCookie != null)
                {
                    articleItem.HasCookie = true;
                    var userCookieValue = FormsAuthentication.Decrypt(userCookie.Value);
                    var cookieData = userCookieValue.UserData.Split('#');
                    var user = new UserModel();
                    user.Name = cookieData[0];
                    user.Email = cookieData[1];
                    ViewData["Viewer"] = user;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while view article detail.", ex);
            }
            return View(articleItem);
        }

        private void AddArticlePageView(Article article)
        {
            var currRequest = System.Web.HttpContext.Current.Request;
            var viewerIP = IpHelper.GetClientIpAddress(currRequest);
            var historyRecord = HistoryRepository.GetHistoryByArticleIdAndIP(article.Id, viewerIP);
            if (historyRecord == null)
            {
                var viewHistory = new ViewHistory();
                viewHistory.Id = Guid.NewGuid();
                viewHistory.ViewTime = DateTime.UtcNow.Ticks;
                viewHistory.ViewerIP = viewerIP;
                viewHistory.ArticleId = article.Id;
                HistoryRepository.Add(viewHistory);
                article.PageView++;
                ArticleRepository.Update(article);
            }
            else
            {
                var today = new DateTime(historyRecord.ViewTime).ToLocalTime().Date;
                if (DateTime.Now.ToLocalTime().Date.CompareTo(today) != 0)
                {
                    historyRecord.ViewTime = DateTime.UtcNow.Ticks;
                    HistoryRepository.Update(historyRecord);
                    article.PageView++;
                    ArticleRepository.Update(article);
                }
            }
        }

        public ActionResult GetArticlesWithCondition(FilterModel filter)
        {
            try
            {
                var articleModels = new List<ArticleModel>();
                var articles = ArticleService.GetArticlesWithCondition(filter);
                foreach (var article in articles)
                {
                    var articleModel = Mapper.Map<Article, ArticleModel>(article);
                    articleModels.Add(articleModel);
                }
                articleModels = articleModels.Skip((filter.Pager.Index - 1) * filter.Pager.Size).Take(filter.Pager.Size).ToList();
                return Json(articleModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while Get Articles With Condition.", ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetLayoutData()
        {
            LayoutComponentModel layoutComponent = new LayoutComponentModel();
            try
            {
                var articles = ArticleRepository.GetAllArticles();
                GetStatistic(layoutComponent, articles);
                GetWriteMonth(layoutComponent, articles);
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while input home index page", ex);
            }
            return Json(layoutComponent, JsonRequestBehavior.AllowGet);
        }

        private void GetStatistic(LayoutComponentModel layoutComponent, List<Article> articles)
        {
            foreach (var label in Enum.GetValues(typeof(Tag)))
            {
                StatisticModel statistic = new StatisticModel();
                statistic.Statistic = label.ToString() == "CSharp" ? "C#" : label.ToString();
                statistic.Count = articles.Where(a => a.Tag.Equals(label.ToString())).Count();
                layoutComponent.Statistics.Add(statistic);
            }
        }

        public void GetWriteMonth(LayoutComponentModel layoutComponent, List<Article> articles)
        {
            var dateTime = DateTime.Now.ToLocalTime();
            for (int i = 0; ; i++)
            {
                if (dateTime.Year == 2016)
                {
                    break;
                }
                WriteMonthModel writeMonth = new WriteMonthModel();
                writeMonth.WriteDate = dateTime;
                writeMonth.ArticleCount = articles.Where(a => new DateTime(a.CreateTime).Year == writeMonth.WriteDate.Year && new DateTime(a.CreateTime).Month == writeMonth.WriteDate.Month).Count();
                layoutComponent.WriteMonths.Add(writeMonth);
                dateTime = DateTime.Parse(dateTime.ToString("yyyy-MM-01")).AddMonths(-1);
            }
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel commentModel, UserModel userModel, Boolean rememberMe)
        {
            try
            {

                var userId = new Guid();
                var userEmail = "";
                var userName = "";
                var userCookie = System.Web.HttpContext.Current.Request.Cookies["viewerCookie"];
                if (userCookie != null)
                {
                    var userCookieValue = FormsAuthentication.Decrypt(userCookie.Value);
                    var cookieData = userCookieValue.UserData.Split('#');
                    userName = cookieData[0];
                    userEmail = cookieData[1];
                    User rememberUser = UserRepository.GetByNameAndEmail(userName, userEmail);
                    userId = rememberUser.Id;
                }
                else
                {
                    User userItem = UserRepository.GetByNameAndEmail(userModel.Name, userModel.Email);
                    if (userItem == null)
                    {
                        var user = new User();
                        user.Id = Guid.NewGuid();
                        userId = user.Id;
                        user.Name = userModel.Name;
                        user.Email = userModel.Email;
                        userEmail = userModel.Email;
                        userName = userModel.Name;
                        UserRepository.Add(user);
                    }
                    else
                    {
                        userId = userItem.Id;
                        userEmail = userItem.Email;
                        userName = userItem.Name;
                    }
                    if (rememberMe)
                    {
                        String userData = userModel.Name + "#" + userModel.Email;
                        CommonWebService.AddRememberMeCookie(userModel.Name, userData);
                    }
                }
                Comment comment = Mapper.Map<CommentModel, Comment>(commentModel);
                comment.Id = Guid.NewGuid();
                comment.UserId = userId;
                comment.CreateTime = DateTime.UtcNow.Ticks;
                CommentRepository.Add(comment);
                comment.User = UserRepository.GetById(userId);
                CommentModel commentItem = Mapper.Map<Comment, CommentModel>(comment);
                EmailHelper.SendEmail("", userEmail, $"{userName} reply", commentItem.Content);
                return Json(commentItem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while Add Comment.", ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public void RemoveCookie()
        {
            try
            {
                var userCookie = System.Web.HttpContext.Current.Response.Cookies["viewerCookie"];
                if (userCookie != null)
                {
                    userCookie.Expires = DateTime.Now.AddDays(-1);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred while remove cookie.", ex);
            }
        }

        public void Search()
        {
            //EmailHelper.SendEmail("", "", "", "");
        }
    }
}