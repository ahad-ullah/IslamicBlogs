using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IslamicBlogs.Models
{
    public class BlogsDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public BlogsDbContext(DbContextOptions<BlogsDbContext> options) : base(options)
        {
        }

    }
}
