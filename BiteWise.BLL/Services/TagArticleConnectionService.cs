using AutoMapper;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL.Repositories;
using BiteWise.DLL.TablesСonnections;
using BiteWise.DLL.UoW;

namespace BiteWise.BLL.Services;

public class TagArticleConnectionService(IUnitOfWork unitOfWork) : IService<TagArticleConnection>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task CreateAsyncTagArticleConnections(List<string> tagIds, Article article)
    {
        var repository = _unitOfWork.GetRepository<TagArticleConnection>() as TagArticleConnectionRepository;

        if (repository is not null)
        {
            foreach (var id in tagIds)
            {
                var tagArticleConnection = new TagArticleConnection()
                {
                    ArticleEntityId = article.Id,
                    TagEntityId = Guid.Parse(id)
                };

                await repository.Create(tagArticleConnection);
            }
        }
    }

    public async Task CreateAsyncTagArticleConnection(string id, Article article)
    {
        var repository = _unitOfWork.GetRepository<TagArticleConnection>() as TagArticleConnectionRepository;

        if (repository is not null)
        {
                var tagArticleConnection = new TagArticleConnection()
                {
                    ArticleEntityId = article.Id,
                    TagEntityId = Guid.Parse(id)
                };

                await repository.Create(tagArticleConnection);
        }
    }

    public async Task<IEnumerable<TagArticleConnection>> GetAllAsync()
    {
        var repository = _unitOfWork.GetRepository<TagArticleConnection>() as TagArticleConnectionRepository;

        if (repository is not null)
        {
            return repository.GetAll();
        }

        throw new ArgumentNullException();
    }

    public Task<TagArticleConnection?> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TagArticleConnection model)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(TagArticleConnection connection)
    {
        var repository = _unitOfWork.GetRepository<TagArticleConnection>() as TagArticleConnectionRepository;

        if (repository is not null)
        {
            await repository.Delete(connection);
        }
    }
}