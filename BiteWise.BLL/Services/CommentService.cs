using AutoMapper;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories;
using BiteWise.DLL.UoW;

namespace BiteWise.BLL.Services;

public class CommentService(IUnitOfWork unitOfWork, IMapper mapper) : IService<Comment>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAsync(Comment comment)
    {
        var repository = _unitOfWork.GetRepository<CommentEntity>() as CommentRepository;
        var commentEntity = _mapper.Map<CommentEntity>(comment);

        if (repository is not null)
        {
            await repository.Create(commentEntity);
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task DeleteAsync(Comment comment)
    {
        var repository = _unitOfWork.GetRepository<CommentEntity>() as CommentRepository;
        var commentEntity = _mapper.Map<CommentEntity>(comment);

        if (repository is not null)
        {
            await repository.Delete(commentEntity);
            await _unitOfWork.SaveChanges();
        }
    }

    public async Task<Comment?> GetAsync(string id)
    {
        var repository = _unitOfWork.GetRepository<CommentEntity>() as CommentRepository;

        if (repository is not null)
        {
            return _mapper.Map<Comment>(await repository.Get(id));
        }

        throw new NullReferenceException();
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        var repository = _unitOfWork.GetRepository<CommentEntity>() as CommentRepository;

        if (repository is not null)
        {
            var commentleList = new List<Comment>();

            foreach (var article in repository.GetAll())
            {
                commentleList.Add(_mapper.Map<Comment>(article));
            }

            return commentleList;
        }

        throw new NullReferenceException();
    }

    public async Task UpdateAsync(Comment comment)
    {
        var repository = _unitOfWork.GetRepository<CommentEntity>() as CommentRepository;

        if (repository is not null)
        {
            await repository.Update(_mapper.Map<CommentEntity>(comment));
            await _unitOfWork.SaveChanges();
        }
    }
}