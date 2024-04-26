using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsBlogAPI.DTO;
using NewsBlogAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<Admin> userManger;
        private readonly IConfiguration config;

        public AdminController(UserManager<Admin> userManger, IConfiguration config)
        {
            this.userManger = userManger;
            this.config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
        {
            if (ModelState.IsValid)
            {
                Admin user = new Admin();

                user.UserName = regUserDTO.UserName;
                user.Email = regUserDTO.Email;

                if (await userManger.FindByEmailAsync(user.Email) is not null)
                {
                    ModelState.AddModelError("emailError", "Email already exists");
                    return BadRequest(ModelState);
                }
                if (await userManger.FindByNameAsync(user.UserName) is not null)
                {
                    ModelState.AddModelError("usernameError", "Username already exists");
                    return BadRequest(ModelState);
                }


                IdentityResult result = await userManger.CreateAsync(user, regUserDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }

                // Add role to the user

                LoginUserDTO logUserDTO = new LoginUserDTO();
                logUserDTO.Email = regUserDTO.Email;
                logUserDTO.Password = regUserDTO.Password;
                return await Login(logUserDTO);
                //return Ok("Account Created Successfully");
            }
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO logUserDto)
        {
            if (ModelState.IsValid)
            {
                Admin user = await userManger.FindByEmailAsync(logUserDto.Email);
                if (user is not null)
                {
                    bool rightPw = await userManger.CheckPasswordAsync(user, logUserDto.Password);
                    if (rightPw)
                    {
                        var userClaims = new List<Claim>();
                        userClaims.Add(new Claim("userID", user.Id));
                        userClaims.Add(new Claim("userEmail", user.Email));
                        userClaims.Add(new Claim("userName", user.UserName));
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


                        SecurityKey securityKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));


                        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken petsToken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],//url web api
                            audience: config["JWT:ValidAudiance"],//url consumer angular
                            claims: userClaims,
                            expires: DateTime.Now.AddDays(15),
                            signingCredentials: credentials
                        );

                        // Save the token to the AspNetUserTokens table
                        await userManger.SetAuthenticationTokenAsync(await userManger.FindByEmailAsync(logUserDto.Email), "JWT", "AccessToken", new JwtSecurityTokenHandler().WriteToken(petsToken));
                        return Ok(
                        new
                        {
                            Message = "Logged in successfully",
                            StatusCode = StatusCodes.Status200OK,
                            token = new JwtSecurityTokenHandler().WriteToken(petsToken),
                            validTo = petsToken.ValidTo
                        });
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
    }
}
