using Microsoft.EntityFrameworkCore;
using PropertySearchApp.Entities;
using PropertySearchApp.Persistence;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Repositories;

public class PictureRepository : IPictureRepository
{
    private readonly ApplicationDbContext _context;

    public PictureRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<PictureEntity?> GetPictureByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Pictures.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}