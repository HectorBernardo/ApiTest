using ApiTest.Application.DTOs;
using ApiTest.Application.Inventory.Commands.CreateMovement;
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

        [HttpPost]
        public async Task<ActionResult<CommandResponse>> Create([FromBody] CreateMovementCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);

                return Ok(new CommandResponse(true, "Operación realizada correctamente", id));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new CommandResponse(false, ex.Message));
            }
        }
    }
}
