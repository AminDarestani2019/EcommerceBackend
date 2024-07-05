using Application.Contracts;
using Application.Dtos.Products;
using Application.Features.Products.Queries.GetAll;
using AutoMapper;
using Domain.Entities.ProductEntity;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Products.Queries.Get
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IUnitOfWork _Uow;
        private readonly IMapper _mapper;
        public GetProductQueryHandler(IUnitOfWork Uow,IMapper mapper)
        {
            _Uow = Uow;
            _mapper = mapper;
        }
        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetProductSpec(request.Id);
            var result = await _Uow.Repository<Product>().GetEntityWithSpec(spec,cancellationToken);
            if (result == null) throw new NotFoundEntityException();
            return _mapper.Map<ProductDto>(result);
        }
    }
}
