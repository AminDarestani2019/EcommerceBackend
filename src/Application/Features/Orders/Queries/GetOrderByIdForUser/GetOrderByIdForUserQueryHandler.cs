using Application.Contracts;
using Application.Dtos.OrderDto;
using AutoMapper;
using Domain.Entities.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Application.Features.Orders.Queries.GetOrderByIdForUser
{
    public class GetOrderByIdForUserQuery:IRequest<OrderDto>
    {
        public int Id { get; set; }
        public GetOrderByIdForUserQuery(int id)
        {
            Id = id;
        }
    }
    public class GetOrderByIdForUserQueryHandler:IRequestHandler<GetOrderByIdForUserQuery ,OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public GetOrderByIdForUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<OrderDto> Handle(GetOrderByIdForUserQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<Order>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            return _mapper.Map<OrderDto>(order);
        }
    }
}
