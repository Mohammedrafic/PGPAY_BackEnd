using Microsoft.AspNetCore.Mvc;
using PGPAY_BL.IService;
using PGPAY_Model.Model.Response;

namespace PGPAY.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService service;
        public DashboardController(IDashboardService service)
        {
            this.service = service;
        }
        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetails(int HostelId)
        {
            ResponseModel response = await service.GetUserDetails(HostelId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
