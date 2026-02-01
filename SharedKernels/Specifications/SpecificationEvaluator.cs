using System.Linq;
using SharedKernels.Domain;
using Microsoft.EntityFrameworkCore;

namespace SharedKernels.Specifications
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            
            if (specification.IgnoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (specification.Includes != null)
            {
                query = specification.Includes.Aggregate(query,
                    (current, include) => current.Include(include));
            }

            if (specification.IncludeStrings != null)
            {
                query = specification.IncludeStrings.Aggregate(query,
                    (current, include) => current.Include(include));
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }
            
            if (specification.IsSplitQuery)
            {
               query = query.AsSplitQuery();
            }

            if (specification.IsNoTracking)
            {
               query = query.AsNoTracking();
            }

            return query;
        }
    }
}
