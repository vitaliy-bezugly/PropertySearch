using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Persistence;

public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {  }

    public DbSet<AccommodationEntity> Accommodations { get; set; } = null!;
    public DbSet<ContactEntity> Contacts { get; set; } = null!;
}
