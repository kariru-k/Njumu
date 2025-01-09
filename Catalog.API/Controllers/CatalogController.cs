using System.Net;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class CatalogController: ApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponse), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<ProductResponse>> GetProductById(string id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }


    
    [HttpGet]
    [Route("[action]/{name}", Name = "GetProductsByName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByName(string name)
    {
        var query = new GetProductByNameQuery(name);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    [HttpGet]
    [Route("[action]/{brand}", Name = "GetProductsByBrand")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByBrand(string brand)
    {
        var query = new GetProductByBrandQuery(brand);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    [HttpGet]
    [Route("[action]", Name = "GetAllProducts")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts()
    {
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    [HttpGet]
    [Route("[action]", Name = "GetAllBrands")]
    [ProducesResponseType(typeof(IList<BrandResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
    {
        var query = new GetAllBrandsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    [HttpGet]
    [Route("[action]", Name = "GetAllTypes")]
    [ProducesResponseType(typeof(IList<TypeResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<IList<TypeResponse>>> GetAllTypes()
    {
        var query = new GetAllTypesQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("CreateProduct")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);

        return Ok(result);
    }
    
    [HttpPut]
    [Route("UpdateProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> UpdateProduct([FromBody] UpdateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);

        return Ok(result);
    }
    
    [HttpDelete]
    [Route("[action]/{id}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult<ProductResponse>> DeleteProduct(string id)
    {
        var command = new DeleteProductByIdCommand(id);

        var result = await _mediator.Send(command);

        return Ok(result);
    }
    

}