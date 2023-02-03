using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApi.AddControllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        private static List<Book> BookList = new List<Book>()
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
        public List<Book> GetBooks([FromQuery] string? SortType)
        {
            List<Book> bookList;
            bookList = BookList.ToList<Book>();
            if (SortType is not null)
            {

                if (SortType == "a-z")
                {
                    bookList = BookList.OrderBy(x => x.Id).ToList<Book>();
                    return bookList;

                }
                if (SortType == "z-a")
                {

                    bookList = BookList.OrderByDescending(x => x.Id).ToList<Book>();
                    return bookList;
                }


            }
            return bookList;



        }

        [HttpGet("{id}")]
        public Book GetById([FromRoute] int id) //from route
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
            var book = BookList.SingleOrDefault(x => x.Title == newBook.Title);
            if (book != null)
                return BadRequest();

            BookList.Add(newBook);

            return Created("201", newBook);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook( int id, [FromBody] Book updatedBook)
        {
            var book = BookList.SingleOrDefault(x => x.Id == id);
            if (book == null)
                return NotFound();

            book.GenereId = updatedBook.GenereId != default ? updatedBook.GenereId : book.GenereId;
            // verisi varsa genreid yi update et yoksa kendi değerini kullan
            book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
            book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate;
            book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title;

            return Ok();

        }

        [HttpPatch("{id}")]
        public IActionResult UpdateBookPatch( int id, [FromBody] JsonPatchDocument<Book> updatedBookPatch)
        {
            var book=BookList.SingleOrDefault(x=>x.Id==id);
            if(book!=null)
             {return NotFound(); }
            updatedBookPatch.ApplyTo(book,ModelState);
            return Ok(book);


        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = BookList.SingleOrDefault(x => x.Id == id);
            if (book == null)
                return BadRequest();

            BookList.Remove(book);
            return Ok();
        }
    }
}