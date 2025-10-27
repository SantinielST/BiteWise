using BiteWise.BLL;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL;
using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories;
using BiteWise.DLL.Repositories.Interfaces;
using BiteWise.DLL.UoW;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BiteWise;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<BiteWiseAppContext>(options => options.UseSqlite(connection));

        builder.Services.AddIdentity<UserEntity, IdentityRole>(opts =>
        {
            opts.Password.RequiredLength = 5;
            opts.Password.RequireNonAlphanumeric = true;
            opts.Password.RequireLowercase = true;
            opts.Password.RequireUppercase = true;
            opts.Password.RequireDigit = true;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BiteWiseAppContext>();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IService<User>, UserService>();
        builder.Services.AddScoped<IService<Article>, ArticleService>();
        builder.Services.AddScoped<IService<Tag>, TagService>();
        builder.Services.AddScoped<IService<Comment>, CommentService>();
        builder.Services.AddScoped<IRepository<ArticleEntity>, ArticleRepository>();
        builder.Services.AddScoped<IRepository<TagEntity>, TagRepository>();
        builder.Services.AddScoped<IRepository<CommentEntity>, CommentRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddAutoMapper((v) => v.AddProfile(new MappingProfile()));

        builder.Services.AddMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new string[] { "Admin", "Moderator", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}