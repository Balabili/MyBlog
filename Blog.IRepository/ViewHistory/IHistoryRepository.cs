using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IRepository
{
    public interface IHistoryRepository : IRepository<ViewHistory>
    {
        void DeleteAll();

        ViewHistory GetHistoryByArticleIdAndIP(Guid articleId, String IP);
    }
}
