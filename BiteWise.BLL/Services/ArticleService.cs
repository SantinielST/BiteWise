using AutoMapper;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories;
using BiteWise.DLL.TablesСonnections;
using BiteWise.DLL.UoW;

namespace BiteWise.BLL.Services;

public class ArticleService(IUnitOfWork unitOfWork, 
    IMapper mapper, 
    IService<TagArticleConnection> tagArticleConnectionService, 
    IService<Tag> tagService, 
    IService<Comment> commentService) : IService<Article>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IService<TagArticleConnection> _tagArticleConnectionService = tagArticleConnectionService;
    private readonly IService<Tag> _tagService = tagService;
    private readonly IService<Comment> _commentService = commentService;

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
       
        var tagArticleConnections = _tagArticleConnectionService.GetAllAsync().Result;
        var comments = _commentService.GetAllAsync().Result;

        foreach (var tagArticleConnection in tagArticleConnections)
        {
            if (tagArticleConnection.ArticleEntityId == article.Id)
            {
                await _tagArticleConnectionService.DeleteAsync(tagArticleConnection);
            }
        }

        foreach (var comment in comments)
        {
            if (comment.ArticleId == comment.ArticleId)
                await _commentService.DeleteAsync(comment);
        }

        if (repository is not null)
        {
            var articleEntity = await repository.Get(article.Id.ToString());
            _mapper.Map(articleEntity, article);

            await repository.Delete(articleEntity);
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task<Article?> GetAsync(string id)
    {
        var repository = _unitOfWork.GetRepository<ArticleEntity>() as ArticleRepository;

        if (repository is not null)
        {
            var article = _mapper.Map<Article>(await repository.Get(id));

            var tagArticleConnections = _tagArticleConnectionService.GetAllAsync().Result.Where(c => c.ArticleEntityId == article.Id).ToList();
            article.Tags = [];

            var comments = _commentService.GetAllAsync().Result.Where(c => c.ArticleId == article.Id).ToList();
            article.Comments = comments;

            foreach (var tagArticleConnection in tagArticleConnections)
            {
                var tag = await _tagService.GetAsync(tagArticleConnection.TagEntityId.ToString());

                if (tag is not null)
                {
                    article.Tags.Add(tag);
                }
            }

            return article;
        }

        throw new NullReferenceException();
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        var repository = _unitOfWork.GetRepository<ArticleEntity>() as ArticleRepository;

        if (repository is not null)
        {
            var articleList = new List<Article>();

            foreach (var articleEntity in repository.GetAll())
            {
                var article = _mapper.Map<Article>(articleEntity);
                var tagArticleConnections = _tagArticleConnectionService.GetAllAsync().Result.Where(c => c.ArticleEntityId == articleEntity.Id).ToList();
                article.Tags = [];

                foreach (var tagArticleConnection in tagArticleConnections)
                {
                    var tag = await _tagService.GetAsync(tagArticleConnection.TagEntityId.ToString());

                    if (tag is not null)
                    {
                        article.Tags.Add(tag);
                    }
                }

                articleList.Add(article);
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
            var articleEntity = await repository.Get(article.Id.ToString());
            _mapper.Map(article, articleEntity);
            await repository.Update(articleEntity);
            await _unitOfWork.SaveChanges();
        }
    }
}