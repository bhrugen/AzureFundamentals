
using AzureBlobProject.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureBlobProject.Services;

public interface IBlobService 
{
    Task<string> GetBlob(string name,string containerName);
    Task<List<string>> GetAllBlobs(string containerName);
    Task<List<Blob>> GetAllBlobsWithUri(string containerName);
    Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob);
    Task<bool> DeleteBlob(string name,string containerName);
}
