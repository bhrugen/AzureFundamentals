
using Azure.Storage.Blobs;

namespace AzureBlobProject.Services;
public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobClient;

    public BlobService(BlobServiceClient blobClient)
    {
        _blobClient = blobClient;
    }
    public Task<bool> DeleteBlob(string name, string containerName)
    {
        throw new NotImplementedException();
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

    public Task<string> GetBlob(string name, string containerName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UploadBlob(string name, IFormFile file, string containerName)
    {
        throw new NotImplementedException();
    }
}
