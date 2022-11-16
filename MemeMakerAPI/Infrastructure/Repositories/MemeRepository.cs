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
    public class MemeRepository : BaseRepository<GeneratedMeme>, IMemeRepository
    {
        public MemeRepository(MemeMakerDBContext context) : base(context)
        {

        }

        #region [ SYNCHRONOUS ]

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

        #endregion

        #region [ ASYNCHRONOUS ]

        public async Task<int> GetMemeCountAsync()
        {
            return await _context.GeneratedMeme.CountAsync();
        }

        public async Task<IEnumerable<GeneratedMeme>> GetMemeOfTemplateAsync(int id)
        {
            var result = await _context.GeneratedMeme
                                       .Where(x => x.template_id == id)
                                       .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<GeneratedMeme>> GetMemesAsync(int skip, int take)
        {
            var result = await _context.GeneratedMeme
                                       .OrderByDescending(m => m.create_date)
                                       .Skip(skip)
                                       .Take(take)
                                       .ToListAsync();

            return result;
        }

        public async Task<bool> InsertMemeAsync(int templateId, string relativePath)
        {
            var meme = new GeneratedMeme
            {
                create_date = DateTime.Now,
                path = relativePath,
                template_id = templateId
            };

            return await InsertAsync(meme);
        }

        #endregion
    }
}
