using Application.DTO;
using Application.Managers.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public class TemplateManager : ITemplateManager
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ITemplateUsageRepository _templateUsageRepository;
        private readonly IMapper _mapper;
        private readonly string _rootDir = null;
         

        public TemplateManager(ITemplateRepository templateRepository, 
                               ITemplateUsageRepository usageRepository,
                               IMapper mapper, 
                               IConfiguration config)
        {
            _templateRepository = templateRepository;
            _templateUsageRepository = usageRepository;
            _mapper = mapper;

            _rootDir = config["DiskPaths:TemplateRootPath"];

            if (_rootDir == null)
                throw new ConfigurationErrorsException("Reading from TemplateRootPath returned null");
        }

        #region [ SYNCHRONOUS ]

        public IEnumerable<TemplateDto> GetAllTemplates()
        {
            var templates = _templateRepository.GetAll();
            var mapped = _mapper.Map<IEnumerable<TemplateDto>>(templates);

            return mapped;
        }

        public IEnumerable<TemplateDto> GetPopularTemplates(int limit)
        {
            var idList = _templateUsageRepository.GetPopularTemplateIDs(limit);
            var templates = _templateRepository.GetTemplatesById(idList);
            var mapped = _mapper.Map<IEnumerable<TemplateDto>>(templates);

            return mapped;
        }

        public IEnumerable<TemplateDto> GetTemplatesContainingPhrase(string phrase)
        {
            var templates = _templateRepository.GetTemplatesWithPhrase(phrase);
            var mapped = _mapper.Map<IEnumerable<TemplateDto>>(templates);

            return mapped;
        }

        public TemplateDataDto GetTemplateById(int id)
        {
            var template = _templateRepository.GetByKeyId(id);
            var mapped = _mapper.Map<TemplateDataDto>(template);
            mapped.ImageData = GetFileData(template.path);

            if (mapped.ImageData == null)
                return null;

            return mapped;
        }

        public bool AddTemplate(string templateName, IFormFile file)
        {
            if(file.Length > 0)
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

                return _templateRepository.InsertTemplate(templateName, Path.GetFileName(filePath));

                #endregion
            }

            return false;
        }

        public bool SaveTemplateUsage(int templateId)
        {
            return _templateUsageRepository.SaveTemplateUsage(templateId);
        }

        #endregion

        #region [ ASYNCHRONOUS ]

        public async Task<IEnumerable<TemplateDto>> GetAllTemplatesAsync()
        {
            var templates = await _templateRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<TemplateDto>>(templates);

            return mapped;
        }

        public async Task<IEnumerable<TemplateDto>> GetPopularTemplatesAsync(int limit)
        {
            var idList = await _templateUsageRepository.GetPopularTemplateIDsAsync(limit);
            var templates = await _templateRepository.GetTemplatesByIdAsync(idList);
            var mapped = _mapper.Map<IEnumerable<TemplateDto>>(templates);

            return mapped;
        }

        public async Task<IEnumerable<TemplateDto>> GetTemplatesContainingPhraseAsync(string phrase)
        {
            var templates = await _templateRepository.GetTemplatesWithPhraseAsync(phrase);
            var mapped = _mapper.Map<IEnumerable<TemplateDto>>(templates);

            return mapped;
        }

        public async Task<TemplateDataDto> GetTemplateByIdAsync(int id)
        {
            var template = await _templateRepository.GetByKeyIdAsync(id);
            var mapped = _mapper.Map<TemplateDataDto>(template);
            mapped.ImageData = await GetFileDataAsync(template.path);

            if (mapped.ImageData == null)
                return null;

            return mapped;
        }

        public async Task<bool> AddTemplateAsync(string templateName, IFormFile file)
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

                return await _templateRepository.InsertTemplateAsync(templateName, Path.GetFileName(filePath));

                #endregion
            }

            return false;
        }

        public async Task<bool> SaveTemplateUsageAsync(int templateId)
        {
            return await _templateUsageRepository.SaveTemplateUsageAsync(templateId);
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
