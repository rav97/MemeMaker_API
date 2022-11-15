using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers.Interfaces
{
    public interface IApiKeyManager
    {
        string GenerateNewApiKey();
        bool CheckApiKey(string key);
    }
}
