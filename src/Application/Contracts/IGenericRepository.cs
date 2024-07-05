using Application.Contracts.Specification;
using Domain.Entities.Base;
using System.Linq.Expressions;

namespace Application.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id,CancellationToken cancellationToken);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity,CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<T,bool>> expression,CancellationToken cancellationToken);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(CancellationToken cancellationToken);
        public Task<IReadOnlyList<T>> ListAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken);
        public Task<T> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken);
        Task<int> CountAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken);
        Task<List<T>> ToListAsync(CancellationToken cancellationToken);
    }
}
