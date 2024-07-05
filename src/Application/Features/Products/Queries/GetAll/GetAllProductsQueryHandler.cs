using Application.Contracts;
using MediatR;
using Application.Dtos.Products;
using AutoMapper;
using Application.Wrappers;
using Domain.Entities.ProductEntity;

namespace Application.Features.Products.Queries.GetAll
{
    public class GetAllProductsQueryHandler:IRequestHandler<GetAllProductsQuery,PaginationResponse<ProductDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IUnitOfWork uow,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetProductSpec(request);
            var count = await _uow.Repository<Product>().CountAsyncSpec(new ProductsCountSpec(request),cancellationToken);
            var result = await _uow.Repository<Product>().ListAsyncSpec(spec, cancellationToken);
            var model = _mapper.Map<IEnumerable<ProductDto>>(result);
            return new PaginationResponse<ProductDto>(request.PageIndex,request.PageSize,count,model);
            //logic 
            //return await _uow.Repository<Product>().GetAllAsync(cancellationToken);
        }
    }
}
