using Microsoft.AspNetCore.Mvc;
using PGPAY_BL.IService;
using PGPAY_BL.Service;
using PGPAY_DL.dto;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;

namespace PGPAY.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class HostelController : ControllerBase
    {
        private readonly IHostelService service;
        public HostelController(IHostelService service)
        {
            this.service = service;
        }
        [HttpGet("GetAllHostelDetails")]
        public async Task<IActionResult> GetAllHostelDetails()
        {
            ResponseModel response = await service.GetAllHostelDetails();
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetHostelRequest/{UserId}")]
        public async Task<IActionResult> GetHostelRequest(int UserId)
        {
            ResponseModel response = await service.GetHostelRequestById(UserId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetHostelDetailsById")]
        public async Task<IActionResult> GetHostelDetailsById(int UserID)
        {
            ResponseModel response = await service.GetHostelDetailsById(UserID);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("HostelBookingRequest")]
        public async Task<IActionResult> HostelBookingRequest([FromBody]BookingRequestDto bookingRequest)
        {
            ResponseModel response = await service.HostelBookingRequest(bookingRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetHostelFullDetailsById")]
        public async Task<IActionResult> GetHostelFullDetailsById(int HostelID)
        {
            ResponseModel response = await service.GetHostelFullDetailsById(HostelID);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("AddHostelDetails")]
        public async Task<IActionResult> AddHostelDetails([FromForm] HostelDetails bookingRequest)
        {
            ResponseModel response = await service.AddHostelDetails(bookingRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("RatingHostel")]
        public async Task<IActionResult> RatingHostel([FromBody] Ratingdto Ratingdto)
        {
            ResponseModel response = await service.RatingHostel(Ratingdto);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
