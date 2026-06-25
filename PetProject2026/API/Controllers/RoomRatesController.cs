using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject2026.Application.RoomRates.Command;
using PetProject2026.Application.RoomRates.Queries;

namespace PetProject2026.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoomRatesController(IMediator mediator) => _mediator = mediator;

        // GET api/roomrates?roomTypeId=1  (roomTypeId optional)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetRoomRatesQuery query, CancellationToken ct)
            => Ok(await _mediator.Send(query, ct));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => Ok(await _mediator.Send(new GetRoomRateByIdQuery(id), ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomRateCommand cmd, CancellationToken ct)
        {
            var dto = await _mediator.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomRateCommand cmd, CancellationToken ct)
        {
            if (id != cmd.Id) return BadRequest("Id không khớp.");
            return Ok(await _mediator.Send(cmd, ct));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteRoomRateCommand(id), ct);
            return NoContent();
        }
    }
}
