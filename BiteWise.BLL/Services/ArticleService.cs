using AutoMapper;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories;
using BiteWise.DLL.UoW;

namespace BiteWise.BLL.Services;

public class ArticleService(IUnitOfWork unitOfWork, IMapper mapper) : IService<Article>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task CreateAsync(Article article)
    {
        var repository = _unitOfWork.GetRepository<ArticleEntity>() as ArticleRepository;
        var articleEntity = _mapper.Map<ArticleEntity>(article);

        if (repository is not null)
        {
            await repository.Create(articleEntity);
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task DeleteAsync(Article article)
    {
        var repository = _unitOfWork.GetRepository<ArticleEntity>() as ArticleRepository;
        var articleEntity = _mapper.Map<ArticleEntity>(article);

        if (repository is not null)
        {
            await repository.Delete(articleEntity);
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task<Article?> GetAsync(string id)
    {
        var repository = _unitOfWork.GetRepository<ArticleEntity>() as ArticleRepository;

        if (repository is not null)
        {
            return _mapper.Map<Article>(await repository.Get(id));
        }

        throw new NullReferenceException();
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        var repository = _unitOfWork.GetRepository<ArticleEntity>() as ArticleRepository;

        if (repository is not null)
        {
            var articleList = new List<Article>();

            foreach (var article in repository.GetAll())
            {
                articleList.Add(_mapper.Map<Article>(article));
            }

            return articleList;
        }

        throw new NullReferenceException();
    }

    public async Task UpdateAsync(Article article)
    {
        var repository = _unitOfWork.GetRepository<ArticleEntity>() as ArticleRepository;

        if (repository is not null)
        {
            await repository.Update(_mapper.Map<ArticleEntity>(article));
            await _unitOfWork.SaveChanges();
        }
    }
}