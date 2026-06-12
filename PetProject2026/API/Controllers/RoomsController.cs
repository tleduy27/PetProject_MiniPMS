using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject2026.Application.Rooms.Queries;

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

        /// <summary>Danh sách phòng kèm trạng thái — phục vụ Floor Map.</summary>
        [HttpGet]
        public async Task<IActionResult> GetRooms(CancellationToken ct)
        {
            var rooms = await _mediator.Send(new GetRoomsQuery(), ct);
            return Ok(rooms);
        }
    }
}
