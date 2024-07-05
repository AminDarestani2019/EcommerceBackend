using Application.Dtos.Products;
using Application.Features.Products.Queries.Get;
using Application.Features.Products.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromQuery] GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request,cancellationToken));
        }
        //CQRS=>command=>add,update,delete
        //query =>get
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetProductQuery(id), cancellationToken));
        }

    }
}
