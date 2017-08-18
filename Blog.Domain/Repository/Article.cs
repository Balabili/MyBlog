using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    [Table("Article")]
    public class Article
    {
        [Key]
        public Guid Id { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String Content { get; set; }

        public String Tag { get; set; }

        public Int32 PageView { get; set; }

        public long CreateTime { get; set; }

        public long? ModifyTime { get; set; }

        public Int32 CommentsCount { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public enum Tag
    {
        [Description("HTML")]
        HTML = 0,
        [Description("CSS")]
        CSS = 1,
        [Description("JavaScript")]
        JavaScript = 2,
        [Description("Nodejs")]
        Nodejs = 3,
        [Description("CSharp")]
        CSharp = 4,
        [Description("MVC")]
        MVC = 5
    }

    public enum Status
    {
        Draft = 0,
        Submitted = 1,
        Deleted = 2
    }
}
