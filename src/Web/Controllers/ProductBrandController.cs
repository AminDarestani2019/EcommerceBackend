using Application.Features.ProductBrands.Queries.GetAll;
using Domain.Entities.ProductEntity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductBrandController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetAllProductBrand(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetAllProductBrandQuery(), cancellationToken));
        }
    }
}
