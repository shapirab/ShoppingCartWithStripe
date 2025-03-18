using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.data.DataModels.Dtos.UserDtos;
using ShoppingCart.data.DataModels.Entities;
using System.Security.Claims;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(SignInManager<AppUser> signInManager, IMapper mapper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            AppUser user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };
            //AppUser user = mapper.Map<AppUser>(registerDto);
            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            // Important: This creates and sets the auth cookie
            await signInManager.SignInAsync(user, new AuthenticationProperties
            {
                IsPersistent = true,
            });

            // Add this temporary debug line
            var authCookie = Response.Headers.FirstOrDefault(h => h.Key.Contains("Set-Cookie"));
            //_logger.LogInformation("Auth cookie being set: {cookie}", authCookie.Value);

            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return NoContent();
            }

            AppUser? user = await signInManager.UserManager.Users
                .Include(user => user.Address)
                .FirstOrDefaultAsync(user => user.Email == User.FindFirstValue(ClaimTypes.Email));

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                address = mapper.Map<AddressDto>(user.Address),
                roles = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        //[HttpGet("user-info")]
        //public async Task<ActionResult> GetUserInfo()
        //{
        //    We don't need to check IsAuthenticated because [Authorize] handles that
        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return Unauthorized();
        //    }

        //    AppUser? user = await signInManager.UserManager.Users
        //        .Include(user => user.Address)
        //        .FirstOrDefaultAsync(user => user.Email == email);

        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(new
        //    {
        //        user.FirstName,
        //        user.LastName,
        //        user.Email,
        //        address = mapper.Map<AddressDto>(user.Address)
        //    });
        //}

        [HttpGet("auth-status")]
        public ActionResult GetAuthState()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
        {
            AppUser? user = await signInManager.UserManager.Users
                .Include(user => user.Address)
                .FirstOrDefaultAsync(user => user.Email == User.FindFirstValue(ClaimTypes.Email));

            if(user == null)
            {
                return Unauthorized();
            }

            if (user.Address == null)
            {
                user.Address = mapper.Map<Address>(addressDto);
            }
            else
            {
                user.Address = mapper.Map(addressDto, user.Address);
            }

            var results = await signInManager.UserManager.UpdateAsync(user);
            if (!results.Succeeded)
            {
                return BadRequest("Problem updating user address");
            }
            return Ok(mapper.Map<AddressDto>(user.Address));
        }
    }
}
