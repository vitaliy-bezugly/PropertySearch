using PropertySearchApp.Entities.Abstract;

namespace PropertySearchApp.Domain;

public class AccommodationDomain : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public Guid UserId { get; set; }

    public AccommodationDomain()
    {
        Title = string.Empty;
        Description = string.Empty;
        Price = 0;
        UserId = Guid.Empty;
    }
}