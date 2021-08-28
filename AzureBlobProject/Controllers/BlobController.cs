using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers;
public class BlobController : Controller
{
    private readonly IBlobService _blobService;

    public BlobController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpGet]
    public async Task<IActionResult> Manage(string containerName)
    {
        var blobsObj = await _blobService.GetAllBlobs(containerName);
        return View(blobsObj);
    }
}
