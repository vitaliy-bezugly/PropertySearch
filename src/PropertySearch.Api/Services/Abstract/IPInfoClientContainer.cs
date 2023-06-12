using IPinfo;

namespace PropertySearch.Api.Services.Abstract;

public interface IPInfoClientContainer
{
    public IPinfoClient Client { get; }
}