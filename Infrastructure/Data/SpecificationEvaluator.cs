using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;
public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
    {
        try
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria); // e.g. x => x.Brand == brand
            }
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsDistinct)
            {
                query = query.Distinct();
            }
            return query;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification)
    {
        try
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria); // e.g. x => x.Brand == brand
            }
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            var selectQuery = query as IQueryable<TResult>;
            if (specification.Select != null)
            {
                selectQuery = query.Select(specification.Select);
            }
            if (specification.IsDistinct)
            {
                selectQuery = selectQuery?.Distinct();
            }
            return selectQuery ?? query.Cast<TResult>();
        }
        catch (Exception)
        {
            throw;
        }
    }

}