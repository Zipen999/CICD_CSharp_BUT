using LibraryDTO;

namespace DtoAbstractLayer;

/// <summary>
/// abstract layer for requests on Books Library
/// </summary>
public interface IDtoManager
{
    /// <summary>
    /// get a book by specifying its id
    /// </summary>
    /// <param name="id">id of the Book to get</param>
    /// <returns>a Book with this id (or null if id is unknown)</returns>
    Task<BookDTO> GetBookById(string id);

    /// <summary>
    /// get a book by specifying its isbn
    /// </summary>
    /// <param name="isbn">isbn of the Book to get</param>
    /// <returns>a Book with this isbn (or null if isbn is unknown)</returns>
    Task<BookDTO> GetBookByISBN(string isbn);

    /// <summary>
    /// get books containing a substring in their titles
    /// </summary>
    /// <param name="title">the substring to look for in book titles</param>
    /// <param name="index">index of the page of resulting books</param>
    /// <param name="count">number of resulting books per page</param>
    /// <param name="sort">sort criterium (not mandatory):
    /// <ul>
    ///     <li>```title```: sort books by titles in alphabetical order,</li>
    ///     <li>```title_reverse```: sort books by titles in reverse alphabetical order,</li>
    ///     <li>```new```: sort books by publishing dates, beginning with the most recents,</li>
    ///     <li>```old```: sort books by publishing dates, beginning with the oldest</li>
    /// </ul>
    /// </param>
    /// <returns>max <i>count</i> books</returns>
    Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByTitle(string title, int index, int count, string sort = "");

    /// <summary>
    /// get books of a particular author by giving the author id
    /// </summary>
    /// <param name="authorId">the id of the author</param>
    /// <param name="index">index of the page of resulting books</param>
    /// <param name="count">number of resulting books per page</param>
    /// <param name="sort">sort criterium (not mandatory):
    /// <ul>
    ///     <li>```title```: sort books by titles in alphabetical order,</li>
    ///     <li>```title_reverse```: sort books by titles in reverse alphabetical order,</li>
    ///     <li>```new```: sort books by publishing dates, beginning with the most recents,</li>
    ///     <li>```old```: sort books by publishing dates, beginning with the oldest</li>
    /// </ul>
    /// </param>
    /// <returns>max <i>count</i> books</returns>
    Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthorId(string authorId, int index, int count, string sort = "");

    /// <summary>
    /// get books of authors whose name (or alternate names) contains a particular substring
    /// </summary>
    /// <param name="author">name to look for in author names or alternate names</param>
    /// <param name="index">index of the page of resulting books</param>
    /// <param name="count">number of resulting books per page</param>
    /// <param name="sort">sort criterium (not mandatory):
    /// <ul>
    ///     <li>```title```: sort books by titles in alphabetical order,</li>
    ///     <li>```title_reverse```: sort books by titles in reverse alphabetical order,</li>
    ///     <li>```new```: sort books by publishing dates, beginning with the most recents,</li>
    ///     <li>```old```: sort books by publishing dates, beginning with the oldest</li>
    /// </ul>
    /// </param>
    /// <returns>max <i>count</i> books</returns>
    Task<Tuple<long, IEnumerable<BookDTO>>> GetBooksByAuthor(string author, int index, int count, string sort = "");

    /// <summary>
    /// get an author by specifying its id
    /// </summary>
    /// <param name="id">id of the Author to get</param>
    /// <returns>an author with this id (or null if id is unknown)</returns>
    Task<AuthorDTO> GetAuthorById(string id);

    /// <summary>
    /// get authors containing a substring in their names (or alternate names)
    /// </summary>
    /// <param name="substring">the substring to look for in author names (or alternate names)</param>
    /// <param name="index">index of the page of resulting authors</param>
    /// <param name="count">number of resulting authors per page</param>
    /// <param name="sort">sort criterium (not mandatory):
    ///     <ul>
    ///         <li>```name```: sort authors by names in alphabetical order,</li>
    ///         <li>```name_reverse```: sort authors by names in reverse alphabetical order,</li>
    ///     </ul>
    /// </param>
    /// <returns>max <i>count</i> authors</returns>
    Task<Tuple<long, IEnumerable<AuthorDTO>>> GetAuthorsByName(string substring, int index, int count, string sort = "");
}

