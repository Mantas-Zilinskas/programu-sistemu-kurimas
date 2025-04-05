using Microsoft.EntityFrameworkCore;
using EmocineSveikataServer.Data;
using EmocineSveikataServer.Services.DiscussionService;
using EmocineSveikataServer.Services.CommentService;
using EmocineSveikataServer.Repositories.DiscussionRepository;
using EmocineSveikataServer.Repositories.CommentRepository;
using EmocineSveikataServer.Mapper;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
	errorApp.Run(async context =>
	{
		var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
		var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

		var exception = exceptionHandlerPathFeature?.Error;

		context.Response.ContentType = "application/json";

		if (exception is KeyNotFoundException)
		{
			context.Response.StatusCode = StatusCodes.Status404NotFound;
			logger.LogWarning(exception, "Resource not found: {Message}", exception.Message);
		}
		else
		{
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			logger.LogError(exception, "An unexpected error occurred");
		}

		await context.Response.WriteAsync(JsonSerializer.Serialize(new
		{
			error = exception?.Message ?? "An unexpected error occurred."
		}));
	});
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
