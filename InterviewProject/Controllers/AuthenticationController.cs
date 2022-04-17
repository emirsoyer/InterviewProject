using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InterviewProject.DbContext;
using InterviewProject.IdentityAuth;
using InterviewProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace InterviewProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        public readonly DatabaseContext _context;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager, IConfiguration configuration, DatabaseContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (model != null && model.UserName != null && model.Password != null)
            {
                var user = await GetUser(model.UserName, model.Password, model.RoleId);
                //var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim("Id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Password", user.Password),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
               

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials:new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo});
            }

            return BadRequest("Invalid Credentials");
        }

        [HttpGet]
        public async Task<User> GetUser(string userName, string pass, int roleId)
        {
            return await _context.Users.FirstOrDefaultAsync((u => u.UserName == userName && u.Password == pass && u.RoleId == roleId));
        }
        //[HttpGet]
        //public async Task<Role> GetRole(string roleName, int roleId)
        //{
        //    return await _context.Roles.FirstOrDefaultAsync((u => u.RoleName == roleName && u.RoleId == roleId));
        //}
    }
}
