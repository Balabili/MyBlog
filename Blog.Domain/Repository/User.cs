using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

    }
}
