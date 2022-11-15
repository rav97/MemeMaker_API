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
        ApiKey FindKey(string key);
    }
}
