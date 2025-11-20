using BiteWise.BLL;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.DLL;
using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories;
using BiteWise.DLL.Repositories.Interfaces;
using BiteWise.DLL.TablesСonnections;
using BiteWise.DLL.UoW;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BiteWise",
        Version = "v1"
    });
});

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

builder.Services.AddSingleton<ICustomLogger, CustomLogger>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IService<User>, UserService>();
builder.Services.AddScoped<IService<Article>, ArticleService>();
builder.Services.AddScoped<IService<Tag>, TagService>();
builder.Services.AddScoped<IService<Comment>, CommentService>();
builder.Services.AddScoped<IService<TagArticleConnection>, TagArticleConnectionService>();
builder.Services.AddScoped<IRepository<ArticleEntity>, ArticleRepository>();
builder.Services.AddScoped<IRepository<TagEntity>, TagRepository>();
builder.Services.AddScoped<IRepository<CommentEntity>, CommentRepository>();
builder.Services.AddScoped<IRepository<TagArticleConnection>, TagArticleConnectionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper((v) => v.AddProfile(new MappingProfile()));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BiteWise v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();