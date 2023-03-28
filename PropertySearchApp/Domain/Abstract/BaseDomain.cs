namespace PropertySearchApp.Domain.Abstract;

public abstract class BaseDomain
{
    public Guid Id { get; set; }

    public BaseDomain()
    {
        Id = Guid.Empty;
    }
    public BaseDomain(Guid id)
    {
        Id = id;
    }
}