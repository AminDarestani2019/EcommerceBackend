using Application.Features.ProductTypes.Queries.GetAll;
using Domain.Entities.ProductEntity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductTypeController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetAllProductType(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetAllProductTypeQuery(), cancellationToken));
        }
    }
}
