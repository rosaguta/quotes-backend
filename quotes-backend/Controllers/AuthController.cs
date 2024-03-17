using DTO;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers
{
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
                // Authenticate user credentials
                bool isUserValid = user.CheckPassword();

                if (isUserValid)
                {
                    // Get user details from database
                    User u = new UserCollection().GetUser(user.Username);

                    // Create claims for the JWT token
                    var claims = new List<Claim>
                    {
                        new Claim("uid", u.Username),
                        // Add additional claims as needed
                        new Claim("Rights", u.Rights.ToString()) // Assuming "Rights" is a claim that determines user rights
                    };

                    // Create symmetric security key
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecretkey@1234567890123456"));

                    // Create signing credentials using the secret key
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    // Create JWT security token
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "DigitalIndividuals",
                        audience: "@everyone",
                        claims: claims,
                        expires: DateTime.Now.AddHours(2),
                        signingCredentials: signinCredentials
                    );

                    // Generate and return JWT token
                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    return Ok(token);
                }

                // Unauthorized if user credentials are not valid
                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Log any exceptions and return Unauthorized
                Console.WriteLine($"Error: {ex.Message}");
                return Unauthorized();
            }
        }
    }
}
