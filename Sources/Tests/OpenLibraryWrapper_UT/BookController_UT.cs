using DtoAbstractLayer;
using LibraryDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OpenLibraryClient;
using OpenLibraryWrapper.Controllers;
using StubbedDTO;

namespace OpenLibraryWrapper_UT;

public class BookController_UT
{
    private readonly BookController controller;
    private readonly IDtoManager dtoManager = new Stub();

    public BookController_UT()
    {
        var logger = new NullLogger<BookController>();
        controller = new BookController(logger, dtoManager);
    }

    [Theory]
    [InlineData(true, "L'\u00c9veil du L\u00e9viathan", "9782330033118")]
    [InlineData(false, null, "1782330033118")]
    public async void TestGetBookByIsbn(bool expectedResult, string expectedTitle, string isbn)
    {
        var result = await controller.GetBookByIsbn(isbn);
        Assert.Equal(expectedResult, result is OkObjectResult);
        if(result is not OkObjectResult)
        {
            return;
        }
        var okResult = result as OkObjectResult;
        var bookDto = okResult.Value as BookDTO;
        Assert.Equal(expectedTitle, bookDto.Title);
    }

    [Theory]
    [InlineData(true, "L'\u00c9veil du L\u00e9viathan", "OL25910297M")]
    [InlineData(false, null, "OL25910xxxM")]
    public async void TestGetBookById(bool expectedResult, string expectedTitle, string id)
    {
        var result = await controller.GetBookById(id);
        Assert.Equal(expectedResult, result is OkObjectResult);
        if(result is not OkObjectResult)
        {
            return;
        }
        var okResult = result as OkObjectResult;
        var bookDto = okResult.Value as BookDTO;
        Assert.Equal(expectedTitle, bookDto.Title);
    }

    [Fact]
    public async void TestGetBooksByTitle()
    {
        var result = await controller.GetBooksByTitle("ne", 0, 5);
        var okResult = result as OkObjectResult;
        var booksTupple = (Tuple<long, IEnumerable<BookDTO>>)okResult.Value;
        long nbBooks = booksTupple.Item1;
        var books = booksTupple.Item2;

        Assert.True(nbBooks > 0);
    }

    [Fact]
    public async void TestGetBooksByAuthor()
    {
        var result = await controller.GetBooksByAuthor("al", 0, 5);
        var okResult = result as OkObjectResult;
        var booksTupple = (Tuple<long, IEnumerable<BookDTO>>)okResult.Value;
        long nbBooks = booksTupple.Item1;
        var books = booksTupple.Item2;

        Assert.Equal(4, books.Count());
    }
}
