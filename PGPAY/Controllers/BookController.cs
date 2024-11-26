using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PGPAY_BL.IService;

namespace PGPAY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IDashboardService service;
        public BookController(IDashboardService service)
        {
            this.service = service;
        }

        [HttpPost("BookHostel")]
        public async Task<IActionResult> BookHostel()
        {
            return Ok();
        }
    }
}
