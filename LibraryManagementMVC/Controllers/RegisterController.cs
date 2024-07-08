using loginmodel;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementMVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly Middleware.Middleware _middleware;

        public RegisterController(Middleware.Middleware middleware)
        {
            _middleware = middleware;
        }

        public IActionResult Index()
        {
            ViewBag.error = "";
            ViewBag.success = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            var result = await _middleware.Register(model);
            if(result != null)
            {
                ViewBag.success = "Başarıyla kaydoldunuz. Kaydınız onay aşamasına geçmiştir.";
                return View();
            } else
            {
                ViewBag.error = "Bu kullanıcı adı zaten kullanılıyor veya şifre kurallara aykırı.";
                return View();
            }
        }
    }
}
