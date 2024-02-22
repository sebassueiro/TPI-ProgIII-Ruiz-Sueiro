using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.Models;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public IUserService _userService;
        public IConfiguration _configuration;

        public AuthenticateController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] CredentialsDto credentialsDto)
        {
            //validar usuario
            BaseResponse userValidationResult = _userService.UserValidation(credentialsDto.Email, credentialsDto.Password);
            if (userValidationResult.Message == "email incorrecto")
            {
                return BadRequest(userValidationResult.Message);
            }
            else if (userValidationResult.Message == "contraseña incorrecta")
            {
                return Unauthorized(userValidationResult.Message);
            }
            else if (userValidationResult.Message == "Por favor, ingrese email y contraseña")
            {
                return BadRequest(userValidationResult.Message);
            }
            if (userValidationResult.Result)
            {

                User user = _userService.GetUserByEmail(credentialsDto.Email);

                var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

                var signature = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);


                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("sub", user.Id.ToString()));
                claimsForToken.Add(new Claim("email", user.Email));
                claimsForToken.Add(new Claim("username", user.UserName));
                claimsForToken.Add(new Claim("role", user.UserType));

                var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],
                    claimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    signature);

                string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return Ok(tokenToReturn);
            }
            return BadRequest();
        }
    }
}
