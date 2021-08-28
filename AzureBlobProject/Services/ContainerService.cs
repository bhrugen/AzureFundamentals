
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Services;
public class ContainerService : IContainerService
{
    private readonly BlobServiceClient _blobClient;

    public ContainerService(BlobServiceClient blobClient)
    {
        _blobClient = blobClient;
    }

    public async Task CreateContainer(string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

    }

    public async Task DeleteContainer(string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
        await blobContainerClient.DeleteIfExistsAsync();
    }

    public async Task<List<string>> GetAllContainer()
    {
        List<string> containerName = new();

        await foreach(BlobContainerItem blobkContainerItem in _blobClient.GetBlobContainersAsync())
        {
            containerName.Add(blobkContainerItem.Name);
        }

        return containerName;
    }

    public Task<List<string>> GetAllContainerAndBlobs()
    {
        throw new NotImplementedException();
    }
}
