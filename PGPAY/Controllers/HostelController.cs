using Microsoft.AspNetCore.Mvc;
using PGPAY_BL.IService;
using PGPAY_BL.Service;
using PGPAY_Model.Model.Response;

namespace PGPAY.Controllers
{
    public class HostelController : Controller
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
    }
}
