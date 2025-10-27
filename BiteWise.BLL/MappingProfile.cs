using AutoMapper;
using BiteWise.BLL.Models;
using BiteWise.DLL.Entities;

namespace BiteWise.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserEntity>();
        CreateMap<UserEntity, User>();

        CreateMap<Article, ArticleEntity>();
        CreateMap<ArticleEntity, Article>();

        CreateMap<TagEntity, Tag>();
        CreateMap<Tag, TagEntity>();

        CreateMap<CommentEntity, Comment>();
        CreateMap<Comment, CommentEntity>();
    }
}
