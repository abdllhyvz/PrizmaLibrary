using LibraryManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementMVC.Controllers
{
    [Authorize(Roles = "Officer")]
    public class OfficerController : Controller
    {
        private readonly Middleware.Middleware _middleware;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public OfficerController(Middleware.Middleware middleware, IHttpContextAccessor httpContextAccessor)
        {
            _middleware = middleware;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book) { 
            var result = await _middleware.CreateBook(book);
            return View();
        }

        public async Task<IActionResult> Books() { 
            var model = await _middleware.GetBooks();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _middleware.DeleteBook(id);
            return RedirectToAction("Books");
        }

        public async Task<IActionResult> Rents()
        {
            var model = await _middleware.GetRentals();
            return View(model);
        }

        public IActionResult ChangePasswordOfficer()
        {
            ViewBag.message = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordOfficer(string currentPassword,string newPassword)
        {
            var userMail = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var result = await _middleware.ChangePassword(userMail,currentPassword, newPassword);
            if (result)
            {
                ViewBag.message = "Şifreniz başarıyla değiştirildi.";
            } else
            {
                ViewBag.message = "Bir hata oluştu. Lütfen girdiğiniz bilgileri kontrol ederek tekrar deneyin.";
            }
            return View();
        }
    }
}
