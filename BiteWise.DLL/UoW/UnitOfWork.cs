using BiteWise.DLL.Repositories.Base;
using BiteWise.DLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BiteWise.DLL.UoW;

public class UnitOfWork(BiteWiseAppContext biteWiseAppContext) : IUnitOfWork
{
    private readonly BiteWiseAppContext _biteWiseAppContext = biteWiseAppContext;
    private Dictionary<Type, object>? _repositories;

    public void Dispose()
    {

    }

    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class
    {
        if (_repositories == null)
        {
            _repositories = new Dictionary<Type, object>();
        }

        if (hasCustomRepository)
        {
            var customRepo = _biteWiseAppContext.GetService<IRepository<TEntity>>();
            if (customRepo != null)
            {
                return customRepo;
            }
        }

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<TEntity>(_biteWiseAppContext);
        }

        return (IRepository<TEntity>)_repositories[type];

    }
    public async Task SaveChanges(bool ensureAutoHistory = false)
    {
        await _biteWiseAppContext.SaveChangesAsync();
    }
}