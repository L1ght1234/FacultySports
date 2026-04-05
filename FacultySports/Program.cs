using System.Reflection;
using FacultySports.Application;
using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories;
using FacultySports.MVC.Services.Competitions;
using FacultySports.MVC.Services;
using FacultySports.MVC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SportsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<SportsDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddInfrastructureRepositories();
builder.Services.AddScoped<IDataPortServiceFactory<Competition>, CompetitionDataPortServiceFactory>();
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.InitializeAsync(userManager, roleManager);
}

app.Run();
