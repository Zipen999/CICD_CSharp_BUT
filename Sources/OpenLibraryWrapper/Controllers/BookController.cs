using System;
using DtoAbstractLayer;
using LibraryDTO;
using Microsoft.AspNetCore.Mvc;
using StubbedDTO;

namespace OpenLibraryWrapper.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BookController : ControllerBase
	{
        private readonly ILogger<BookController> _logger;

        private IDtoManager DtoManager;

        public BookController(ILogger<BookController> logger, IDtoManager dtoManager)
        {
            _logger = logger;
            DtoManager = dtoManager;
        }

        /// <summary>
        /// Gets books of the collection matching a particular title
        /// </summary>
        /// <param name="title">part of a title to look for in book titles (case is ignored)</param>
        /// <param name="index">index of the page</param>
        /// <param name="count">number of elements per page</param>
        /// <param name="sort">sort criterium of the resulting books:
        ///     <ul>
        ///         <li>```title```: sort books by titles in alphabetical order,</li>
        ///         <li>```title_reverse```: sort books by titles in reverse alphabetical order,</li>
        ///         <li>```new```: sort books by publishing dates, beginning with the most recents,</li>
        ///         <li>```old```: sort books by publishing dates, beginning with the oldest</li>
        ///     </ul>
        ///     
        /// </param>
        /// <returns>a collection of count (or less) books</returns>
        /// <remarks>
        /// Sample requests:
        ///
        ///     book/getbooksbytitle?title=ne&amp;index=0&amp;count=5
        ///     book/getbooksbytitle?title=ne&amp;index=0&amp;count=5&amp;sort=old
        ///
        /// </remarks>
        /// <response code="200">Returns count books at page index</response>
        /// <response code="404">no books within this range</response>
        [HttpGet("getBooksByTitle")]
        [ProducesResponseType(typeof(Tuple<long, IEnumerable<BookDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooksByTitle([FromQuery] string title, [FromQuery] int index, [FromQuery] int count, [FromQuery] string sort = "")
        {
            _logger.LogDebug("Get books by title");
            
            var booksDto = (await DtoManager.GetBooksByTitle(title, index, count, sort));
            _logger.LogInformation($"{booksDto.Item1} books found");
            if(booksDto.Item1 == 0)
            {
                return NotFound();
            }
            return Ok(booksDto);
        }

        /// <summary>
        /// Gets books of the collection whose of one the authors is matching a particular name
        /// </summary>
        /// <param name="name">part of a author name to look for in book authors (case is ignored)</param>
        /// <param name="index">index of the page</param>
        /// <param name="count">number of elements per page</param>
        /// <param name="sort">sort criterium of the resulting books:
        ///     <ul>
        ///         <li>```title```: sort books by titles in alphabetical order,</li>
        ///         <li>```title_reverse```: sort books by titles in reverse alphabetical order,</li>
        ///         <li>```new```: sort books by publishing dates, beginning with the most recents,</li>
        ///         <li>```old```: sort books by publishing dates, beginning with the oldest</li>
        ///     </ul>
        ///     
        /// </param>
        /// <returns>a collection of count (or less) books</returns>
        /// <remarks>
        /// Sample requests:
        ///
        ///     book/getbooksbyauthor?name=al&amp;index=0&amp;count=5
        ///     book/getbooksbyauthor?name=al&amp;index=0&amp;count=5&amp;sort=old
        ///     
        /// <b>Note:</b>
        /// <i>name is also looked for in alternate names of the authors</i>
        /// </remarks>
        /// <response code="200">Returns count books at page index</response>
        /// <response code="404">no books within this range</response>
        [HttpGet("getBooksByAuthor")]
        [ProducesResponseType(typeof(Tuple<long, IEnumerable<BookDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooksByAuthor([FromQuery] string name, [FromQuery] int index, [FromQuery] int count, [FromQuery] string sort = "")
        {
            _logger.LogDebug("Get books by author");
            
            var booksDto = (await DtoManager.GetBooksByAuthor(name, index, count, sort));
            _logger.LogInformation($"{booksDto.Item1} books found");
            if(booksDto.Item1 == 0)
            {
                return NotFound();
            }
            return Ok(booksDto);
        }

        /// <summary>
        /// Gets books of the collection of a particular author
        /// </summary>
        /// <param name="id">id of the author</param>
        /// <param name="index">index of the page</param>
        /// <param name="count">number of elements per page</param>
        /// <param name="sort">sort criterium of the resulting books:
        ///     <ul>
        ///         <li>```title```: sort books by titles in alphabetical order,</li>
        ///         <li>```title_reverse```: sort books by titles in reverse alphabetical order,</li>
        ///         <li>```new```: sort books by publishing dates, beginning with the most recents,</li>
        ///         <li>```old```: sort books by publishing dates, beginning with the oldest</li>
        ///     </ul>
        ///     
        /// </param>
        /// <returns>a collection of count (or less) books</returns>
        /// <remarks>
        /// Sample requests:
        ///
        ///     book/getbooksbyauthorid?id=OL1846639A&amp;index=0&amp;count=5
        ///     book/getbooksbyauthorid?id=OL1846639A&amp;index=0&amp;count=5&amp;sort=old
        ///     
        /// </remarks>
        /// <response code="200">Returns count books at page index</response>
        /// <response code="404">no books within this range</response>
        [HttpGet("getBooksByAuthorId")]
        [ProducesResponseType(typeof(Tuple<long, IEnumerable<BookDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooksByAuthorId([FromQuery] string id, [FromQuery] int index, [FromQuery] int count, [FromQuery] string sort = "")
        {
            _logger.LogDebug("Get books by author id");
            
            var booksDto = (await DtoManager.GetBooksByAuthorId(id, index, count, sort));
            _logger.LogInformation($"{booksDto.Item1} books found");
            if(booksDto.Item1 == 0)
            {
                return NotFound();
            }
            return Ok(booksDto);
        }

        /// <summary>
        /// Gets authors of the collection matching a particular name
        /// </summary>
        /// <param name="name">name to look for in author names</param>
        /// <param name="index">index of the page</param>
        /// <param name="count">number of elements per page</param>
        /// <param name="sort">sort criterium of the resulting authors:
        ///     <ul>
        ///         <li>```name```: sort authors by names in alphabetical order,</li>
        ///         <li>```name_reverse```: sort authors by names in reverse alphabetical order,</li>
        ///     </ul>
        ///     
        /// </param>
        /// <returns>a collection of count (or less) authors</returns>
        /// <remarks>
        /// Sample requests:
        ///
        ///     book/getauthorsbyname?name=al&amp;index=0&amp;count=5
        ///     book/getauthorsbyname?name=al&amp;index=0&amp;count=5&amp;sort=name
        ///
        /// </remarks>
        /// <response code="200">Returns count authors at page index</response>
        /// <response code="404">no authors within this range</response>
        [HttpGet("getAuthorsByName")]
        [ProducesResponseType(typeof(Tuple<long, IEnumerable<AuthorDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorsByName([FromQuery] string name, [FromQuery] int index, [FromQuery] int count, [FromQuery] string sort = "")
        {
            _logger.LogDebug("Get authors by name");
            var authorsDto = (await DtoManager.GetAuthorsByName(name, index, count, sort));
            _logger.LogInformation($"{authorsDto.Item1} authors found");
            if(authorsDto.Item1 == 0)
            {
                return NotFound();
            }
            return Ok(authorsDto);
        }

        /// <summary>
        /// Gets book by isbn
        /// </summary>
        /// <param name="isbn">isbn of the book to get</param>
        /// <returns>the book with the seeked isbn (or null)</returns>
        /// <remarks>
        /// Sample requests:
        ///
        ///     book/getBookByIsbn/9782330033118
        ///
        /// </remarks>
        /// <response code="200">Returns the book with this isbn</response>
        /// <response code="404">no book found</response>
        [HttpGet("getBookByIsbn/{isbn?}")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookByIsbn(string isbn)
        {
            _logger.LogDebug("Get book by isbn");
            var bookDto = (await DtoManager.GetBookByISBN(isbn));
            if(bookDto == null)
            {
                _logger.LogInformation($"{isbn} not found");
                return NotFound();
            }
            _logger.LogInformation($"{bookDto.Title} found");
            return Ok(bookDto);
        }

        /// <summary>
        /// Gets book by id
        /// </summary>
        /// <param name="id">id of the book to get</param>
        /// <returns>the book with the seeked id (or null)</returns>
        /// <remarks>
        /// Sample requests:
        ///
        ///     book/getBookById/OL25910297M
        ///
        /// </remarks>
        /// <response code="200">Returns the book with this id</response>
        /// <response code="404">no book found</response>
        [HttpGet("getBookById/{id?}")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(string id)
        {
            _logger.LogDebug("Get book by ID");
            var bookDto = (await DtoManager.GetBookById(id));
            if(bookDto == null)
            {
                _logger.LogInformation($"{id} not found");
                return NotFound();
            }
            _logger.LogInformation($"{bookDto.Title} found");
            return Ok(bookDto);
        }

        /// <summary>
        /// Gets author by id
        /// </summary>
        /// <param name="id">id of the author to get</param>
        /// <returns>the author with the seeked id (or null)</returns>
        /// <remarks>
        /// Sample requests:
        ///
        ///     book/getAuthorById/OL1846639A
        ///
        /// </remarks>
        /// <response code="200">Returns the author with this id</response>
        /// <response code="404">no author found</response>
        [HttpGet("getAuthorById/{id?}")]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById(string id)
        {
            _logger.LogDebug("Get Author by ID");
            var authorDTO = (await DtoManager.GetAuthorById(id));
            if(authorDTO == null)
            {
                _logger.LogInformation($"{id} not found");
                return NotFound();
            }
            _logger.LogInformation($"{authorDTO.Name} found");
            return Ok(authorDTO);
        }
	}
}

