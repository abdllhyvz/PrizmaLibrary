using LibraryManagementService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementService.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleController(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Roles.All());
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersRole(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string name)
        {
            //await _unitOfWork.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole(name));
            await _roleManager.CreateAsync(new IdentityRole(name));
            await _unitOfWork.SaveAsync();
            return Ok(name);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string mail,string roleName)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            var role = await _roleManager.FindByNameAsync(roleName);
            if(user != null)
            {
                await _userManager.AddToRoleAsync(user, roleName);
                return Ok("Rol Atandı");
            }
            return BadRequest();
        }
    }
}
