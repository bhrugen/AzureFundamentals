
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Services;
public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobClient;

    public BlobService(BlobServiceClient blobClient)
    {
        _blobClient = blobClient;
    }
    public async Task<bool> DeleteBlob(string name, string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

        var blobClient = blobContainerClient.GetBlobClient(name);

        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<List<string>> GetAllBlobs(string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
        var blobs = blobContainerClient.GetBlobsAsync();

        var blobString = new List<string>();

        await foreach(var item in blobs)
        {
            blobString.Add(item.Name);
        }

        return blobString;
    }

    public async Task<string> GetBlob(string name, string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

        var blobClient = blobContainerClient.GetBlobClient(name);

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

        var blobClient = blobContainerClient.GetBlobClient(name);

        var httpHeaders = new BlobHttpHeaders()
        {
            ContentType = file.ContentType
        };

        var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);

        if (result != null)
        {
            return true;
        }
        return false;
    }
}
