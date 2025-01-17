using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        try
        {
            context.Set<T>().Add(entity);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        var query = context.Set<T>().AsQueryable();
        query = spec.ApplyCriteria(query);
        return await query.CountAsync();
    }

    public bool Exists(int id)
    {
        try
        {
            return context.Set<T>().Any(x => x.Id == id);

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            return await context.Set<T>().FindAsync(id);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        try
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        try
        {
            return await context.Set<T>().ToListAsync();
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        try
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Remove(T entity)
    {
        try
        {
            context.Set<T>().Remove(entity);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<bool> SaveAllAsync()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void Update(T entity)
    {
        try
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        try
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specification);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        try
        {
            return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), specification);
        }
        catch (Exception)
        {
            throw;
        }
    }
}