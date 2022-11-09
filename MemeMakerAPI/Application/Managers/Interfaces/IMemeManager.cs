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
        bool AddMeme(int templateId, IFormFile file);
        MemeDataDto GetRandomMeme();
        IEnumerable<MemeDto> GetAllMemes();
        IEnumerable<MemeDto> GetRecentMemes(int skip, int take);
        IEnumerable<MemeDto> GetMemesFromTemplate(int templateId);
    }
}
