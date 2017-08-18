using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        List<Comment> GetCommentByName(String name);

        List<Comment> GetCommentsByNameAndEmail(String name,String Email);

        List<Comment> GetCommentsByArticleId(Guid id);

    }
}
