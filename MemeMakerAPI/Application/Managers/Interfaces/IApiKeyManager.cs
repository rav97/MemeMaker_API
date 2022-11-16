using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers.Interfaces
{
    public interface IApiKeyManager
    {
        #region [ SYNCHRONOUS ]

        string GenerateNewApiKey();
        bool CheckApiKey(string key);

        #endregion

        #region [ ASYNCHRONOUS ]

        Task<string> GenerateNewApiKeyAsync();
        Task<bool> CheckApiKeyAsync(string key);

        #endregion
    }
}
