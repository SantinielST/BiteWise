using BiteWise.DLL.Entities;
using BiteWise.DLL.TablesСonnections;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BiteWise.DLL;

public class BiteWiseAppContext : IdentityDbContext<UserEntity>
{
    public DbSet<ArticleEntity> Articles { get; set; }
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<TagArticleConnection> TagsConnection { get; set; }

    public BiteWiseAppContext(DbContextOptions<BiteWiseAppContext> options) : base(options)
    {

    }

    public BiteWiseAppContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=bitewise.db");
    }
}