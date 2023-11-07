using Acuedify.Data;
using Acuedify.Services.Library;
using Acuedify.Services.Playing;
using Acuedify.Services.Questions;
using Acuedify.Services.Library.Interfaces;
using Acuedify.Services.Playing.Interfaces;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDBContext>();

builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IPlayingService, PlayingService>();
builder.Services.AddScoped<IQuestionsService, QuestionsService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

using (var scope = app.Services.CreateScope())
{
    var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    if (!appDBContext.Quizzes.Any())
    {
        MockDataInitializer.SeedQuizzes(appDBContext, "MockData/quizzes.json");
    }
}

app.Run();
