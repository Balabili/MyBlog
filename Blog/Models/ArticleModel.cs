using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Domain;

namespace Blog.Models
{
    public class ArticleModel
    {
        public Guid Id { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String Content { get; set; }

        public String Tag { get; set; }

        public Int32 PageView { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public Status Status { get; set; }

        public Status OldStatus { get; set; }

        public Boolean HasCookie { get; set; }

        public List<CommentModel> Comments { get; set; }

        public Int32 CommentsCount
        {
            get
            {
                return this.Comments == null ? 0 : this.Comments.Count();
            }
        }
    }
}