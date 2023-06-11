using Microsoft.AspNetCore.Mvc;
using PropertySearchApp.Repositories.Abstract;

namespace PropertySearchApp.Controllers;

public class ImageController : ControllerBase
{
    private readonly IPictureRepository _pictureRepository;

    public ImageController(IPictureRepository pictureRepository)
    {
        _pictureRepository = pictureRepository;
    }

    [HttpGet, Route("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var image = await _pictureRepository.GetPictureByIdAsync(id, cancellationToken);
        if (image is null)
            return NotFound();

        return File(image.PictureSource, "image/png");
    }
}