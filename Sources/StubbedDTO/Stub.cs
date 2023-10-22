using System.Data.SqlTypes;
using DtoAbstractLayer;
using JsonReader;
using LibraryDTO;
using static System.Reflection.Metadata.BlobBuilder;

namespace StubbedDTO;

public class Stub : IDtoManager
{
    public static List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();

    public static List<BookDTO> Books { get; set; } = new List<BookDTO>();

    public static List<WorkDTO> Works { get; set; } = new List<WorkDTO>();

    public static string BasePath { get; set; } = "";

    static Stub()
    {
        foreach(var fileAuthor in new DirectoryInfo($"{BasePath}authors/").GetFiles())
        { 
            using(StreamReader reader = File.OpenText(fileAuthor.FullName))
            {
                Authors.Add(AuthorJsonReader.ReadAuthor(reader.ReadToEnd()));
            }
        }

        foreach(var fileWork in new DirectoryInfo($"{BasePath}works/").GetFiles())
        {
            var ratingsFile = $"{BasePath}ratings/{fileWork.Name.Insert((int)(fileWork.Name.Length - fileWork.Extension.Length), ".ratings")}";
            using(StreamReader reader = File.OpenText(fileWork.FullName))
            using(StreamReader readerRatings = File.OpenText(ratingsFile))
            {
                var work = WorkJsonReader.ReadWork(reader.ReadToEnd(), readerRatings.ReadToEnd());
                if(work.Authors != null)
                foreach(var author in work.Authors.ToList())
                {
                    var newAuthor = Authors.SingleOrDefault(a => a.Id == author.Id);
                    work.Authors.Remove(author);
                    work.Authors.Add(newAuthor);
                }
                Works.Add(work);
            }
        }

        foreach(var fileBook in new DirectoryInfo($"{BasePath}books/").GetFiles())
        { 
            using(StreamReader reader = File.OpenText(fileBook.FullName))
            {
                var book = BookJsonReader.ReadBook(reader.ReadToEnd());
                foreach(var author in book.Authors.ToList())
                {
                    var newAuthor = Authors.SingleOrDefault(a => a.Id == author.Id);
                    book.Authors.Remove(author);
                    book.Authors.Add(newAuthor);
                }
                foreach(var work in book.Works.ToList())
                {
                    var newWork = Works.SingleOrDefault(w => w.Id == work.Id);
                    book.Works.Remove(work);
                    book.Works.Add(newWork);
                }
                Books.Add(book);
            }
        }
        
    }

    public Task<AuthorDTO> GetAuthorById(string id)
    {
        var author = Stub.Authors.SingleOrDefault(a => a.Id.Contains(id));
        return Task.FromResult(author);
    }

    private Task<Tuple<long, IEnumerable<AuthorDTO>>> OrderAuthors(IEnumerable<AuthorDTO> authors, int index, int count, string sort = "")
    {
        switch(sort)
        {
            case "name":
                authors = authors.OrderBy(a => a.Name);
                break;
            case "name_reverse":
                authors = authors.OrderByDescending(a => a.Name);
                break;
        }
        return Task.FromResult(Tuple.Create((long)authors.Count(), authors.Skip(index*count).Take(count)));
    }

    public async Task<Tuple<long, IEnumerable<AuthorDTO>>> GetAuthors(int index, int count, string sort = "")
    {
        IEnumerable<AuthorDTO> authors = Stub.Authors;
        return await OrderAuthors(authors, index, count, sort);
    }

    public async Task<Tuple<long, IEnumerable<AuthorDTO>>> GetAuthorsByName(string name, int index, int count, string sort = "")
    {
        var authors = Stub.Authors.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
                                            || a.AlternateNames.Exists(alt => alt.Contains(name, StringComparison.OrdinalIgnoreCase)));
        return await OrderAuthors(authors, index, count, sort);
    }

    public Task<BookDTO> GetBookById(string id)
    {
        var book = Stub.Books.SingleOrDefault(b => b.Id.Contains(id));
        return Task.FromResult(book);
    }

    private Task<Tuple<long, IEnumerable<BookDTO>>> OrderBooks(IEnumerable<BookDTO> books, int index, int count, string sort = "")
    {
        switch(sort)
        {
            case "title":
                books = books.OrderBy(b => b.Title);
                break;
            case "title_reverse":
                books = books.OrderByDescending(b => b.Title);
                break;
            case "new":
                books = books.OrderByDescending(b => b.PublishDate);
                break;
            case "old":
                books = books.OrderBy(b => b.PublishDate);
                break;

        }
        return Task.FromResult(Tuple.Create((long)books.Count(), books.Skip(index*count).Take(count)));
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooks(int index, int count, string sort = "")
    {
        var books = Stub.Books;
        return await OrderBooks(books, index, count, sort);
    }

    public Task<BookDTO> GetBookByISBN(string isbn)
    {
        var book = Stub.Books.SingleOrDefault(b => b.ISBN13.Equals(isbn, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(book);
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByTitle(string title, int index, int count, string sort = "")
    {
        var books = Stub.Books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)
                                    || b.Series.Exists(s => s.Contains(title, StringComparison.OrdinalIgnoreCase)));
        return await OrderBooks(books, index, count, sort);
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthorId(string authorId, int index, int count, string sort = "")
    {
        var books = Stub.Books.Where(b => b.Authors.Exists(a => a.Id.Contains(authorId))
                                        || b.Works.Exists(w => w.Authors.Exists(a => a.Id.Contains(authorId))));
        return await OrderBooks(books, index, count, sort);
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthor(string name, int index, int count, string sort = "")
    {
        var books = Stub.Books.Where(b => ContainsAuthorName(b, name));
        return await OrderBooks(books, index, count, sort);
    }

    private bool ContainsAuthorName(BookDTO book, string name)
    {
        IEnumerable<AuthorDTO> authors = new List<AuthorDTO>();

        if(book.Authors != null && book.Authors.Count > 0)
        {
            authors = authors.Union(book.Authors);
        }
        if(book.Works != null)
        {
            var worksAuthors = book.Works.SelectMany(w => w.Authors).ToList();
            if(worksAuthors.Count > 0)
                authors = authors.Union(worksAuthors);
        }
        foreach(var author in authors)
        {
            if(author.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
                || author.AlternateNames.Exists(alt => alt.Contains(name, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }
        return false;
    }

    public Task<Tuple<long, IEnumerable<WorkDTO>>> GetWorks(int index, int count)
    {
        long nbWorks = Stub.Works.Count;
        var works = Stub.Works.Skip(index*count).Take(count);
        return Task.FromResult(Tuple.Create(nbWorks, works));
    }

    public Task<long> GetNbAuthors()
        => Task.FromResult((long)Stub.Authors.Count);

    public Task<long> GetNbBooks()
        => Task.FromResult((long)Stub.Books.Count);

    public Task<long> GetNbWorks()
        => Task.FromResult((long)Stub.Works.Count);
}

