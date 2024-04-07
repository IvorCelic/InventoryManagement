using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EdunovaAPP.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthorizationController : ControllerBase
    {

        private readonly InventoryManagementContext _context;


        public AuthorizationController(InventoryManagementContext context)
        {
            _context = context;
        }



        [HttpPost("token")]
        public IActionResult GenerateToken(EmployeeAuthDTO employee)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var adminDB = _context.Employees
                   .Where(p => p.Email!.Equals(employee.email))
                   .FirstOrDefault();

            if (adminDB == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Not authorized, non existing employee!");
            }



            if (!BCrypt.Net.BCrypt.Verify(employee.password, adminDB.Password))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Not authorized, password doesn't match");
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("I cant do frontend becauseThisDoesntWork and that is RealLYFruSTrAtiNG");


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(8)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);


            return Ok(jwt);

        }
    }
}
