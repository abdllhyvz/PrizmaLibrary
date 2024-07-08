using LibraryManagementService.DBModels;
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
    public class RentalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;


        public RentalController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Rentals.All());
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _unitOfWork.Rentals.FindById(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetByUser(string mail)
        {
            var rentals = await _unitOfWork.Rentals.ActiveByUser(mail);
            return Ok(rentals);
        }

        [HttpGet]
        public async Task<IActionResult> Return(int id,int bookid)
        {
            var rental = await _unitOfWork.Rentals.FindById(id);
            rental.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            var book = await _unitOfWork.Books.FindById(bookid);
            book.IsAvailable = true;
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rental rental)
        {
            var existingUser = await _userManager.FindByIdAsync(rental.User.Id);
            if (existingUser == null)
            {
                return BadRequest();
            }

            rental.User = existingUser;

            var existingBook = await _unitOfWork.Books.FindById(rental.Book.Id);
            if (existingBook == null)
            {
                return BadRequest();
            }

            rental.Book = existingBook;

            var result = await _unitOfWork.Rentals.Add(rental);
            if (result)
            {
                existingBook.IsAvailable = false;
                await _unitOfWork.SaveAsync();
                return Ok(rental);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetRents()
        {
            return Ok(await _unitOfWork.Rentals.GetRentsAllData());
        }

    }
}
