// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using MyLibraryDB;
using StubbedDB;

//StubbedDTO.Stub.BasePath = Directory.GetCurrentDirectory();

using (var context = new MyLibraryStubbedContext())
{
    foreach(var a in context.Authors.Include(auth => auth.Works))
    {
        Console.WriteLine($"{a.Id} - {a.Name} (Works count: {a.Works.Count})");
    }
    foreach(var b in context.Books.Include(book => book.Authors))
    {
        Console.WriteLine($"{b.Id} - {b.Title} (Authors count: {b.Authors.Count})");
    }
}
