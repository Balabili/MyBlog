using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

    }
}