using Microsoft.EntityFrameworkCore;
using EmocineSveikataServer.Data;
using EmocineSveikataServer.Services.DiscussionService;
using EmocineSveikataServer.Services.CommentService;
using EmocineSveikataServer.Services.AuthService;
using EmocineSveikataServer.Repositories.DiscussionRepository;
using EmocineSveikataServer.Repositories.CommentRepository;
using EmocineSveikataServer.Repositories.UserRepository;
using EmocineSveikataServer.Repositories.ProfileRepository;
using EmocineSveikataServer.Mapper;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EmocineSveikataServer.Services.RoomService;

var builder = WebApplication.CreateBuilder(args);

// CORS konfigūracija 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // naudojama API enums vietoj ids
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<ISpecialistProfileRepository, SpecialistProfileRepository>();
builder.Services.AddScoped<ISpecialistTimeSlotRepository, SpecialistTimeSlotRepository>();

builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

// JWT autentifikacija
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Autorizacija rolėmis
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Naudotojas", policy => policy.RequireRole("Naudotojas"));
    options.AddPolicy("Specialistas", policy => policy.RequireRole("Specialistas"));
});

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
// SVARBU: CORS middleware turi but callintas pries kitus middleware
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

// Uzkomentuota HTTPS redirectionas kad išvengt CORS problemu per developmenta
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
