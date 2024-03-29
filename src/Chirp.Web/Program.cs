using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Chirp.Web;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// This file is the entry point of the application. 
/// It is responsible for setting up the application and starting it.
/// </summary>

var builder = WebApplication.CreateBuilder(args);

string currentDirectory = Directory.GetCurrentDirectory();
string dbPath;

if (Directory.Exists(Path.Combine(currentDirectory, "..", "Chirp.Infrastructure", "data")))
{
    dbPath = Path.Combine(currentDirectory, "..", "Chirp.Infrastructure", "data", "ChirpDBContext.db"); //Build directory
}
else 
{
    dbPath = Path.Combine(currentDirectory, "data", "ChirpDBContext.db"); //Publish directory
}

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<ChirpDbContext>(options => 
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddDefaultIdentity<Author>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ChirpDbContext>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IValidator<CreateCheep>, CheepCreateValidator>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();

builder.Services.AddSession(
	options =>
	{
		options.Cookie.Name = ".Chirp.Session";
    	options.IdleTimeout = TimeSpan.FromMinutes(10);
    	options.Cookie.HttpOnly = false;
    	options.Cookie.IsEssential = true;
	});

//Github OAuth:
builder.Services.AddAuthentication()
    .AddCookie();

var app = builder.Build();

// Get a scope to manage the liftime of the context
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Get an instance of the DbContext
    var context = services.GetRequiredService<ChirpDbContext>();

    // Call the method to remove duplicate user Logins
    await context.RemoveDuplicateUserLogins();

    // Call the method to seed the database
    try {
        DbInitializer.SeedDatabase(context);
    } catch (Exception ex) {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
    
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.Run();