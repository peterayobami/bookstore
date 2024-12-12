using System.Reflection;
using Book.Store.Api.Server;
using Dna;
using Dna.AspNet;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// NB: Dna Framework is a library that gives easy access to DI in static classes, among other things...
// Configure DnaFramework
builder.WebHost.UseDnaFramework(construct =>
{
    // Add configuration
    construct.AddConfiguration(builder.Configuration);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Book Store API",
        Version = "v1",
        Description = "The APIs for Book Store"
    });

    // Include the XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Add example filters
    options.ExampleFilters();
});

// Add swagger examples
builder.Services.AddSwaggerExamplesFromAssemblyOf<BookCredentials>();

builder.Services.AddControllers();

builder.Services.AddApplicationDatabase(builder.Configuration);
builder.Services.AddApplicationIdentity();
builder.Services.AddDatabaseSetup();
builder.Services.AddAuthentizationConfiguration(builder.Configuration);
builder.Services.AddAuthorizationConfiguration();

builder.Services.AddDomainServices();

var app = builder.Build();

// Execute database setup
app.UseDatabaseSetup();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Store API v1");
});

app.UseHttpsRedirection();
app.UseDnaFramework();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
