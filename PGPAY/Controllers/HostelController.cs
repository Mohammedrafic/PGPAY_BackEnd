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
    }
}
