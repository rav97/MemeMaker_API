using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TemplateUsageRepository : BaseRepository<TemplateUsage>, ITemplateUsageRepository
    {
        public TemplateUsageRepository(MemeMakerDBContext context) : base(context)
        {

        }

        public IEnumerable<int> GetPopularTemplateIDs(int limit)
        {
            var popular = _context.TemplateUsage
                                  .GroupBy(u => u.template_id)
                                  .Select(t => new { template_id = t.Key, count = t.Count() })
                                  .OrderByDescending(u => u.count)
                                  .Take(limit)
                                  .ToList();

            var popularId = popular.Select(p => p.template_id);

            return popularId;
        }

        public bool SaveTemplateUsage(int templateId)
        {
            var tu = new TemplateUsage
            {
                template_id = templateId,
                date = DateTime.Now
            };

            return Insert(tu);
        }
    }
}
