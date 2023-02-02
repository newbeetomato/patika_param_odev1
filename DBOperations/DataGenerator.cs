
/*using Microsoft.EntityFrameworkCore;

namespace WebApi.DBOperations
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                {
                    return;
                }
                context.Books.AddRange(
                    new Book
                    {
                        Id = 1,
                        Title = "Lean Startup",
                        GenereId = 1, //kişisel gelişim personal growth
                        PageCount = 200,
                        PublishDate = new DateTime(2001, 06, 12)
                    },
                new Book
                {
                    Id = 2,
                    Title = "Herland",
                    GenereId = 2,
                    PageCount = 200,
                    PublishDate = new DateTime(2012, 08, 08)
                },
                new Book
                {
                    Id = 3,
                    Title = "Dune",
                    GenereId = 2,//science fiction
                    PageCount = 200,
                    PublishDate = new DateTime(2023, 12, 12)
                }
                );
                context.SaveChanges();

            }
        }
    }
}*/