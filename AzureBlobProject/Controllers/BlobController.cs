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

    [HttpGet]
    public IActionResult AddFile(string containerName)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddFile(string containerName, IFormFile file)
    {
        if (file == null || file.Length < 1) return View();

        //file name - xps_img2.png 
        //new name - xps_img2_GUIDHERE.png
        var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
        var result = await _blobService.UploadBlob(fileName, file, containerName);

        if(result)
            return RedirectToAction("Index", "Container");

        return View();
    }


}
