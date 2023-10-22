using System;
using System.Data.SqlTypes;
using System.Net;
using System.Text.Json;
using DtoAbstractLayer;
using JsonReader;
using LibraryDTO;
using static System.Net.WebRequestMethods;

namespace OpenLibraryClient;

public class OpenLibClientAPI : IDtoManager
{
    const string BasePath = @"https://openlibrary.org/";
    const string SearchAuthorPrefix = @"search/authors.json?q=";
    const string SearchBookTitlePrefix = @"search.json?title=";
    const string SearchBookByAuthorPrefix = @"search.json?author=";
    const string AuthorPrefix = @"authors/";
    const string BookPrefix = @"books/";
    const string IsbnPrefix = @"isbn/";
    HttpClient client = new HttpClient();
    JsonSerializerOptions SerializerOptions { get; set; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private async Task<T> GetElement<T>(string route, Func<string,T> deserializer)
    {
        try
        {
            T result = default(T);
            Uri uri = new Uri (route, UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await client.GetAsync (uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync ();
                result = deserializer(content);
            }
            return result;
        }
        catch(Exception exc)
        {
            throw new WebException($"The route {route} seems to be invalid");
        }
    }

    

    public async Task<AuthorDTO> GetAuthorById(string id)
    {
        string route = $"{BasePath}{AuthorPrefix}{id}.json";
        return await GetElement<AuthorDTO>(route, json => AuthorJsonReader.ReadAuthor(json));
    }

    public async Task<Tuple<long, IEnumerable<AuthorDTO>>> GetAuthorsByName(string substring, int index, int count, string sort = "")
    {
        string searchedString = substring.Trim().Replace(" ", "+");
        string route = $"{BasePath}{SearchAuthorPrefix}{searchedString}"
            .AddPagination(index, count)
            .AddSort(sort);
        return await GetElement<Tuple<long, IEnumerable<AuthorDTO>>>(route, json => AuthorJsonReader.GetAuthorsByName(json));
    }

    public async Task<BookDTO> GetBookById(string id)
    {
        string route = $"{BasePath}{BookPrefix}{id}.json";
        return await GetElement<BookDTO>(route, json => BookJsonReader.ReadBook(json));
    }

    public async Task<BookDTO> GetBookByISBN(string isbn)
    {
        string route = $"{BasePath}{IsbnPrefix}{isbn}.json";
        return await GetElement<BookDTO>(route, json => BookJsonReader.ReadBook(json));
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthor(string author, int index, int count, string sort = "")
    {
        string searchedString = author.Trim().Replace(" ", "+");
        string route = $"{BasePath}{SearchBookByAuthorPrefix}{searchedString}"
            .AddPagination(index, count)
            .AddSort(sort);
        return await GetElement<Tuple<long, IEnumerable<BookDTO>>>(route, json => BookJsonReader.GetBooksByAuthor(json));
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthorId(string authorId, int index, int count, string sort = "")
    {
        string searchedString = authorId.Trim().Replace(" ", "+");
        string route = $"{BasePath}{SearchBookByAuthorPrefix}{searchedString}"
            .AddPagination(index, count)
            .AddSort(sort);
        return await GetElement<Tuple<long, IEnumerable<BookDTO>>>(route, json => BookJsonReader.GetBooksByAuthor(json));
    }

    public async Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByTitle(string title, int index, int count, string sort = "")
    {
        string searchedString = title.Trim().Replace(" ", "+");
        string route = $"{BasePath}{SearchBookTitlePrefix}{searchedString}"
            .AddPagination(index, count)
            .AddSort(sort);
        return await GetElement<Tuple<long, IEnumerable<BookDTO>>>(route, json => BookJsonReader.GetBooksByTitle(json));
    }
}

