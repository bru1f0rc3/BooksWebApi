using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Author;
using WebApplication2.Interfaces;

namespace WebApplication2.Controllers.Book
{
    /// <summary>
    /// Контроллер для управления авторами книг
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        /// <summary>
        /// Конструктор контроллера авторов
        /// </summary>
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDTO>>> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorById(id);
            if (author == null)
                return NotFound();
            return Ok(author);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthorDTO>> CreateAuthor(CreateAuthorDTO author)
        {
            var createdAuthor = await _authorService.CreateAuthor(author);
            return CreatedAtAction(nameof(GetAuthorById), new { createdAuthor.id }, createdAuthor);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthorDTO>> UpdateAuthor(int id, UpdateAuthorDTO author)
        {
            var updatedAuthor = await _authorService.UpdateAuthor(id, author);
            if (updatedAuthor == null)
                return NotFound();
            return Ok(updatedAuthor);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var result = await _authorService.DeleteAuthor(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
} 