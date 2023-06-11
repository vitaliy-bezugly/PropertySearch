using PropertySearchApp.Entities;

namespace PropertySearchApp.Repositories.Abstract;

public interface IPictureRepository
{
    Task<PictureEntity?> GetPictureByIdAsync(Guid id, CancellationToken cancellationToken);
}