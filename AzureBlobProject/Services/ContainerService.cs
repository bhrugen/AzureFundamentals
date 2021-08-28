
using Azure.Storage.Blobs;

namespace AzureBlobProject.Services;
public class ContainerService : IContainerService
{
    private readonly BlobServiceClient _blobClient;

    public ContainerService(BlobServiceClient blobClient)
    {
        _blobClient = blobClient;
    }

    public Task CreateContainer(string containerName)
    {
        throw new NotImplementedException();
    }

    public Task DeleteContainer(string containerName)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetAllContainer()
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetAllContainerAndBlobs()
    {
        throw new NotImplementedException();
    }
}
