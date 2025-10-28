using AutoMapper;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories;
using BiteWise.DLL.TablesСonnections;
using BiteWise.DLL.UoW;

namespace BiteWise.BLL.Services;

public class TagService(IUnitOfWork unitOfWork, IMapper mapper, IService<TagArticleConnection> tagArticleConnectionService) : IService<Tag>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IService<TagArticleConnection> _tagArticleConnectionService = tagArticleConnectionService;

    public async Task CreateAsync(Tag tag)
    {
        var repository = _unitOfWork.GetRepository<TagEntity>() as TagRepository;
        var tagEntity = _mapper.Map<TagEntity>(tag);

        if (repository is not null)
        {
            await repository.Create(tagEntity);
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task DeleteAsync(Tag tag)
    {
        var repository = _unitOfWork.GetRepository<TagEntity>() as TagRepository;
        var tagEntity = _mapper.Map<TagEntity>(tag);

        if (repository is not null)
        {
            await repository.Delete(tagEntity);
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task<Tag?> GetAsync(string id)
    {
        var repository = _unitOfWork.GetRepository<TagEntity>() as TagRepository;

        if (repository is not null)
        {
            return _mapper.Map<Tag>(await repository.Get(id));
        }

        throw new NullReferenceException();
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        var repository = _unitOfWork.GetRepository<TagEntity>() as TagRepository;
        var tagArticleConnections = _tagArticleConnectionService.GetAllAsync().Result;

        if (repository is not null)
        {
            var tagList = new List<Tag>();

            foreach (var tagEntity in repository.GetAll())
            {
                var tag = _mapper.Map<Tag>(tagEntity);
                tag.CountArticles = tagArticleConnections.Where(c => c.TagEntityId == tag.Id).Count();
                tagList.Add(tag);
            }

            return tagList;
        }

        throw new NullReferenceException();
    }

    public async Task UpdateAsync(Tag tag)
    {
        var repository = _unitOfWork.GetRepository<TagEntity>() as TagRepository;

        if (repository is not null)
        {
            await repository.Update(_mapper.Map<TagEntity>(tag));
            await _unitOfWork.SaveChanges();
        }
    }
}