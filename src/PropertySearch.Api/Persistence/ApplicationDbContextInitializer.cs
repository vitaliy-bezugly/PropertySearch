using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertySearch.Api.Entities;

namespace PropertySearch.Api.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
            else
            {
                string errorMessage = "Can not execute migration. Database provider not supported. Supported providers: SqlServer";
                _logger.LogError(errorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error has occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (_context.Roles.Any() == false)
        {
            _logger.LogInformation("Seeding database...");
            
            _context.Roles.AddRange(new List<IdentityRole<Guid>>
            {
                new IdentityRole<Guid> { Id = Guid.Parse("63258DBE-D709-41C8-90AF-7278CB987C1E"), Name = "admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString()},
                new IdentityRole<Guid> { Id = Guid.Parse("9E4F35DB-192C-47FB-B7D4-A281B7989470"), Name = "landlord", NormalizedName = "LANDLORD", ConcurrencyStamp = Guid.NewGuid().ToString()},
                new IdentityRole<Guid> { Id = Guid.Parse("A84FBF0E-1B2A-4EA2-B703-A7E8D9F4A647"), Name = "user", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString()}
            });

            _context.Users.Add(new UserEntity
            {
                Id = Guid.Parse("1F9EBB06-0947-4D6F-F99C-08DB34EFB52C"), 
                UserName = "admin", 
                NormalizedUserName = "ADMIN", 
                Email = "adminemail@exapmle.com",
                NormalizedEmail = "ADMINEMAIL@EXAMPLE.COM",
                IsLandlord = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });
            
            _context.UserRoles.AddRange(new []
            {
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("63258DBE-D709-41C8-90AF-7278CB987C1E"), UserId = Guid.Parse("1F9EBB06-0947-4D6F-F99C-08DB34EFB52C")},
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("9E4F35DB-192C-47FB-B7D4-A281B7989470"), UserId = Guid.Parse("1F9EBB06-0947-4D6F-F99C-08DB34EFB52C")},
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("A84FBF0E-1B2A-4EA2-B703-A7E8D9F4A647"), UserId = Guid.Parse("1F9EBB06-0947-4D6F-F99C-08DB34EFB52C")}
            });

            /*
             * _context.Accommodations.Add(new AccommodationEntity
            {
                Id = Guid.NewGuid(),
                Title = "Accommodation 1",
                Description = "Description 1",
                PhotoUri = "https://via.placeholder.com/150",
                Price = 100,
                UserId = Guid.Parse("1F9EBB06-0947-4D6F-F99C-08DB34EFB52C"),
                LocationId = Guid.Parse("62ABF6A0-6F0B-4F0A-8F0A-08D9A4F4F5A1"),
                Location = new LocationEntity
                {
                    Id = Guid.Parse("62ABF6A0-6F0B-4F0A-8F0A-08D9A4F4F5A1"),
                    Country = "Ukraine",
                    Region = "Lviv Oblast",
                    City = "Lviv City",
                    Address = "Address 1",
                    CreationTime = DateTime.Now
                },
                CreationTime = DateTime.Now
            });
             */

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeding database completed.");
        }
        else
        {
            _logger.LogInformation("Database already seeded.");
        }
    }
}