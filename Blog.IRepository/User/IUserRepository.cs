using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByNameAndEmail(String name, String email);

        List<User> GetByName(String name);

    }
}
