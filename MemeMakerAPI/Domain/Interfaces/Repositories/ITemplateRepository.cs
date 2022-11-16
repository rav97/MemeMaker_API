using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITemplateRepository : IBaseRepository<Template>
    {
        #region [ SYNCHRONOUS ]

        bool InsertTemplate(string templateName, string relativePath);
        List<Template> GetTemplatesById(IEnumerable<int> idList);
        List<Template> GetTemplatesWithPhrase(string phrase);

        #endregion

        #region [ ASYNCHRONOUS ]

        Task<bool> InsertTemplateAsync(string templateName, string relativePath);
        Task<List<Template>> GetTemplatesByIdAsync(IEnumerable<int> idList);
        Task<List<Template>> GetTemplatesWithPhraseAsync(string phrase);

        #endregion
    }
}
