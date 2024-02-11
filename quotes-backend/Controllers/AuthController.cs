using DTO;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [SwaggerOperation(
        Summary = "Generates a JWT token"
    )]
    [HttpPost]
    [Route("/Auth")]
    public IActionResult Auth([FromBody] User user)
    {
        try
        {
            bool pwcorrect = user.CheckPassword();
            if (pwcorrect)
            {
                User u = new UserCollection().GetUser(user.Username);
                var claims = new List<Claim>
                {
                    new("uid", u.Username),
                    new("EYO","WHY U DECODE THIS?!?!?!?, THIS IS PERSONAL INFO")
                    // You can add more claims if needed, e.g., new Claim(ClaimTypes.Name, u.Username)
                };
                var secretKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes("thisisasecretkey@1234567890123456"));
                var signinCredentials = new SigningCredentials
                (secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "DigitalIndividuals",
                    claims: claims,
                    audience: "@everyone",
                    expires: DateTime.Now.AddHours(10),
                    signingCredentials: signinCredentials
                );
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
            }

            return Unauthorized();
        }catch
        {
            return Unauthorized();
        }
}

}