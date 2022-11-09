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
    public class MemeRepository : BaseRepository<GeneratedMeme>, IMemeRepository
    {
        public MemeRepository(MemeMakerDBContext context) : base(context)
        {

        }

        public int GetMemeCount()
        {
            return _context.GeneratedMeme.Count();
        }

        public IEnumerable<GeneratedMeme> GetMemeOfTemplate(int id)
        {
            var result = _context.GeneratedMeme
                                 .Where(x => x.template_id == id)
                                 .ToList();

            return result;
        }

        public IEnumerable<GeneratedMeme> GetMemes(int skip, int take)
        {
            var result = _context.GeneratedMeme
                                 .OrderByDescending(m => m.create_date)
                                 .Skip(skip)
                                 .Take(take)
                                 .ToList();

            return result;
        }

        public bool InsertMeme(int templateId, string relativePath)
        {
            var meme = new GeneratedMeme
            {
                create_date = DateTime.Now,
                path = relativePath,
                template_id = templateId
            };

            return Insert(meme);
        }
    }
}
