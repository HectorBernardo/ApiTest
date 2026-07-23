using ApiTest.Application.DTOs;
using ApiTest.Application.Inventory.Commands.CreateMovement;
using ApiTest.Application.Products.Commands.CreateProduct;
using ApiTest.Application.Products.Commands.DeleteProduct;
using ApiTest.Application.Products.Commands.ReactivateProduct;
using ApiTest.Application.Products.Commands.UpdateProduct;
using ApiTest.Application.Products.Queries.GetProductById;
using ApiTest.Application.Products.Queries.GetProductsList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetProductsListQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetAll), new { id = productId }, productId);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CommandResponse>> Delete(int id)
    {
        return await _mediator.Send(new DeleteProductCommand(id));
    }

    [HttpPatch("{id}/reactivate")]
    public async Task<ActionResult<CommandResponse>> Reactivate(int id)
    {
        return await _mediator.Send(new ReactivateProductCommand(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommandResponse>> Update(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.ProductId) return BadRequest("ID no coincide");

        return await _mediator.Send(command);
    }
}