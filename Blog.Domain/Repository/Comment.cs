using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public String Content { get; set; }

        public long CreateTime { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Article")]
        public Guid ArticleId { get; set; }

        public virtual Article Article { get; set; }

    }
}
