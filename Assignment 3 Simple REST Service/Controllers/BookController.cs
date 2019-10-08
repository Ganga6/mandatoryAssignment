using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookLib;

namespace Assignment_3_Simple_REST_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private List<Book> BooksList = new List<Book>
        {
            new Book("Book Title", "Author Name", 200, "1111111111111"),
            new Book("Book Title 1", "Author Name 1", 21, "1111111111112"),
            new Book("Book Title 2", "Author Name 2", 440, "1111111111123"),
            new Book("Book Title 3", "Author Name 3", 10, "1111111111234"),
            new Book("Book Title 4", "Author Name 4", 800, "1111111112345"),
            new Book("Book Title 5", "Author Name 5", 80, "1111111123456"),
        };
        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Book> Get() => BooksList;

        [HttpGet("{id}")]
        public ActionResult<Book> Get(string id) => BooksList.Where(x => x.ISBN13 == id).FirstOrDefault();

        [HttpPost]
        public ActionResult Post([FromBody] Book value)
        {
            var book = BooksList.Where(x => x.ISBN13 == value.ISBN13).FirstOrDefault();
            if(book != null)
            {
                return BadRequest("Book with that ISBN already exists");
            }
            BooksList.Add(value);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Book value)
        {
            var book = BooksList.Where(x => x.ISBN13 == value.ISBN13).FirstOrDefault();
            if (book != null)
            {
                book = value;
                return Ok();
            } 
            else 
            {
                return BadRequest("Book with that ISBN already exists");
            }

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var book = BooksList.Where(x => x.ISBN13 == id).FirstOrDefault();
            if (book == null)
            {
                BooksList.Remove(book);
                return Ok();
            }
            else
            {
                return BadRequest("Book with that ISBN deosn't exist");
            }
        }
    }
}
