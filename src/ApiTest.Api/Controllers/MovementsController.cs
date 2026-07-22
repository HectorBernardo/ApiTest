using ApiTest.Application.Inventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MovementsController: ControllerBase
    {
        private readonly IMediator _mediator;

        public MovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetMovementsListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
