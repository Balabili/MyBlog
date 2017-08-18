using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Utility;
using AutoMapper;
using Blog.Domain;
using Blog.Models;
using System.Net;

namespace Blog.AutoMapper
{
    public class CommonMapperProfile : AutoMapperBase
    {
        protected override void Configure()
        {
            this.CreateMap<Article, ArticleModel>()
                .ForMember(d => d.CreateTime, opt => opt.MapFrom(s => ConvertToDatetime(s.CreateTime)))
                .ForMember(d => d.ModifyTime, opt => opt.MapFrom(s => ConvertToDatetime(s.ModifyTime)))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => DecodeHtml(s.Description)))
                .ForMember(d => d.Content, opt => opt.MapFrom(s => DecodeHtml(s.Content)))
                .ForMember(d => d.HasCookie, opt => opt.Ignore())
                .ForMember(d => d.OldStatus, opt => opt.Ignore());

            this.CreateMap<ArticleModel, Article>()
                .ForMember(d => d.CreateTime, opt => opt.MapFrom(s => s.CreateTime.ToUniversalTime().Ticks))
                .ForMember(d => d.ModifyTime, opt => opt.MapFrom(s => s.ModifyTime.HasValue ? s.ModifyTime.Value.ToUniversalTime().Ticks : 0))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => EncodeHtml(s.Description)))
                .ForMember(d => d.Content, opt => opt.MapFrom(s => EncodeHtml(s.Content)));

            this.CreateMap<Comment, CommentModel>()
                .ForMember(d => d.Content, opt => opt.MapFrom(s => DecodeHtml(s.Content)))
                .ForMember(d => d.CreateTime, opt => opt.MapFrom(s => ConvertToDatetime(s.CreateTime)));

            this.CreateMap<CommentModel, Comment>()
                .ForMember(d => d.Content, opt => opt.MapFrom(s => DecodeHtml(s.Content)))
                .ForMember(d => d.CreateTime, opt => opt.MapFrom(s => s.CreateTime.ToUniversalTime().Ticks))
                .ForMember(d => d.Article, opt => opt.Ignore());

            this.CreateMap<UserModel, User>();
            this.CreateMap<User, UserModel>();
        }

        private String EncodeHtml(String html)
        {
            return WebUtility.HtmlEncode(html);
        }

        private String DecodeHtml(String html)
        {
            return WebUtility.HtmlDecode(html);
        }

        private DateTime ConvertToDatetime(long? ticks)
        {
            var date = DateTime.MinValue.ToLocalTime();
            if (ticks.HasValue)
            {
                date = new DateTime(ticks.Value).ToLocalTime();
            }
            return date;
        }

        private String ConvertToDatetimeString(long? ticks)
        {
            var dateStr = "";
            if (ticks.HasValue)
            {
                var date = new DateTime(ticks.Value).ToLocalTime();
                dateStr = FormatTime(date);
            }
            return dateStr;
        }

        private DateTime ConvertToDatetime(long ticks)
        {
            var date = DateTime.MinValue.ToLocalTime();
            date = new DateTime(ticks).ToLocalTime();
            return date;
        }

        public static string FormatTime(DateTime dt)
        {
            return dt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        }
    }
}