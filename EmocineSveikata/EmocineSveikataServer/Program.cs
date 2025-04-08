using Microsoft.EntityFrameworkCore;
using EmocineSveikataServer.Data;
using EmocineSveikataServer.Services.DiscussionService;
using EmocineSveikataServer.Services.CommentService;
using EmocineSveikataServer.Repositories.DiscussionRepository;
using EmocineSveikataServer.Repositories.CommentRepository;
using EmocineSveikataServer.Mapper;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// CORS konfigūracija
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));
// === Vartotojų autentifikacija (galimai prireiks) ===
// 1. JWT autentifikacija
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//         };
//     });
//
// 2. Autorizacija
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
//     options.AddPolicy("User", policy => policy.RequireRole("User"));
// });

// === Paleidžiam React frontend'ą ===
try
{
    var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "frontend");

    if (Directory.Exists(frontendPath))
    {
        var npmProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "npm",
                Arguments = "start",
                WorkingDirectory = frontendPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        npmProcess.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        npmProcess.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
        npmProcess.Start();
        npmProcess.BeginOutputReadLine();
        npmProcess.BeginErrorReadLine();
        Console.WriteLine($"Frontend server started at {frontendPath}");
    }
    else
    {
        Console.WriteLine("Warning: Frontend directory not found. Starting backend only.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error starting frontend: {ex.Message}");
}

var app = builder.Build();

// === Middleware ===
// 1. CORS middleware
app.UseCors("AllowFrontend");


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

app.Use(async (context, next) =>
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    
    await next();
    
    stopwatch.Stop();
    var elapsed = stopwatch.Elapsed;
    
    Console.WriteLine($"{context.Request.Method} {context.Request.Path} - {elapsed.TotalMilliseconds}ms");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
