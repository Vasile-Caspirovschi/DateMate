using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using CloudinaryDotNet.Actions;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt
                .MapFrom(src => src.Photos!.FirstOrDefault(photo => photo.IsMain)!.Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt
                .MapFrom(src => src.Sender!.Photos!.FirstOrDefault(photo => photo.IsMain)!.Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt
                .MapFrom(src => src.Recipient!.Photos!.FirstOrDefault(photo => photo.IsMain)!.Url));
            CreateMap<Photo, PhotoForApprovalDto>()
                .ForMember(dest => dest.Username, opt => opt
                .MapFrom(src => src.User.UserName));
        }
    }
}
