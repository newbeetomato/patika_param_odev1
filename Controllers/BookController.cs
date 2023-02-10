using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApi.DBOperations;

namespace WebApi.AddControllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public BookController (BookStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetBooks([FromQuery] string? ShortType) //Get işlemi
        {

            try
            {
                List<Book> bookList;
                bookList = _context.Books.ToList<Book>();
                if (ShortType is not null)
                {

                    if (ShortType == "a-z")//A dan Z ye listeleme işlemi
                    {
                        bookList = _context.Books.OrderBy(x => x.Title).ToList<Book>();
                        return Ok(bookList);

                    }
                    if (ShortType == "z-a")//Z den A ye listeleme işlemi
                    {

                        bookList = _context.Books.OrderByDescending(x => x.Title).ToList<Book>();
                        return Ok(bookList);
                    }
                    return BadRequest();

                }
                return Ok(bookList);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);

            }




        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) //from route id ile get işlemi
        {

            try
            {
                var book = _context.Books.Where(x => x.Id == id).SingleOrDefault();//highorder
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }





        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {

            try
            {
                var book = _context.Books.SingleOrDefault(x => x.Title == newBook.Title);
                if (book != null)
                    return BadRequest();

                _context.Books.Add(newBook);
                _context.SaveChanges();


                return Created("201", newBook); // 201 kodu
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);

            }

        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook) //İd ye göre değişitirlen kısımları Update 
        {
            try
            {
                var book = _context.Books.SingleOrDefault(x => x.Id == id);
                if (book == null)
                    return NotFound(); // 404 kodu

                book.GenereId = updatedBook.GenereId != default ? updatedBook.GenereId : book.GenereId;
                // verisi varsa genreid yi update et yoksa kendi değerini kullan
                book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
                book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate;
                book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title;
                _context.SaveChanges();

                return Ok(book); //200
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }

        /* patch için aşşagıdaki yolla title değerini istediğimiz value ile update edebilirz
          [
            {
             "value":"yeni değer",
             "path":"Title",
             "op":"replace"
            }
            ]
         */
        [HttpPatch("{id}")] 
        
        public IActionResult UpdateBookPatch(int id, [FromBody] JsonPatchDocument<Book> updatedBookPatch)
        {
            try
            {
                var book = _context.Books.SingleOrDefault(x => x.Id == id);
                if (book == null)
                { return NotFound(); }
                updatedBookPatch.ApplyTo(book, ModelState);
                _context.SaveChanges();

                return Ok(book);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }



        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id) 
        {
            try
            {
                var book = _context.Books.SingleOrDefault(x => x.Id == id);
                if (book == null)
                    return BadRequest(); // 400 Http durum kodu

                _context.Books.Remove(book);
                _context.SaveChanges();

                return Ok(); //200 durum kodu
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);

            }

        }
    }
}