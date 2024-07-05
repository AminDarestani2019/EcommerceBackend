using Application.Contracts;
using Domain.Entities.Order;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZarinpalSandbox;

namespace Application.Features.Orders.Commands.Verify
{
    public class VerifyCommand:IRequest<string>
    {
        public string Authority { get; set; }
        public string Status { get; set; }
        public VerifyCommand(string authority, string status)
        {
            Authority = authority;
            Status = status;
        }
    }
    public class VerifyCommandHandler : IRequestHandler<VerifyCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public VerifyCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(VerifyCommand request, CancellationToken cancellationToken)
        {
            //1. order : authority
            var order = await _unitOfWork.Context.Set<Order>()
                .Include(x=>x.DeliveryMethod)
                .Where(x=>x.Authority == request.Authority)
                .SingleOrDefaultAsync(cancellationToken);
            if (order == null) throw new BadRequestEntityException("سفارش شما یافت نشد، مجددا تلاش کنید");
            //2. portal : orderId
            var portal = await _unitOfWork.Repository<Portal>().Where(x=>x.OrderId == order.Id)
                .SingleOrDefaultAsync(cancellationToken);
            if (portal == null) throw new BadRequestEntityException("پرداخت شما مشکل دارد، لطفا با پشتیبانی تماس بگیرید");
            //3. cancel submitted
            if (request.Status != "OK")
            {
                //cancel submit
                //update order
                order.OrderSTatus = OrderStatus.Cancelled;
                await _unitOfWork.Repository<Order>().UpdateAsync(order);
                await _unitOfWork.Save(cancellationToken);
                //update portal
                portal.Status = PaymentDataStatus.Canceled;
                await _unitOfWork.Repository<Portal>().UpdateAsync(portal);
                return _configuration["Order:CallBackCanceled"];
            }
            //4. status => success,unsuccessful
            var amount = (int)order.GetOriginalTotal();
            var payment = new Payment(amount);
            var result = await payment.Verification(request.Authority);
            if (result.Status == 100)
            {
                //success
                //update order
                order.IsFinally = true;
                order.OrderSTatus = OrderStatus.Pending;
                await _unitOfWork.Repository<Order>().UpdateAsync(order);
                //update portal
                portal.ReferenceId = result.RefId.ToString();
                portal.Status = PaymentDataStatus.Success;
                await _unitOfWork.Repository<Portal>().UpdateAsync(portal);
                await _unitOfWork.Save(cancellationToken);
                //redirect
                return _configuration["Order:CallBackSuccess"];
            }
            //failed , unsuccessful
            //update order
            order.OrderSTatus = OrderStatus.PaymentFailed;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            //update portal
            portal.Status = PaymentDataStatus.Failed;
            await _unitOfWork.Repository<Portal>().UpdateAsync(portal);
            await _unitOfWork.Save(cancellationToken);
            return _configuration["Order:CallBackFailed"];
        }
    }
}
