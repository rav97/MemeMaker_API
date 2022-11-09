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
        bool InsertTemplate(string templateName, string relativePath);
        List<Template> GetTemplatesById(IEnumerable<int> idList);
        List<Template> GetTemplatesWithPhrase(string phrase);
    }
}
