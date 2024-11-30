using Microsoft.AspNetCore.Mvc;
using PGPAY_BL.IService;
using PGPAY_Model.Model.Login;
using PGPAY_Model.Model.Response;
using System.ComponentModel.DataAnnotations;

namespace PGPAY.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;
        private readonly IConfiguration _c;
        public LoginController(ILoginService service, IConfiguration c)
        {
            this.service = service;
            _c = c;
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

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            ResponseModel response = await service.ForgotPassword(Email);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> resetpassword(ResetPassword RPassword)
        {
            ResponseModel response = await service.resetpassword(RPassword);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetUniqueIdForForgotPassword")]
        public async Task<IActionResult> GetUniqueIdForForgotPassword(Guid uniqueId)
        {
            ResponseModel response = await service.GetUniqueIdForForgotPassword(uniqueId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
