using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject2026.Application.RatePlans.Command;
using PetProject2026.Application.RatePlans.Queries;

namespace PetProject2026.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatePlansController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RatePlansController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await _mediator.Send(new GetRatePlansQuery(), ct));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => Ok(await _mediator.Send(new GetRatePlanByIdQuery(id), ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRatePlanCommand cmd, CancellationToken ct)
        {
            var dto = await _mediator.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRatePlanCommand cmd, CancellationToken ct)
        {
            if (id != cmd.Id) return BadRequest("Id không khớp.");
            return Ok(await _mediator.Send(cmd, ct));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteRatePlanCommand(id), ct);
            return NoContent();
        }
    }
}
