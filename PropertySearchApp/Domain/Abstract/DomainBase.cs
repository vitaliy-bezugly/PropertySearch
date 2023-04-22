namespace PropertySearchApp.Domain.Abstract;

public abstract class DomainBase
{
    public Guid Id { get; set; }

    public DomainBase()
    {
        Id = Guid.Empty;
    }
    public DomainBase(Guid id)
    {
        Id = id;
    }
}