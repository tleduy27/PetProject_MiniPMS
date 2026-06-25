using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Application.Rooms.Commands;
using PetProject2026.Application.Rooms.Queries;
using PetProject2026.Domain.Enums;

namespace PetProject2026.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Danh sách phòng (tìm theo số phòng + phân trang) — phục vụ Floor Map.</summary>
        // GET api/rooms?search=&pageNumber=&pageSize=
        [HttpGet]
        public async Task<IActionResult> GetRooms([FromQuery] GetRoomsQuery query, CancellationToken ct)
        {
            var rooms = await _mediator.Send(query, ct);
            return Ok(rooms);
        }

        // GET api/rooms/available?roomTypeId=1&checkIn=2026-07-01&checkOut=2026-07-05
        // Số phòng trống của 1 loại phòng trong khoảng ngày — nền tảng cho Reservation.
        // Lưu ý: route literal "available" đặt TRƯỚC "{id}" để không bị bắt nhầm vào GetById.
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] GetAvailableRoomsQuery query, CancellationToken ct)
            => Ok(await _mediator.Send(query, ct));

        // GET api/rooms/5 -> 404 (NotFoundException) nếu không tồn tại
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => Ok(await _mediator.Send(new GetRoomByIdQuery(id), ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomCommand command, CancellationToken ct)
        {
            var dto = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomCommand command, CancellationToken ct)
        {
            if (id != command.Id) return BadRequest("Id không khớp.");
            return Ok(await _mediator.Send(command, ct));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteRoomCommand(id), ct);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<RoomDto>> UpdateStatus(int id, [FromBody] RoomStatus newStatus, CancellationToken ct)
            => Ok(await _mediator.Send(new UpdateRoomStatusCommand(id, newStatus), ct));
    }
}
