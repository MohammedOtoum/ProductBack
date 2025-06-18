using entity.Model;
using Microsoft.EntityFrameworkCore;

namespace entity.Data
{
    public class EntityFrameWork : DbContext
    {
        public DbSet<Blog> blogs { get; set; }
        public DbSet<Post> posts { get; set; }

        public string DbPath
    }
}
