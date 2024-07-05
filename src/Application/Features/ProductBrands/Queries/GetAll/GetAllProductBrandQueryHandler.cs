using Application.Contracts;
using Domain.Entities.ProductEntity;
using MediatR;

namespace Application.Features.ProductBrands.Queries.GetAll
{
    public class GetAllProductBrandQueryHandler:IRequestHandler<GetAllProductBrandQuery,IEnumerable<ProductBrand>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllProductBrandQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<ProductBrand>> Handle(GetAllProductBrandQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new ProductBrandSpec();
            return await _uow.Repository<ProductBrand>().ListAsyncSpec(spec, cancellationToken);
            //return await _uow.Repository<ProductBrand>().GetAllAsync(cancellationToken);
        }
    }
}
