using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }

        public String Content { get; set; }

        public DateTime CreateTime { get; set; }

        public Guid UserId { get; set; }

        public UserModel User { get; set; }

        public Guid ArticleId { get; set; }

    }
}