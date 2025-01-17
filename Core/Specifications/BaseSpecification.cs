using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;
public class BaseSpecification<T>(Expression<Func<T, bool>>? _criteria) : ISpecification<T>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>>? Criteria => _criteria;

    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public bool IsDistinct { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        try
        {
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }
            return query;
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }

    protected void ApplyPaging(int skip, int take)
    {
        try
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
        catch (Exception)
        {

            throw;
        }
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
: BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null) { }

    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}