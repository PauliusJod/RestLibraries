using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestLibraries.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestLibraries.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<LibrariesUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<LibrariesUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null)
                return BadRequest("Request invalid.");

            var newUser = new LibrariesUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
            if(!createUserResult.Succeeded)
                return BadRequest("Can't create user.");

            await _userManager.AddToRoleAsync(newUser, LibrariesRoles.LibraryUser);

            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.UserName, newUser.Email));

        }
         //string rawUserId = HttpContext.User.FindFirstValue("id"); // User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            //if(!Guid.TryParse(rawUserId, out Guid userId))
            //{
            //    return Unauthorized("unauthorized......");
            //}
            //await 

            ////return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.UserName, newUser.Email));
            //return NoContent("Ištrintas");


            //TokenLifetimeInMinutes
            //     _jwtTokenService.GetToken();
        [Authorize]
        [HttpDelete]
        [Route("logout")]
        public async Task<ActionResult> Logout()//string userName)
        {

            //var user = _userManager.GetUserAsync(userName);
            //var city = await _userManager.GetAuthenticationTokenAsync(cityid);
            //// 404
            //if (city == null)
            //    return NotFound();
            //await _citiesRepository.DeleteAsync(city);



            //// 204
            //return NoContent();
            var user = await _userManager.FindByNameAsync("paulius33");//logoutDto.UserName);
            if (user == null)
                return BadRequest("Request invalid.");
            var soo = await _userManager.RemoveAuthenticationTokenAsync(user,"HS256","Bearer");
            _userManager.ResetAuthenticatorKeyAsync(user);
            ////valid user
            //var roles = await _userManager.GetRolesAsync(user);
            //_jwtTokenService.DeleteAccessToken(user.UserName, user.Id, roles);

            return Ok("Deleted?");

        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return BadRequest("User name is invalid.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return BadRequest("Password is invalid.");


            //valid user
            var roles = await _userManager.GetRolesAsync(user);
            var accesToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

            return Ok(new SuccessfullLoginDto(accesToken));



        }
    }
}
