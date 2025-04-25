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
using System.Text.Json.Serialization;
using EmocineSveikataServer.Services.Meets;

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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Use strings when sending API enums instead of ids
});
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

builder.Services.AddScoped<GoogleMeetService>();

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
        var npmInstallProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c npm install",
                WorkingDirectory = frontendPath,
                UseShellExecute = true,
                CreateNoWindow = false
            }
        };
        npmInstallProcess.Start();
        npmInstallProcess.WaitForExit();

        var npmStartProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c npm start",
                WorkingDirectory = frontendPath,
                UseShellExecute = true,
                CreateNoWindow = false
            }
        };
        npmStartProcess.Start();

        try
        {
            var frontendUrl = "http://localhost:3000";
            var browserProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = frontendUrl,
                    UseShellExecute = true 
                }
            };
            browserProcess.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not open browser: {ex.Message}");
        }

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
