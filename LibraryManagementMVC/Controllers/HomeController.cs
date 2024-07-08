using LibraryManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;

namespace LibraryManagementMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Middleware.Middleware _middleware;


        public HomeController(Middleware.Middleware middleware)
        {
            _middleware = middleware;
        }

            public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if(token == null)
            {
                return RedirectToAction("Logout","Login");
            }
            var roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim == null || string.IsNullOrEmpty(roleClaim.Value))
            {
                return RedirectToAction("Logout", "Login");
            }
            else if (roleClaim.Value == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (roleClaim.Value == "Officer")
            {
                return RedirectToAction("Index", "Officer");
            }
            else if (roleClaim.Value == "User")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Login");

            }
        }

        public async Task<IActionResult> SearchBook(string searchText,string filterType)
        {
            if (string.IsNullOrEmpty(searchText)) { 
                return RedirectToAction("Index");
            }
            var model = await _middleware.SearchBook(searchText, filterType);
            return PartialView("_BooksPartial",model);
        }

        public async Task<IActionResult> RentBook(int bookId)
        {
            var userMail = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var user = await _middleware.GetUserByMail(userMail);
            var book = await _middleware.GetBookById(bookId);
            Rental rental = new Rental { 
                RentDate = DateOnly.FromDateTime(DateTime.Now),
                //ReturnDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                Book = book,
                User = user
            };

            await _middleware.RentBook(rental);
            return Json("OK");
        }

        public async Task<IActionResult> ReturnBook()
        {
            var userMail = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var model = await _middleware.GetUserRentals(userMail);

            return View(model);
        }

        public async Task<IActionResult> Return(int id,int bookid)
        {
            await _middleware.Return(id,bookid);
            return RedirectToAction("ReturnBook");
        }

        public IActionResult ChangePasswordUser()
        {
            ViewBag.message = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordUser(string currentPassword, string newPassword)
        {
            var userMail = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var result = await _middleware.ChangePassword(userMail, currentPassword, newPassword);
            if (result)
            {
                ViewBag.message = "Þifreniz baþarýyla deðiþtirildi.";
            }
            else
            {
                ViewBag.message = "Bir hata oluþtu. Lütfen girdiðiniz bilgileri kontrol ederek tekrar deneyin.";
            }
            return View();
        }

    }
}
