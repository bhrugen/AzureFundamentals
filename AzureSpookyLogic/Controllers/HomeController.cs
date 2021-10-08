using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureSpookyLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureSpookyLogic.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    static readonly HttpClient client = new HttpClient();
    private readonly BlobServiceClient _blobClient;
    public HomeController(ILogger<HomeController> logger, BlobServiceClient blobClient)
    {
        _logger = logger;
        _blobClient= blobClient;    
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(SpookyRequest spookyRequest, IFormFile file)
    {
        spookyRequest.Id = Guid.NewGuid().ToString();
        var jsonContent = JsonConvert.SerializeObject(spookyRequest);
        using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
        {
            HttpResponseMessage httpResponse = await client.PostAsync("https://prod-11.northcentralus.logic.azure.com:443/workflows/e20e0f51a7894fb4a0a4b1b5b015019d/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=7bWyYpEgNN1BmKGyx-UegSUQATt9gO5KbL9nQqaOUQo", content);
        }

        if (file != null)
        {
            var fileName = spookyRequest.Id + Path.GetExtension(file.FileName);
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient("logicappholder");
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
        }

            return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
