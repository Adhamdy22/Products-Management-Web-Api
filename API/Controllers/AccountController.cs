using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViewModels.Account;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<User> _userManager,IConfiguration _configration) { 
            userManager=_userManager;
            configuration= _configration;
        }

       
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email is already in use. Please choose another email.");
                    return BadRequest(ModelState);
                }
                User user = new()
                {
                   FirstName = model.FirstName,
                   LastName = model.LastName,
                   UserName = model.UserName, 
                   Email = model.Email,
                   PhoneNumber = model.PhoneNumber,
                   //PasswordHash=model.Password
                };
                
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //return Ok("Registeration Successfuly");
                    var claims = new List<Claim>();

                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    var roles = await userManager.GetRolesAsync(user);

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        claims: claims,
                        issuer: configuration["JWT:Issuer"],
                        audience: configuration["JWT:Audience"],
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: sc
                    );

                    var _token = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        Roles = roles
                    };
                    return Ok(_token);
                }
                else
                {
                   foreach (var item in result.Errors)
                   {
                        ModelState.AddModelError("", item.Description);
                   }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginViewModel model) 
        {
            if (ModelState.IsValid) { 
                var user = await userManager.FindByNameAsync(model.LoginMethod) ?? await userManager.FindByEmailAsync(model.LoginMethod);
                if (user != null) { 

                    if( await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        //return Ok("Token");
                        var claims = new List<Claim>();

                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
                        var roles = await userManager.GetRolesAsync(user);

                        foreach(var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            claims:claims,
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires:DateTime.Now.AddHours(1),
                            signingCredentials:sc
                        );

                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration=token.ValidTo,
                            Roles=roles
                        };
                        return Ok(_token );
                    }

                    else
                    {
                        return BadRequest("Invalid Username Or Password"); 
                    }
                
                }
                return NotFound("User not found");
            }
            return BadRequest("Invalid Username Or Password");
        }

    }
}
