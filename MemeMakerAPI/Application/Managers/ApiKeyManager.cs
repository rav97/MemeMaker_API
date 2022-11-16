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

        #region [ SYNCHRONOUS ]

        public bool CheckApiKey(string key)
        {
            var baseKey = _apiKeyRepository.FindKey(key);

            return CheckKey(baseKey, key);
        }

        public string GenerateNewApiKey()
        {
            var now = DateTime.Now;
            var key = GenerateKey(now);

            var result = _apiKeyRepository.InsertKey(now, key);

            if (result)
                return key;
            else
                throw new Exception("Error during generation or saving api key");
        }

        #endregion

        #region [ ASYNCHRONOUS ]

        public async Task<bool> CheckApiKeyAsync(string key)
        {
            var baseKey = await _apiKeyRepository.FindKeyAsync(key);

            return CheckKey(baseKey, key);
        }

        public async Task<string> GenerateNewApiKeyAsync()
        {
            var now = DateTime.Now;
            var key = GenerateKey(now);

            var result = await _apiKeyRepository.InsertKeyAsync(now, key);

            if (result)
                return key;
            else
                throw new Exception("Error during generation or saving api key");
        }

        #endregion

        private bool CheckKey(ApiKey baseKey, string key)
        {
            if (baseKey != null)
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

        private string GenerateKey(DateTime now)
        {
            #region [BASE 64 ENCODING]

            var bytes = Encoding.UTF8.GetBytes(now.ToString("yyyy-MM-dd HH:mm:ss"));
            var key = Convert.ToBase64String(bytes);

            #endregion

            return key;
        }
    }
}
