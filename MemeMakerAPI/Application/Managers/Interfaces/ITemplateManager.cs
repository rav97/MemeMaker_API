using Application.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers.Interfaces
{
    public interface ITemplateManager
    {
        #region [ SYNCHRONOUS ]

        IEnumerable<TemplateDto> GetAllTemplates();
        IEnumerable<TemplateDto> GetPopularTemplates(int limit);
        IEnumerable<TemplateDto> GetTemplatesContainingPhrase(string phrase);
        TemplateDataDto GetTemplateById(int id);
        bool AddTemplate(string templateName, IFormFile file);
        bool SaveTemplateUsage(int templateId);

        #endregion

        #region [ ASYNCHRONOUS ]

        Task<IEnumerable<TemplateDto>> GetAllTemplatesAsync();
        Task<IEnumerable<TemplateDto>> GetPopularTemplatesAsync(int limit);
        Task<IEnumerable<TemplateDto>> GetTemplatesContainingPhraseAsync(string phrase);
        Task<TemplateDataDto> GetTemplateByIdAsync(int id);
        Task<bool> AddTemplateAsync(string templateName, IFormFile file);
        Task<bool> SaveTemplateUsageAsync(int templateId);

        #endregion
    }
}
