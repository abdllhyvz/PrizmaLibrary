using loginmodel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly Middleware.Middleware _middleware;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LoginController(Middleware.Middleware middleware,IHttpContextAccessor httpContextAccessor)
        {
            _middleware = middleware;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewBag.error = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            var result = await _middleware.Login(model);
            if (result != null) {
                var role = await _middleware.GetUserRole(model.Email);
                if(role != null)
                {
                    var claims = new List<Claim>
                {
                new Claim(type:ClaimTypes.Name,value:model.Email),
                new Claim(type:ClaimTypes.Role,value:role)

                };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            AllowRefresh = true,
                        }
                    );
                    if(role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");

                    } else if(role == "Officer")
                    {
                        return RedirectToAction("Index", "Officer");

                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Bir hata oluştu.";
                    return View();
                }

            } else
            {
                ViewBag.error = "Kullanıcı Adı veya Şifre Yanlış.";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Login");
        }
    }
}
