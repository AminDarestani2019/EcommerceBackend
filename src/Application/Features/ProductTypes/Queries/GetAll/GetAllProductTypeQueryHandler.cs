using Application.Contracts;
using Application.Features.ProductBrands.Queries.GetAll;
using Domain.Entities.ProductEntity;
using MediatR;

namespace Application.Features.ProductTypes.Queries.GetAll
{
    public class GetAllProductTypeQueryHandler:IRequestHandler<GetAllProductTypeQuery,IEnumerable<ProductType>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllProductTypeQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<ProductType>> Handle(GetAllProductTypeQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new ProductTypeSpec();
            return await _uow.Repository<ProductType>().ListAsyncSpec(spec, cancellationToken);
            //return await _uow.Repository<ProductType>().GetAllAsync(cancellationToken);
        }
    }
}
