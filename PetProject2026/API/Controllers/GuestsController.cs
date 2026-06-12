using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject2026.Application.Guests.Command;
using PetProject2026.Application.Guests.Queries;

namespace PetProject2026.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GuestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuest([FromBody] CreateGuestCommand command, CancellationToken ct)
        {
            var guest = await _mediator.Send(command, ct);
            return Ok(guest);
        }

        // GET api/guests?search=an&pageNumber=1&pageSize=10
        // Tìm khách theo tên/sđt, có phân trang. Tất cả tham số đều optional.
        [HttpGet]
        public async Task<IActionResult> GetGuests([FromQuery] GetGuestsQuery query, CancellationToken ct)
        {
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        // GET api/guests/5 -> nếu không tìm thấy, handler ném NotFoundException,
        // middleware bắt và trả về 404.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuestById(int id, CancellationToken ct)
        {
            var guest = await _mediator.Send(new GetGuestByIdQuery(id), ct);
            return Ok(guest);
        }
    }
}
