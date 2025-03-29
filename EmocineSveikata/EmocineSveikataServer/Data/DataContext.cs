using Microsoft.EntityFrameworkCore;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
        
        }

        public DbSet<DiscussionModel> Discussions { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
    }
}
