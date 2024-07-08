using LibraryManagementService.Entities;
using LibraryManagementService.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementService.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Books.All());
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _unitOfWork.Books.FindById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            var result = await _unitOfWork.Books.Add(book);
            if (result)
            {
                await _unitOfWork.SaveAsync();
                return Ok(book);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _unitOfWork.Books.FindById(id);
            var result = await _unitOfWork.Books.Delete(book);
            if (result) { 
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Book book)
        {
            var result = await _unitOfWork.Books.Update(book);
            if (result)
            {
                await _unitOfWork.SaveAsync();
                return Ok(book);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> SearchByTitle(string title)
        {
            var books = await _unitOfWork.Books.SearchByTitle(title);
            return Ok(books);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByAuthor(string author)
        {
            var books = await _unitOfWork.Books.SearchByAuthor(author);
            return Ok(books);
        }
    }
}
