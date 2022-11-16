using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITemplateUsageRepository : IBaseRepository<TemplateUsage>
    {
        #region [ SYNCHRONOUS ]

        IEnumerable<int> GetPopularTemplateIDs(int limit);
        bool SaveTemplateUsage(int templateId);

        #endregion

        #region [ ASYNCHRONOUS ]

        Task<IEnumerable<int>> GetPopularTemplateIDsAsync(int limit);
        Task<bool> SaveTemplateUsageAsync(int templateId);

        #endregion
    }
}
