using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate_Dapper_Api.Dtos.LoginDtos;
using RealEstate_Dapper_Api.Models.DapperContext;
using RealEstate_Dapper_Api.Tools;
using System.Threading.Tasks;

namespace RealEstate_Dapper_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Context _context;

        public LoginController(Context context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(CreateLoginDto loginDto)
        {
            string query = "SELECT * FROM AppUser WHERE Username = @username AND Password = @password";
            string query2 = "SELECT UserId FROM AppUser WHERE Username = @username AND Password = @password";
            var parameters = new DynamicParameters();
            parameters.Add("@username", loginDto.Username);
            parameters.Add("@password", loginDto.Password);

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<CreateLoginDto>(query, parameters);
                if (user == null)
                {
                    return Ok("Başarısız"); // Or better: return Unauthorized("Invalid credentials");
                }

                var userId = await connection.QueryFirstOrDefaultAsync<GetAppUserIdDto>(query2, parameters);
                if (userId == null)
                {
                    return Ok("Başarısız"); // Or better: return Unauthorized("Invalid credentials");
                }

                GetCheckAppUserViewModel model = new GetCheckAppUserViewModel
                {
                    Username = user.Username,
                    Id = userId.UserId
                };

                var token = JwtTokenGenerator.GenerateToken(model);
                return Ok(token);
            }
        }
    }
}
