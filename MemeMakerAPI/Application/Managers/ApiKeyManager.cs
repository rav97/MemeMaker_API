using Application.Managers.Interfaces;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public class ApiKeyManager : IApiKeyManager
    {
        private readonly IApiKeyRepository _apiKeyRepository;

        public ApiKeyManager(IApiKeyRepository repository)
        {
            _apiKeyRepository = repository;
        }

        public bool CheckApiKey(string key)
        {
            var baseKey = _apiKeyRepository.FindKey(key);

            if(baseKey != null)
            {
                if (baseKey.expire_date < DateTime.Now || baseKey.active == false)
                    return false;

                #region [BASE 64 DECODING]

                var bytes = Convert.FromBase64String(key);
                var keyData = Encoding.UTF8.GetString(bytes);

                #endregion

                var dateFromKey = DateTime.ParseExact(keyData, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                if ((baseKey.create_date - dateFromKey).Seconds == 0)
                    return true;
                
            }

            return false;
        }

        public string GenerateNewApiKey()
        {
            var now = DateTime.Now;

            #region [BASE 64 ENCODING]

            var bytes = Encoding.UTF8.GetBytes(now.ToString("yyyy-MM-dd HH:mm:ss"));
            var key = Convert.ToBase64String(bytes);

            #endregion

            var apiKey = new ApiKey
            {
                create_date = now,
                expire_date = now.AddYears(1),
                active = true,
                api_key = key
            };

            var result = _apiKeyRepository.Insert(apiKey);

            if (result)
                return key;
            else
                throw new Exception("Error during generation or saving api key");
        }
    }
}
