using Microsoft.AspNetCore.Mvc;
using PGPAY_BL.IService;
using PGPAY_Model.Model.Response;
using System.ComponentModel.DataAnnotations;

namespace PGPAY.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService service;
        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            ResponseModel response = await service.Login(Email, Password);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
