using Blog.Domain;
using Blog.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public class HistoryRepository : Repository<ViewHistory>, IHistoryRepository
    {
        public void DeleteAll()
        {
            var histories = this.context.Histories.Select(h => h).ToList();
            foreach (var history in histories)
            {
                this.context.Histories.Remove(history);
            }
        }

        public ViewHistory GetHistoryByArticleIdAndIP(Guid articleId, String IP)
        {
            return this.context.Histories.Select(h => h)
                .Where(h => h.ArticleId.Equals(articleId) && h.ViewerIP.Equals(IP))
                .OrderByDescending(h => h.ViewTime).FirstOrDefault();
        }
    }
}
