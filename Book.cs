
using System;
namespace WebApi
{
public class Book
{
    public int Id { get; set; }
    public string ?Title { get; set; }
    public int GenereId { get; set; }
    public int PageCount { get; set; }
    public DateTime PublishDate { get; set; }
}
}