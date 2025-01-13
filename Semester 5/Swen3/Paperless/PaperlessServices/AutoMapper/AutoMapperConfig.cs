using AutoMapper;
using Contract;
using PaperlessServices.Entities;
using PostgreSQL.Entities;

namespace PaperlessServices.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // Map between BlDocument (business logic entity) and DocumentDto (contract layer DTO)
        CreateMap<BlDocument, DocumentDto>()
            .ForMember(dest => dest.File, opt => opt.Ignore()); // Ignore IFormFile as it's not part of BlDocument

        CreateMap<DocumentDto, BlDocument>()
            .ForMember(dest => dest.FilePath,
                opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FilePath) ? null : src.FilePath))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DateUploaded,
                opt => opt.MapFrom(src => src.DateUploaded == default ? DateTime.UtcNow : src.DateUploaded))
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File))
            .ForMember(dest => dest.OcrText, opt => opt.MapFrom(src => src.OcrText));

        // Map between Document (database entity) and BlDocument (business logic entity)
        CreateMap<Document, BlDocument>()
            .ForMember(dest => dest.File, opt => opt.Ignore()) // Ignore file content as it's not stored in the database
            .ForMember(dest => dest.OcrText, opt => opt.MapFrom(src => src.OcrText ?? string.Empty));

        CreateMap<BlDocument, Document>()
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
            .ForMember(dest => dest.OcrText, opt => opt.MapFrom(src => src.OcrText ?? string.Empty))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
    }
}
