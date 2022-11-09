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
        IEnumerable<TemplateDto> GetAllTemplates();
        IEnumerable<TemplateDto> GetPopularTemplates(int limit);
        IEnumerable<TemplateDto> GetTemplatesContainingPhrase(string phrase);
        TemplateDataDto GetTemplateById(int id);
        bool AddTemplate(string templateName, IFormFile file);
        bool SaveTemplateUsage(int templateId);
    }
}
