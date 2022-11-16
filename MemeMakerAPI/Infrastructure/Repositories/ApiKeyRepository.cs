using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
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

        #region [ SYNCHRONOUS ]

        public ApiKey FindKey(string key)
        {
            var apiKey = _context.ApiKey
                                 .Where(k => k.api_key == key)
                                 .SingleOrDefault();

            return apiKey;
        }

        public bool InsertKey(DateTime now, string key)
        {
            var apiKey = new ApiKey
            {
                create_date = now,
                expire_date = now.AddYears(1),
                active = true,
                api_key = key
            };

            return Insert(apiKey);
        }

        #endregion

        #region [ ASYNCHRONOUS ]

        public async Task<ApiKey> FindKeyAsync(string key)
        {
            var apiKey = await _context.ApiKey
                                       .Where(k => k.api_key == key)
                                       .SingleOrDefaultAsync();

            return apiKey;
        }

        public async Task<bool> InsertKeyAsync(DateTime now, string key)
        {
            var apiKey = new ApiKey
            {
                create_date = now,
                expire_date = now.AddYears(1),
                active = true,
                api_key = key
            };

            return await InsertAsync(apiKey);
        }

        #endregion
    }
}
