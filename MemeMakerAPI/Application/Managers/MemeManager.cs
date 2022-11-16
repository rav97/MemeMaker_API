using Application.DTO;
using Application.Managers.Interfaces;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public class MemeManager : IMemeManager
    {
        private readonly IMemeRepository _memeRepository;
        private readonly IMapper _mapper;
        private readonly string _rootDir = null;
        private readonly Random _rand;

        public MemeManager(IMemeRepository memeRepository,
                           IMapper mapper,
                           IConfiguration config,
                           Random random)
        {
            _memeRepository = memeRepository;
            _mapper = mapper;
            _rand = random;

            _rootDir = config["DiskPaths:MemeRootPath"];

            if (_rootDir == null)
                throw new ConfigurationErrorsException("Reading from TemplateRootPath returned null");
            else
            {
                if (!Directory.Exists(_rootDir))
                    Directory.CreateDirectory(_rootDir);
            }
        }

        #region [ SYNCHRONOUS ]

        public bool AddMeme(int templateId, IFormFile file)
        {
            if (file.Length > 0)
            {
                #region [ SaveFileOnDisk ]

                string filePath = Path.Combine(_rootDir, file.FileName);

                int i = 1;
                while (File.Exists(filePath))
                    filePath = Path.Combine(_rootDir, $"{Path.GetFileNameWithoutExtension(file.FileName)}({i}).{Path.GetExtension(file.FileName)}");

                using (Stream filestream = new FileStream(filePath, FileMode.Create))
                    file.CopyTo(filestream);

                #endregion

                #region [ AddToDatabase ]

                return _memeRepository.InsertMeme(templateId, Path.GetFileName(filePath));

                #endregion
            }

            return false;
        }

        public IEnumerable<MemeDto> GetAllMemes()
        {
            var memes = _memeRepository.GetAll();
            var mapped = _mapper.Map<IEnumerable<MemeDto>>(memes);

            return mapped;
        }

        public IEnumerable<MemeDto> GetMemesFromTemplate(int templateId)
        {
            var memes = _memeRepository.GetMemeOfTemplate(templateId);
            var mapped = _mapper.Map<IEnumerable<MemeDto>>(memes);

            return mapped;
        }

        public MemeDataDto GetRandomMeme()
        {
            var count = _memeRepository.GetMemeCount();

            if (count > 0)
            {
                var skip = _rand.Next(count);
                MemeDto meme = null;

                while (meme == null)
                    meme = GetRecentMemes(skip, 1).FirstOrDefault();

                var result = _memeRepository.GetByKeyId(meme.Id);
                var mapped = _mapper.Map<MemeDataDto>(result);

                return mapped;
            }
            return null;
        }

        public IEnumerable<MemeDto> GetRecentMemes(int skip, int take)
        {
            var memes = _memeRepository.GetMemes(skip, take);
            var mapped = _mapper.Map<IEnumerable<MemeDto>>(memes);

            return mapped;
        }

        public IEnumerable<MemeDataDto> GetRecentMemesData(int skip, int take)
        {
            var memes = _memeRepository.GetMemes(skip, take);
            var mapped = _mapper.Map<IEnumerable<MemeDataDto>>(memes);

            foreach(var m in mapped)
                m.ImageData = GetFileData(m.Path);

            return mapped;
        }

        #endregion

        #region [ ASYNCHRONOUS ]

        public async Task<bool> AddMemeAsync(int templateId, IFormFile file)
        {
            if (file.Length > 0)
            {
                #region [ SaveFileOnDisk ]

                string filePath = Path.Combine(_rootDir, file.FileName);

                int i = 1;
                while (File.Exists(filePath))
                    filePath = Path.Combine(_rootDir, $"{Path.GetFileNameWithoutExtension(file.FileName)}({i}).{Path.GetExtension(file.FileName)}");

                using (Stream filestream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(filestream);

                #endregion

                #region [ AddToDatabase ]

                return await _memeRepository.InsertMemeAsync(templateId, Path.GetFileName(filePath));

                #endregion
            }

            return false;
        }

        public async Task<IEnumerable<MemeDto>> GetAllMemesAsync()
        {
            var memes = await _memeRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<MemeDto>>(memes);

            return mapped;
        }

        public async Task<IEnumerable<MemeDto>> GetMemesFromTemplateAsync(int templateId)
        {
            var memes = await _memeRepository.GetMemeOfTemplateAsync(templateId);
            var mapped = _mapper.Map<IEnumerable<MemeDto>>(memes);

            return mapped;
        }

        public async Task<MemeDataDto> GetRandomMemeAsync()
        {
            var count = await _memeRepository.GetMemeCountAsync();

            if (count > 0)
            {
                var skip = _rand.Next(count);
                MemeDto meme = null;

                while (meme == null)
                {
                    var memeList = await GetRecentMemesAsync(skip, 1);
                    meme = memeList.FirstOrDefault();
                }

                var result = await _memeRepository.GetByKeyIdAsync(meme.Id);
                var mapped = _mapper.Map<MemeDataDto>(result);

                return mapped;
            }
            return null;
        }

        public async Task<IEnumerable<MemeDto>> GetRecentMemesAsync(int skip, int take)
        {
            var memes = await _memeRepository.GetMemesAsync(skip, take);
            var mapped = _mapper.Map<IEnumerable<MemeDto>>(memes);

            return mapped;
        }

        public async Task<IEnumerable<MemeDataDto>> GetRecentMemesDataAsync(int skip, int take)
        {
            var memes = await _memeRepository.GetMemesAsync(skip, take);
            var mapped = _mapper.Map<IEnumerable<MemeDataDto>>(memes);

            await Parallel.ForEachAsync(mapped, new ParallelOptions { MaxDegreeOfParallelism = 2 }, async (m, token) => 
            { 
                m.ImageData = await GetFileDataAsync(m.Path);
            });

            return mapped;
        }

        #endregion

        #region [ PRIVATE ]

        private byte[] GetFileData(string relativePath)
        {
            string filePath = Path.Combine(_rootDir, relativePath);

            if (!File.Exists(filePath))
                return null;

            return File.ReadAllBytes(filePath);
        }

        private async Task<byte[]> GetFileDataAsync(string relativePath)
        {
            string filePath = Path.Combine(_rootDir, relativePath);

            if (!File.Exists(filePath))
                return null;

            return await File.ReadAllBytesAsync(filePath);
        }

        #endregion

    }
}
