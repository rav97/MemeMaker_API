using Application.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers.Interfaces
{
    public interface IMemeManager
    {
        #region [ SYNCHRONOUS ]

        bool AddMeme(int templateId, IFormFile file);
        MemeDataDto GetRandomMeme();
        IEnumerable<MemeDto> GetAllMemes();
        IEnumerable<MemeDto> GetRecentMemes(int skip, int take);
        IEnumerable<MemeDataDto> GetRecentMemesData(int skip, int take);
        IEnumerable<MemeDto> GetMemesFromTemplate(int templateId);

        #endregion

        #region [ ASYNCHRONOUS ]

        Task<bool> AddMemeAsync(int templateId, IFormFile file);
        Task<MemeDataDto> GetRandomMemeAsync();
        Task<IEnumerable<MemeDto>> GetAllMemesAsync();
        Task<IEnumerable<MemeDto>> GetRecentMemesAsync(int skip, int take);
        Task<IEnumerable<MemeDataDto>> GetRecentMemesDataAsync(int skip, int take);
        Task<IEnumerable<MemeDto>> GetMemesFromTemplateAsync(int templateId);

        #endregion
    }
}
