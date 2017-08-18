using Blog.Domain;
using Blog.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public User GetByNameAndEmail(String name, String email)
        {
            return this.context.Users.Where(u => u.Name.Equals(name) && u.Email.Equals(email)).Select(u => u).FirstOrDefault();
        }

        public List<User> GetByName(String name)
        {
            return this.context.Users.Where(u => u.Name.Equals(name)).Select(u => u).ToList();
        }

    }
}
