using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject2026.Application.RoomTypes.Commands;
using PetProject2026.Application.RoomTypes.Queries;

namespace PetProject2026.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoomTypesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await _mediator.Send(new GetRoomTypesQuery(), ct));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => Ok(await _mediator.Send(new GetRoomTypeByIdQuery(id), ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomTypeCommand cmd, CancellationToken ct)
        {
            var dto = await _mediator.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomTypeCommand cmd, CancellationToken ct)
        {
            if (id != cmd.Id) return BadRequest("Id không khớp.");
            return Ok(await _mediator.Send(cmd, ct));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteRoomTypeCommand(id), ct);
            return NoContent();
        }
    }
}