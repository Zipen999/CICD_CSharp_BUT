using System.Data.SqlTypes;
using DtoAbstractLayer;
using LibraryDTO;
using Microsoft.EntityFrameworkCore;
using MyLibraryDB;
using MyLibraryEntities;
using StubbedDB;

namespace MyLibraryManager;

public class MyLibraryMgr : IDtoManager
{
    protected MyLibraryContext Context => _dbContext;
    private readonly MyLibraryContext _dbContext;

    public MyLibraryMgr() : this(new MyLibraryStubbedContext())
    {
    }

    public MyLibraryMgr(string dbPlatformPath)
        : this(new MyLibraryStubbedContext(dbPlatformPath))
    {
        Context.Database.EnsureCreated();
    }

    internal MyLibraryMgr(MyLibraryStubbedContext context)
    {
        _dbContext = context;
    }

    public async Task<AuthorDTO> GetAuthorById(string id)
    {
        var author = await Context.Authors.SingleOrDefaultAsync(a => a.Id.ToUpper().Contains(id.ToUpper()));
        return author.ToDto();
    }

    public async Task<Tuple<long, IEnumerable<AuthorDTO>>> GetAuthorsByName(string substring, int index, int count, string sort = "")
    {
        var authors = Context.Authors.Where(a => a.Name.ToUpper().Contains(substring.ToUpper()));
        switch(sort)
        {
            case "name":
                authors = authors.OrderBy(a => a.Name);
                break;
            case "name_reverse":
                authors = authors.OrderByDescending(a => a.Name);
                break;
            default:
                break;
        }

        long nb = await authors.CountAsync();

        return await Task.FromResult(Tuple.Create(nb, authors.Skip(index*count).Take(count).ToDtos()));
    }

    public async Task<BookDTO> GetBookById(string id)
    {
        var book = await Context.Books.SingleOrDefaultAsync(b => b.Id.ToUpper().Contains(id.ToUpper()));
        return book.ToDto();
    }

    public async Task<BookDTO> GetBookByISBN(string isbn)
    {
        var book = await Context.Books.SingleOrDefaultAsync(b => b.ISBN13 == isbn);
        return book?.ToDto();
    }

    Func<AuthorEntity, string, bool> predicateAuthorName =(a, s) => a.Name.ToUpper().Contains(s.ToUpper());
                                                                //|| a.AlternateNames.Count(an => an.ToUpper().Contains(s.ToUpper())) > 0;

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthor(string author, int index, int count, string sort = "")
    {
        var books2 = Context.Books.SelectMany(b => b.Authors, (b, a) => new {Book = b, AuthorName = a.Name });
        var books3 = books2.Where(ba => ba.AuthorName.ToUpper().Contains(author.ToUpper()));
        var books4 = books3.Select(ba => ba.Book).Distinct();
//        var books = Context.Books.Where(b => b.Authors.Where(a => a.Name.ToUpper().Contains(author.ToUpper())).Count() > 0);// Exists(a => predicateAuthorName(a, author)));

        return await SortAndFilterBooks(books4, index, count, sort);
    }

    private async Task<Tuple<long, IEnumerable<BookDTO>>> SortAndFilterBooks(IQueryable<BookEntity>? books, int index, int count, string sort = "")
    {
        switch(sort)
        {
            case "title":
                books = books.OrderBy(a => a.Title);
                break;
            case "title_reverse":
                books = books.OrderByDescending(a => a.Title);
                break;
            case "new":
                books = books.OrderByDescending(a => a.PublishDate);
                break;
            case "old":
                books = books.OrderBy(a => a.PublishDate);
                break;
            default:
                break;
        }

        long nb = await books.CountAsync();

        return await Task.FromResult(Tuple.Create(nb, books.Skip(index*count).Take(count).ToDtos()));
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthorId(string authorId, int index, int count, string sort = "")
    {
        //var books = Context.Books.Where(b => b.Authors.Count(a => a.Id == authorId) > 0);
        var books2 = Context.Books.SelectMany(b => b.Authors, (b, a) => new {Book = b, AuthorId = a.Id });
        var books3 = books2.Where(ba => ba.AuthorId.ToUpper().Contains(authorId.ToUpper()));
        var books4 = books3.Select(ba => ba.Book).Distinct();

        return await SortAndFilterBooks(books4, index, count, sort);
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByTitle(string title, int index, int count, string sort = "")
    {
        var books = Context.Books.Where(b => b.Title.ToUpper().Contains(title.ToUpper()));

        return await SortAndFilterBooks(books, index, count, sort);
    }
}

