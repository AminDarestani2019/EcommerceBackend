using Application.Contracts;
using Application.Dtos.OrderDto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.BasketEntity;
using Domain.Entities.Order;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using ZarinpalSandbox;
using ZarinpalSandbox.Models;

namespace Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommand:IRequest<OrderDto>
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public ShipToAddress ShipToAddress { get; set; }
        public PortalType PortalType { get; set; } = PortalType.ZarrinPal;
    }
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IBasketRepository basketRepository, 
            IConfiguration configuration, 
            ICurrentUserService currentUserService, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _configuration = configuration;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //1. get basket
            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            //2. delivery method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByIdAsync(request.DeliveryMethodId, cancellationToken);
            //3. connect gateway => success,link,auth
            var amount = (int)(basket.calculateOriginalPrice()+deliveryMethod.Price);
            var payment = await new Payment(amount).PaymentRequest("فاکتور فروش", _configuration["Order:CallBack"],"",request.BuyerPhoneNumber);
            //4. reducer => event handler
            //5. create order 
            var result = await CreateOrder(request, cancellationToken, basket, deliveryMethod, payment);
            //6. delete basket
            await _basketRepository.DeleteBasketAsync(basket.Id);
            //7. create portal
            var portal = new Portal(result.Id, result.PortalType, PaymentDataStatus.Pending, amount, null);
            await _unitOfWork.Repository<Portal>().AddAsync(portal, cancellationToken);
            await _unitOfWork.Save(cancellationToken);
            //8. create response
            var model = _mapper.Map<OrderDto>(result);
            //9. link => 3.link
            model.Link = payment.Link;
            //10. 8.return
            return model;
        }

        private async Task<Order> CreateOrder(CreateOrderCommand request, CancellationToken cancellationToken, CustomerBasket basket, DeliveryMethod deliveryMethod, PaymentRequestResponse payment)
        {
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var itemOrder =new ProductItemOrdered(item.Id,item.Product,item.Brand,item.Type,item.PictureUrl);
                orderItems.Add( new OrderItem() { 
                    ItemOrdered = itemOrder, 
                    Price = item.Price, 
                    Quantity = item.Quantity 
                });
            }
            var order = new Order()
            {
                BuyerPhoneNumber = request.BuyerPhoneNumber,
                ShipToAddress = request.ShipToAddress,
                DeliveryMethod = deliveryMethod,
                OrderItems = orderItems,
                SubTotal = basket.calculateOriginalPrice(),
                PortalType = request.PortalType,
                Authority = payment.Authority,
                CreatedBy = _currentUserService.UserId
            };
            var result = await _unitOfWork.Repository<Order>().AddAsync(order, cancellationToken);
            if(result == null) throw new BadImageFormatException("سفارش شما با شکست روبرو شد لطفا مجددا تلاش کنید");
            await _unitOfWork.Save(cancellationToken);
            return result;
        }
    }
}
