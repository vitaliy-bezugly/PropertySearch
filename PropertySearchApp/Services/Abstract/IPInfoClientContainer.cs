using IPinfo;

namespace PropertySearchApp.Services.Abstract;

public interface IPInfoClientContainer
{
    public IPinfoClient Client { get; }
}