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
    public class ApiKeyRepository : BaseRepository<ApiKey>, IApiKeyRepository
    {
        public ApiKeyRepository(MemeMakerDBContext context) : base(context)
        {

        }

        public ApiKey FindKey(string key)
        {
            var apiKey = _context.ApiKey
                                 .Where(k => k.api_key == key)
                                 .SingleOrDefault();

            return apiKey;
        }
    }
}
