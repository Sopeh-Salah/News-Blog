using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlogAPI.Context;
using NewsBlogAPI.DTO;
using NewsBlogAPI.Models;

namespace NewsBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        newsBlogDatabase db;
        public AuthorsController(newsBlogDatabase context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult gettAll()
        {
            var authors = db.Authors.Include(d => d.news).ToList();
            if (authors == null)
            {
                return NotFound();
            }
            List<AuthorDTO> authorDTOs = authors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
            }).ToList();
            return Ok(authorDTOs);
        }
        [HttpPost]
        public IActionResult AddAuthor(AuthorDTO authorDTO)
        {
            if (authorDTO == null)
            {
                return BadRequest();
            }
            Author author = new Author
            {
                Name = authorDTO.Name
            };
            db.Authors.Add(author);
            db.SaveChanges();
            return Ok(author);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var author = db.Authors.Include(d => d.news).Where(a => a.Id == id).FirstOrDefault();
            if (author == null)
            {
                return NotFound();
            }
            AuthorDTO authorDTO = new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name
            };
            return Ok(authorDTO);
        }
    }
}
