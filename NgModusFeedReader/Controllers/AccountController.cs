using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModusCreateSampleApp.Data.Entities;
using NgModusFeedReader.Data.ViewModels;

namespace NgModusFeedReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        private JwtSecurityToken GenerateToken(User user)
        {            
            var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Security:JWT:Tokens:Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                            _config["Security:JWT:Tokens:Issuer"],
                            _config["Security:JWT:Tokens:Audience"],
                            claims,
                            notBefore: DateTime.UtcNow,
                            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Security:JWT:Tokens:ExpirationTimeMinutes"])),
                            signingCredentials: credentials
                        );
            return jwtToken;
        }

        public AccountController(ILogger<AccountController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        /*[HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            _logger.LogInformation($"Attmpeting to login: {loginModel.Email} @ {DateTime.Now}");
            if (ModelState.IsValid)
            {
                //dont lockout on failed attempts
                var loginAttempt = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false,false);

                if (loginAttempt.Succeeded)
                {
                    _logger.LogInformation($"Successfully logged in: {loginModel.Email} @ {DateTime.Now}");
                }
                else
                {
                    _logger.LogInformation($"Failed login attempt for: {loginModel.Email} @ {DateTime.Now}");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return BadRequest("Wrong username or password");
        }*/


        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel signup)
        {
            if (ModelState.IsValid)
            {
                //Didnt have enough time to implement AutoMapper :(
                var newUser = new User()
                {
                    FirstName = signup.FirstName,
                    LastName = signup.LastName,
                    Email = signup.Email,
                    UserName = signup.Email
                };

                var result = await _userManager.CreateAsync(newUser, signup.Password);
                if (result.Succeeded)
                {
                    var jwtToken = GenerateToken(newUser);
                    var jwtTokenPayload = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    };
                    return Created("", jwtTokenPayload);
                }
                else
                {                   
                    return BadRequest(result.ToString());
                }
            }

            return BadRequest(ModelState);
        }


        /*[HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }*/

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var loginAttempt = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (loginAttempt.Succeeded)
                    {
                        //create jwt token
                        var jwtToken = GenerateToken(user);
                        var jwtTokenPayload = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            expiration = jwtToken.ValidTo
                        };
                        return Created("", jwtTokenPayload);
                    }
                }

                
            }
            return BadRequest();
        }
    }
}