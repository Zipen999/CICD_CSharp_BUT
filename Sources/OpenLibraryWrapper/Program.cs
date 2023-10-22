using System.Reflection;
using DtoAbstractLayer;
using LibraryDTO;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.OpenApi.Models;
using MyLibraryManager;
using OpenLibraryClient;
using StubbedDTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string typedata = System.Environment.GetEnvironmentVariable("TYPE_DATA", System.EnvironmentVariableTarget.Process);
switch (typedata)
{
    case "STUB":
        builder.Services.AddSingleton<IDtoManager, Stub>();
        break;
    case "BDD":
        string host = System.Environment.GetEnvironmentVariable("HOST_DB", System.EnvironmentVariableTarget.Process);
        string port = System.Environment.GetEnvironmentVariable("PORT_DB", System.EnvironmentVariableTarget.Process);
        string user = System.Environment.GetEnvironmentVariable("USER_DB", System.EnvironmentVariableTarget.Process);
        string password = System.Environment.GetEnvironmentVariable("PSWD_DB", System.EnvironmentVariableTarget.Process);
        string db = System.Environment.GetEnvironmentVariable("NAME_DB", System.EnvironmentVariableTarget.Process);
        builder.Services.AddSingleton<IDtoManager>(cs => new MyLibraryMgr($"server={host};port={port};user={user};password={password};database={db}"));
        break;
    case "API":
    default:
        builder.Services.AddSingleton<IDtoManager, OpenLibClientAPI>();
        break;

}

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


/*
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
*/



app.Run();

