using JWTApi.Dtos;
using JWTApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase
    {
        private readonly ITokenAuthenticationService _tokenAuthService;

        public AuthenticationController(ITokenAuthenticationService tokenAuthService)
        {
            _tokenAuthService = tokenAuthService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            string token = "";

            if (_tokenAuthService.IsAuthenticated(request,out token)){
                return Ok(token);
            }
            return Ok();

        }   
    }
}