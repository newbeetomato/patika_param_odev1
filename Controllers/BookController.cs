using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApi.AddControllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        private static List<Book> BookList = new List<Book>() //static oluşturulan kitaplar
        {
                new Book{
                    Id=1,
                    Title="Lean Startup",
                    GenereId=1, //kişisel gelişim personal growth
                    PageCount=200,
                    PublishDate=new DateTime(2001,06,12)
                },
                new Book{
                    Id=2,
                    Title="Herland",
                    GenereId=2,
                    PageCount=200,
                    PublishDate=new DateTime(2012,08,08)
                },
                new Book{
                    Id=3,
                    Title="Dune",
                    GenereId=2,//science fiction
                    PageCount=200,
                    PublishDate=new DateTime(2023,12,12)
                }
        };

        [HttpGet]
        public List<Book> GetBooks([FromQuery] string? ShortType) //Get işlemi
        {

            
                List<Book> bookList;
                bookList = BookList.ToList<Book>();
                if (ShortType is not null)
                {

                    if (ShortType == "a-z")//A dan Z ye listeleme işlemi
                    {
                        bookList = BookList.OrderBy(x => x.Title).ToList<Book>();
                        return bookList;

                    }
                    if (ShortType == "z-a")//Z den A ye listeleme işlemi
                    {

                        bookList = BookList.OrderByDescending(x => x.Title).ToList<Book>();
                        return bookList;
                    }


                }
                return bookList;
           
            



        }

        [HttpGet("{id}")]
        public Book GetById([FromRoute] int id) //from route id ile get işlemi
        {

            
                var book = BookList.Where(book => book.Id == id).SingleOrDefault();//highorder
                if (book == null)
                {
                    return BookList.First();
                }
                return book;
           
           
            
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook) 
        {

            try
            {
                var book = BookList.SingleOrDefault(x => x.Title == newBook.Title);
                if (book != null)
                    return BadRequest();

                BookList.Add(newBook);

                return Created("201", newBook); // 201 kodu
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);

            }
            
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook( int id, [FromBody] Book updatedBook) //İd ye göre değişitirlen kısımları Update 
        {
            try
            {
                var book = BookList.SingleOrDefault(x => x.Id == id);
                if (book == null)
                    return NotFound(); // 404 kodu

                book.GenereId = updatedBook.GenereId != default ? updatedBook.GenereId : book.GenereId;
                // verisi varsa genreid yi update et yoksa kendi değerini kullan
                book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
                book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate;
                book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title;

                return Ok(); //200
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);

            }
            

        }

        [HttpPatch("{id}")]
        public IActionResult UpdateBookPatch( int id, [FromBody] JsonPatchDocument<Book> updatedBookPatch)
        {
            try
            {
                var book = BookList.SingleOrDefault(x => x.Id == id);
                if (book == null)
                { return NotFound(); }
                updatedBookPatch.ApplyTo(book, ModelState);
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
                var book = BookList.SingleOrDefault(x => x.Id == id);
                if (book == null)
                    return BadRequest(); // 400 Http durum kodu

                BookList.Remove(book);
                return Ok(); //200 durum kodu
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);

            }

        }
    }
}