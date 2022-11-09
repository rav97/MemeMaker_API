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
    public class TemplateRepository : BaseRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(MemeMakerDBContext context) : base(context)
        {

        }

        public List<Template> GetTemplatesById(IEnumerable<int> idList)
        {
            var result = _context.Template
                                 .Where(t => idList.Contains(t.id))
                                 .ToList();

            return result;
        }

        public List<Template> GetTemplatesWithPhrase(string phrase)
        {
            var result = _context.Template
                                 .Where(t => t.name.ToLower().Contains(phrase) || t.path.ToLower().Contains(phrase))
                                 .ToList();

            return result;

        }

        public bool InsertTemplate(string templateName, string relativePath)
        {
            var template = new Template
                                {
                                    create_date = DateTime.Now,
                                    name = templateName,
                                    path = relativePath
                                };

            return Insert(template);
        }
    }
}
