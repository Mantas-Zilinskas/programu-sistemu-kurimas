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
using EmocineSveikataServer.Services.Meets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EmocineSveikataServer.Services.PositiveMessageService;
using EmocineSveikataServer.Services.RoomService;
using EmocineSveikataServer.Services.NotificationService;
using EmocineSveikataServer.Repositories.NotificationRepository;
using EmocineSveikataServer.Filters;
using Serilog;

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

// Logs can be found in '\EmocineSveikataServer\Logs\ES_LOG_*.txt'.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LoggingActionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // naudojama API enums vietoj ids
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    // Ctikrinam kokia sistema naudojama
    bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
    
    if (isWindows)
    {
        // Naudojamas SQL Server LocalDB Windows
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    else
    {
        // Naudojamas SQLite Linux/Mac
        var sqliteConnectionString = builder.Configuration.GetConnectionString("SQLiteConnection") 
            ?? "Data Source=EmocineSveikata.db";
        options.UseSqlite(sqliteConnectionString);
        
        Console.WriteLine("Using SQLite database on non-Windows platform");
    }
});

builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<ISpecialistProfileRepository, SpecialistProfileRepository>();
builder.Services.AddScoped<ISpecialistTimeSlotRepository, SpecialistTimeSlotRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<GoogleMeetService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPositiveMessageService, PositiveMessageService>();
builder.Services.AddScoped<IRoomService, RoomService>();

// Notification service using Strategy Design Pattern
// Utilize it by changing "appsettings.json".NotificationSettings.Type from the default "Regular" to "Hearts" to add hearts to notifications... for the extra user comfort!
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<NotificationServiceHearts>();
builder.Services.AddScoped<INotificationServiceFactory, NotificationServiceFactory>();
builder.Services.AddScoped(sp => sp.GetRequiredService<INotificationServiceFactory>().Create());

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

if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        
        dbContext.Database.EnsureDeleted();
        
        dbContext.Database.EnsureCreated();
        
        Console.WriteLine("SQLite database created or verified successfully");
    }
}

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

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Closing web host");
    Log.CloseAndFlush();
}
