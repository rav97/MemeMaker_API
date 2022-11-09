using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IMemeRepository : IBaseRepository<GeneratedMeme>
    {
        int GetMemeCount();
        bool InsertMeme(int templateId, string relativePath);
        IEnumerable<GeneratedMeme> GetMemeOfTemplate(int id);
        IEnumerable<GeneratedMeme> GetMemes(int skip, int take);
    }
}
