using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
       
        public UsersController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == login.UserName);

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

            if (!passwordMatch)
                return Unauthorized("Invalid password");
            /*string password = BCrypt.Net.BCrypt.HashPassword(login.Password);
            if (user == null || user.Password != password)
                return Unauthorized("Invalid username or password");*/

           

            if(user.Role_Id=="1"|| user.Role_Id == "3")
            {
                var token = GenerateJwtToken(user);
                return Ok(new
                {
                    token,
                    user.userId,
                    user.Name,
                    user.Role_Id
                });
            }
            else
            {
                return Unauthorized("Invalid password");
            }
                            
        }


        [AllowAnonymous]
        [HttpPost("userlogin")]
        public async Task<IActionResult> userlogin(LoginDto login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == login.UserName);
            bool passwordMatch = false;
            try
            {
                passwordMatch = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
                if (!passwordMatch)
                {
                    passwordMatch = login.Password == user.Password;
                }
            }
            catch (Exception)
            {
                // If password is not hashed or invalid format
                passwordMatch = login.Password == user.Password;
            }

            if (!passwordMatch)
                return Unauthorized("Invalid password");
            
            if (user.Role_Id == "2" || user.Role_Id == "4")
            {
                var token = GenerateJwtToken(user);
                return Ok(new
                {
                    token,
                    user.userId,
                    user.Name,
                    user.Role_Id
                });
            }
            else
            {
                return Unauthorized("Invalid password");
            }

        }

        private string GetRoleName(string roleId)
        {
            return roleId switch
            {
                "1" => "Admin",
                "2" => "User",
                _ => "User"
            };
        }

        private string GenerateJwtToken(User user)
        {
            var roleName = GetRoleName(user.Role_Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roleName)
            };

            
            var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config["JwtSettings:Key"])
                    );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ✅ GET ALL USERS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // ✅ GET USER BY USERNAME
        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetByUserName(string username)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // ✅ ADD USER
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // ✅ UPDATE USER
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
                return BadRequest("ID mismatch");

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("User Updated");
        }

        // ✅ DELETE USER
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User Deleted");
        }

        [AllowAnonymous]
        [HttpPost("convert")]
        public IActionResult ConvertSignature([FromBody] SignatureRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Base64Signature))
                    return BadRequest("Signature data is empty");

                string cleanBase64 = request.Base64Signature
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "");

                byte[] sigBytes = Convert.FromBase64String(cleanBase64);

                // Save temporary .sig file
                string sigPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sig");
                System.IO.File.WriteAllBytes(sigPath, sigBytes);

                Type sigCtlType = Type.GetTypeFromProgID("Florentis.SigCtl");

                if (sigCtlType == null)
                    return BadRequest("SigCaptX not installed");

                dynamic sigCtl = Activator.CreateInstance(sigCtlType);

                sigCtl.SetProperty("Licence", "AgAfADeB5ivVARNXYWNvbSBTaWduYXR1cmUgU0RLAAECgQMBAmQA");

                // Load signature into SigCtl
                sigCtl.Read(sigPath);

                int flags = 0x2000 + 0x80000 + 0x400000;

                string b64 = sigCtl.Signature.RenderBitmap(
                    "",
                    300,
                    150,
                    "image/png",
                    0.5,
                    0xff0000,
                    0xffffff,
                    0.0,
                    0.0,
                    flags
                );

                byte[] pngBytes = Convert.FromBase64String(b64);

                if (System.IO.File.Exists(sigPath))
                    System.IO.File.Delete(sigPath);

                return File(pngBytes, "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("convert2")]
        public IActionResult ConvertSignature2([FromBody] SignatureRequest request)
        {
            if (string.IsNullOrEmpty(request.Base64Signature))
                return BadRequest("Signature missing");

            // remove line breaks
            var cleanBase64 = request.Base64Signature
                .Replace("\n", "")
                .Replace("\r", "");

            byte[] bytes = Convert.FromBase64String(cleanBase64);

            return Ok(new { length = bytes.Length });
        }

    }
}
