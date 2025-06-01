using AutoMapper;
using ReStudyAPI.Entities;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Utility.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<AddUserDto, User>();
            CreateMap<EditUserDto, User>();

            CreateMap<Subject, AddSubjectDto>().ReverseMap();
            CreateMap<Subject, EditSubjectDto>().ReverseMap();
            CreateMap<Subject, SubjectDto>().ReverseMap();

            CreateMap<Category, AddCategoryDto>().ReverseMap();
            CreateMap<Category, EditCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Concept, AddConceptDto>().ReverseMap();
            CreateMap<Concept, EditConceptDto>().ReverseMap();
            CreateMap<Concept, ConceptDto>().ReverseMap();
        }
    }
}
