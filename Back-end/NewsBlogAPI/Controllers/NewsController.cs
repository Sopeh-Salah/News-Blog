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
    public class NewsController : ControllerBase
    {
        newsBlogDatabase db;
        public NewsController(newsBlogDatabase context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult gettAll()
        {
            var news = db.News.Include(d => d.author).ToList();
            if (news == null)
            {
                return NotFound();
            }
            List<NewDTO> newsDTO = news.Select(n => new NewDTO
            {
                Id = n.Id,
                Title = n.Title,
                Image = n.Image,
                PublicationDate = n.PublicationDate,
                CreationDate = n.CreationDate,
                AuthorId = n.AuthorId,

            }).ToList();

            return Ok(newsDTO);
        }
        [HttpPost]
        public IActionResult AddNew(NewDTO news)
        {
            if (news == null)
            {
                return BadRequest();
            }
            New addednew = new New
            {
                AuthorId = news.AuthorId,
                CreationDate = DateTime.Now,
                PublicationDate = news.PublicationDate,
                Title = news.Title,
                Image = news.Image,
                author = db.Authors.Where(a => a.Id == news.AuthorId).FirstOrDefault()
            };
            if (addednew == null)
            {
                return BadRequest();
            }
            db.News.Add(addednew);
            db.SaveChanges();
            return Ok(addednew);
        }
        [HttpGet("{authorId:int}")]
        public IActionResult GetByAuthor(int authorId)
        {
            var Dnew = db.News.Include(d => d.author).Where(n => n.AuthorId == authorId).ToList();
            if (Dnew == null)
            {
                return NotFound();
            }
            List<NewDTO> newsDTO = Dnew.Select(n => new NewDTO
            {
                Id = n.Id,
                AuthorId = n.AuthorId,
                CreationDate = n.CreationDate,
                PublicationDate = n.PublicationDate,
                Title = n.Title,
                Image = n.Image,
            }).ToList();
            return Ok(newsDTO);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var deletednew = db.News.Include(d => d.author).Where(n => n.Id == id).FirstOrDefault();
            if (deletednew == null)
            {
                return BadRequest();
            }
            db.News.Remove(deletednew);
            db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IActionResult edit(NewDTO newDTO)
        {
            if (newDTO == null)
            {
                return BadRequest();
            }
            New editednew = new New
            {
                Id = newDTO.Id,
                AuthorId = newDTO.AuthorId,
                CreationDate = newDTO.CreationDate,
                Image = newDTO.Image,
                Title = newDTO.Title,
                PublicationDate = newDTO.PublicationDate,
                author = db.Authors.Where(a => a.Id == newDTO.AuthorId).FirstOrDefault(),
            };
            db.News.Update(editednew);
            db.SaveChanges();
            return Ok();
        }
    }
}
