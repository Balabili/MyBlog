using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.IRepository;
using Blog.Domain;
using Blog.Utility;

namespace Blog.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        Logger logger = Logger.GetInstance(typeof(CommentRepository));

        public List<Comment> GetCommentByName(String name)
        {
            return this.context.Comments.Select(c => c).ToList();
        }

        public List<Comment> GetCommentsByNameAndEmail(String name, String email)
        {
            return this.context.Comments.Select(c => c).ToList();
        }

        public List<Comment> GetCommentsByArticleId(Guid id)
        {
            return this.context.Comments.Where(c=>c.ArticleId.Equals(id)).Select(c => c).ToList();
        }
    }
}
