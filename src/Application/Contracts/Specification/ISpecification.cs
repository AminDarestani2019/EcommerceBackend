using Domain.Entities.Base;
using System.Linq.Expressions;

namespace Application.Contracts.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        Expression<Func<T,bool>> Predicate { get; }
        List<Expression<Func<T,object>>> Includes { get; }
        Expression<Func<T,object>> OrderBy { get; }
        Expression<Func<T,object>> OrderByDesc { get; }
        //pagination
        public int Take {  get; set; }
        public int Skip {  get; set; }
        public bool IsPagingEnabled {  get; set; }
    }
}
