using ApiTest.Application.Categories.Commands;
using ApiTest.Application.Categories.Commands.ReactivateCategory;
using ApiTest.Application.DTOs;
using ApiTest.Application.Products.Commands.ReactivateProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    => Ok(await _mediator.Send(new GetCategoriesListQuery()));

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
        => await _mediator.Send(new GetCategoryByIdQuery(id)) is CategoryDto category ? Ok(category) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        var categoryId = await _mediator.Send(command);
        return CreatedAtAction(null, null, categoryId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommandResponse>> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.CategoryId) return BadRequest("ID no coincide");
        return await _mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CommandResponse>> Delete(int id)
    {
        return await _mediator.Send(new DeleteCategoryCommand(id));
    }
    [HttpPatch("{id}/reactivate")]
    public async Task<ActionResult<CommandResponse>> Reactivate(int id)
    {
        return await _mediator.Send(new ReactivateCategoryCommand(id));
    }
}