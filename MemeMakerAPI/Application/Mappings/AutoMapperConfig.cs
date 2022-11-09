using Application.DTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        => new MapperConfiguration(cfg =>
        {
            #region [ Additional rules ]

            cfg.RecognizeDestinationPostfixes(new[] { "_" });
            cfg.RecognizeDestinationPrefixes(new[] { "_" });
            cfg.RecognizePostfixes(new[] { "_" });
            cfg.RecognizePrefixes(new[] { "_" });

            cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();

            #endregion

            #region [ Template ]

            cfg.CreateMap<Template, TemplateDto>();
            cfg.CreateMap<Template, TemplateDataDto>();

            #endregion

            #region [ Meme ]

            cfg.CreateMap<GeneratedMeme, MemeDto>();
            cfg.CreateMap<GeneratedMeme, MemeDataDto>();

            #endregion

        }).CreateMapper();
    }
}
