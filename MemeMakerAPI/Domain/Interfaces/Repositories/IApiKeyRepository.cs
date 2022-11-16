using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IApiKeyRepository : IBaseRepository<ApiKey>
    {
        #region [ SYNCHRONOUS ]

        ApiKey FindKey(string key);
        bool InsertKey(DateTime now, string key);

        #endregion

        #region [ ASYNCHRONOUS ]

        Task<ApiKey> FindKeyAsync(string key);
        Task<bool> InsertKeyAsync(DateTime now, string key);

        #endregion
    }
}
