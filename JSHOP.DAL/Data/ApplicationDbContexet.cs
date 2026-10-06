using JSHOP.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JSHOP.DAL.Data
{
    public class ApplicationDbContexet:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ApplicationDbContexet (DbContextOptions<ApplicationDbContexet> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries <AuditableEntity>();
           var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entity in entries)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Property(e => e.CreatedById).CurrentValue = currentUserId;
                    entity.Property(e => e.CreatedDate).CurrentValue = DateTime.UtcNow;
                }
                else if(entity.State == EntityState.Modified)
                {
                    entity.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                    entity.Property(e => e.UpdatedDate).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
