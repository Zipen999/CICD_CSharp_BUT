using System.Globalization;
using LibraryDTO;
using Newtonsoft.Json.Linq;

namespace JsonReader;

public static class BookJsonReader
{
    static Dictionary<string, Languages> languages = new Dictionary<string, Languages>()
    {
        [@"/languages/fre"] = Languages.French,
        [@"/languages/eng"] = Languages.English,
        ["fre"] = Languages.French,
        ["eng"] = Languages.English,
        [""] = Languages.Unknown
    };

    public static BookDTO ReadBook(string json)
    {
        JObject o = JObject.Parse(json);
        var l = o["languages"]?.FirstOrDefault("");
        Languages lang = l != null ? languages[(string)l["key"]] : Languages.Unknown;
        //List<Tuple<string, CultureInfo>> pubDateFormat =new List<Tuple<string, CultureInfo>>()
        //{
        //    Tuple.Create("d MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR")),
        //    Tuple.Create("MMM dd, yyyy", CultureInfo.InvariantCulture)
        //};

        //DateTime? publishDate = null;
        //foreach(var format in pubDateFormat)
        //{
        //    if(DateTime.TryParseExact((string)o["publish_date"], format.Item1, format.Item2, DateTimeStyles.None, out DateTime readDate))
        //    {
        //        publishDate = readDate;
        //        break;
        //    }
        //}
        //if(!publishDate.HasValue)
        //{
        //    publishDate = new DateTime((int)o["publish_date"], 12, 31);
        //}
        DateTime? publishDate = AuthorJsonReader.ReadDate((string)o["publish_date"]);

        BookDTO book = new BookDTO
        {
            Id = (string)o["key"],
            Title = (string)o["title"],
            Publishers = o["publishers"].Select(p => (string)p).ToList(),
            PublishDate = publishDate.GetValueOrDefault(DateTime.Now),
            ISBN13 = (string)o["isbn_13"][0],
            NbPages = o["number_of_pages"] != null ? (int)o["number_of_pages"] : -1,
            Language  = lang,
            Format = o.TryGetValue("physical_format", out JToken? f) ? (string)f : null,
            Works = o["works"].Select(w => new WorkDTO { Id = (string)w["key"] }).ToList(),
            Contributors = o.TryGetValue("contributors", out JToken? contr) ? contr.Select(c => new ContributorDTO { Name = (string)c["name"], Role = (string)c["role"] }).ToList() : new List<ContributorDTO>(),
            Authors = o["authors"]?.Select(a => new AuthorDTO { Id = (string)a["key"] }).ToList()
        };
        if(book.Authors == null)
        {
            book.Authors = new List<AuthorDTO>();
        }
        return book;
    }

    public static Tuple<long, IEnumerable<BookDTO>> GetBooksByAuthor(string json)
    {
        JObject o = JObject.Parse(json);
        long numFound = (long)o["numFound"];
        var books = o["docs"].Select(doc => new BookDTO
        {
            Id = (string)(doc["seed"].First()),
            Title = (string)doc["title"],
            ISBN13 = (string)(doc["isbn"].First()),
            Authors = doc["seed"].Where(s => ((string)s).StartsWith("/authors/"))
                                 .Select(s => new AuthorDTO { Id = (string)s }).ToList(),
            Language = languages.GetValueOrDefault((string)(doc["language"].First()))
        });
        return Tuple.Create(numFound, books);
    }

    public static Tuple<long, IEnumerable<BookDTO>> GetBooksByTitle(string json)
    {
        JObject o = JObject.Parse(json);
        long numFound = (long)o["numFound"];
        var books = o["docs"].Select(doc => new BookDTO
        {
            Id = (string)(doc["seed"].First()),
            Title = (string)doc["title"],
            ISBN13 = (string)(doc["isbn"].First()),
            Authors = doc["seed"].Where(s => ((string)s).StartsWith("/authors/"))
                                 .Select(s => new AuthorDTO { Id = (string)s }).ToList(),
            Language = languages.GetValueOrDefault((string)(doc["language"].First()))
        });
        return Tuple.Create(numFound, books);
    }
}

