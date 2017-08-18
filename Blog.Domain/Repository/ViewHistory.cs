using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    [Table("History")]
    public class ViewHistory
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }

        public String ViewerIP { get; set; }

        public long ViewTime { get; set; }
    }
}
