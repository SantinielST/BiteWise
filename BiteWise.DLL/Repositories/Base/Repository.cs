using BiteWise.DLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace BiteWise.DLL.Repositories.Base;

public class Repository<T> : IRepository<T> where T : class
{
    protected DbContext _db;

    public DbSet<T> Set
    {
        get;
        private set;
    }

    public Repository(BiteWiseAppContext db)
    {
        _db = db;
        var set = _db.Set<T>();
        set.Load();

        Set = set;
    }

    public async Task Create(T item)
    {
        await Set.AddAsync(item);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(T item)
    {
        Set.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<T> Get(string id)
    {
        return await Set.FindAsync(Guid.Parse(id))?? throw new SqlNullValueException();
    }

    public IQueryable<T> GetAll()
    {
        return Set;
    }

    public async Task Update(T item)
    {
        Set.Update(item);
        await _db.SaveChangesAsync();
    }
}