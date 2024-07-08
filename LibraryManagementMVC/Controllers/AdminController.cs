using LibraryManagementMVC.Middleware.APIModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Middleware.Middleware _middleware;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AdminController(Middleware.Middleware middleware, IHttpContextAccessor httpContextAccessor)
        {
            _middleware = middleware;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddUser()
        {
            var roles = await _middleware.GetRoles();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel userModel)
        {
            var result = await _middleware.CreateUser(userModel);
            if (result)
            {
                return RedirectToAction("Users","Admin");
            }
            else
            {
                var roles = await _middleware.GetRoles();
                return View(roles);
            }
        }

        public async Task<IActionResult> Users()
        {
            var model = await _middleware.GetUsers();
            return View(model);
        }

        public async Task<IActionResult> EditUser(string mail)
        {
            var model = await _middleware.GetUserByMail(mail);
            var roles = await _middleware.GetRoles();
            var userRole = await _middleware.GetUserRole(mail);
            ViewBag.userRole = userRole;
            ViewBag.roles = roles;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string mail,string NewEmail,string Role)
        {
            var model = await _middleware.UpdateUser(mail, NewEmail, Role);
            return RedirectToAction("EditUser","Admin", new { mail = NewEmail }); 
        }

        public async Task<IActionResult> Pendings()
        {
            var model = await _middleware.GetUsers();
            model = model.Where(w => w.EmailConfirmed == false).ToList();
            return View(model);
        }

        public async Task<IActionResult> ConfirmUser(string mail)
        {
            var model = await _middleware.ConfirmUser(mail);
            return RedirectToAction("Pendings", "Admin");
        }

        public async Task<IActionResult> RejectUser(string mail)
        {
            await _middleware.RejectUser(mail);
            return RedirectToAction("Pendings", "Admin");

        }

        public IActionResult ChangePasswordAdmin()
        {
            ViewBag.message = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAdmin(string currentPassword, string newPassword)
        {
            var userMail = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var result = await _middleware.ChangePassword(userMail, currentPassword, newPassword);
            if (result)
            {
                ViewBag.message = "Şifreniz başarıyla değiştirildi.";
            }
            else
            {
                ViewBag.message = "Bir hata oluştu. Lütfen girdiğiniz bilgileri kontrol ederek tekrar deneyin.";
            }
            return View();
        }


    }
}
