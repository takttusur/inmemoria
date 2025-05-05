using AutoMapper;
using TaktTusur.InMemoria.MigrationApp.OldModels;
using Person = TaktTusur.InMemoria.MigrationApp.OldModels.Person;

namespace TaktTusur.InMemoria.MigrationApp.Mappings;

public class PersonProfile : Profile
{
	public PersonProfile()
	{
		CreateMap<Person, TaktTusur.InMemoria.Domain.Entities.Person>()
		    .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
		    .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
		    .ForMember(dest => dest.MaidenName, opts => opts.MapFrom(src => src.LastName2))
		    .ForMember(dest => dest.Patronymic, opts => opts.MapFrom(src => src.Patronymic))
		    .ForMember(dest => dest.Nickname, opts => opts.MapFrom(src => src.Nickname))
		    .ForMember(dest => dest.Birthday, opts => opts.MapFrom(src => src.Birthday))
		    .ForMember(dest => dest.DeathDay, opts => opts.MapFrom(src => src.DeathDay))
		    .ForMember(dest => dest.Biography, opts => opts.MapFrom(src => src.Biography))
		    .ForMember(dest => dest.Active, opts => opts.MapFrom(src => src.Active == 1 ? true : false))
            .ForMember(dest => dest.Attachments, opts => opts.ConvertUsing(new MediaResourceToAttachmentValueConverter()));
	}
}


public class MediaResourceToAttachmentValueConverter : IValueConverter<List<MediaResource>, List<TaktTusur.InMemoria.Domain.Entities.Attachment>>
{
    public List<TaktTusur.InMemoria.Domain.Entities.Attachment> Convert(List<MediaResource> source, ResolutionContext context)
    {
        return source.Select(m =>
		    {
			    var category = TaktTusur.InMemoria.Domain.Entities.AttachmentCategory.Hikings;
			    if (m.Rubric.ToLower() == "gallery")
				    category = TaktTusur.InMemoria.Domain.Entities.AttachmentCategory.Gallery;
			    else if (m.Rubric.ToLower() == "memoir")
				    category = TaktTusur.InMemoria.Domain.Entities.AttachmentCategory.Memories;

			    var attachmentType = TaktTusur.InMemoria.Domain.Entities.AttachmentTypes.Document;
			    if (m.ResourceType.ToLower() == "img")
				    attachmentType = TaktTusur.InMemoria.Domain.Entities.AttachmentTypes.Image;
			    else if (m.ResourceType.ToLower() == "video")
				    attachmentType = TaktTusur.InMemoria.Domain.Entities.AttachmentTypes.Video;
			    else if (m.ResourceType.ToLower() == "audio")
				    attachmentType = TaktTusur.InMemoria.Domain.Entities.AttachmentTypes.Audio;
			    else if (m.ResourceType.ToLower() == "url")
				    attachmentType = TaktTusur.InMemoria.Domain.Entities.AttachmentTypes.Url;
			    else if (m.ResourceType.ToLower() == "text")
				    attachmentType = TaktTusur.InMemoria.Domain.Entities.AttachmentTypes.Text;
			    else if (m.ResourceType.ToLower() == "html")
				    attachmentType = TaktTusur.InMemoria.Domain.Entities.AttachmentTypes.Html;

                return new TaktTusur.InMemoria.Domain.Entities.Attachment
                {
                    Title = m.Name,
                    Subtitle = m.Comment,
                    Category = category,
                    AttachmentType = attachmentType,
		        Data = m.Data,
		        Order = m.Position,
		        CreatedAt = m.CreateTime,
		        IsActive = m.Active == 1 ? true : false
                };
        }).ToList();
	}
}
