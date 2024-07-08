using LibraryManagementService.Entities;
using LibraryManagementService.HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementService.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Users.All());
        }

        [HttpGet]
        public async Task<IActionResult> GetByMail(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel userModel)
        {
            var user = new IdentityUser { 
                Email = userModel.Email,
                UserName = userModel.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user,userModel.Password);
            if (result.Succeeded) {
                await _userManager.AddToRoleAsync(user, userModel.Role);
                return Ok(user);

            } else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string mail,string currentPassword,string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user != null) {
                var result = await _userManager.ChangePasswordAsync(user,currentPassword, newPassword);
                if (result.Succeeded) { 
                    return Ok(newPassword);
                } else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string mail, string newMail, string Role)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user != null)
            {
                user.Email = newMail;

                var currentRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                var role = await _roleManager.FindByNameAsync(Role);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, Role);
                }
                else
                {
                    return BadRequest();
                }

                await _userManager.UpdateAsync(user);

                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmUser(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user != null) {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");

                return Ok(user);
            }
            else { return BadRequest(); }

        }

        [HttpGet]
        public async Task<IActionResult> RejectUser(string mail) {
            var user = await _userManager.FindByEmailAsync(mail);
            await _userManager.DeleteAsync(user);
            return Ok();
        }

    }
}
