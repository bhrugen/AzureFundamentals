
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureBlobProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public async Task<List<Blob>> GetAllBlobsWithUri(string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
        var blobs = blobContainerClient.GetBlobsAsync();
        var blobList = new List<Blob>();
        string sasContainerSignature = "";

        if (blobContainerClient.CanGenerateSasUri)
        {
            BlobSasBuilder sasBuilder = new()
            {
                BlobContainerName = blobContainerClient.Name,
                Resource = "c",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            sasContainerSignature = blobContainerClient.GenerateSasUri(sasBuilder).AbsoluteUri.Split('?')[1].ToString();
        }



        await foreach (var item in blobs)
        {
            var blobClient = blobContainerClient.GetBlobClient(item.Name);
            Blob blobIndividual = new()
            {
                Uri = blobClient.Uri.AbsoluteUri + "?" + sasContainerSignature
            };

            //if (blobClient.CanGenerateSasUri)
            //{
            //    BlobSasBuilder sasBuilder = new()
            //    {
            //        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
            //        BlobName = blobClient.Name,
            //        Resource = "b",
            //        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            //    };

            //    sasBuilder.SetPermissions(BlobSasPermissions.Read);

            //    blobIndividual.Uri = blobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
            //}

            BlobProperties properties = await blobClient.GetPropertiesAsync();
            if (properties.Metadata.ContainsKey("title"))
            {
                blobIndividual.Title = properties.Metadata["title"];
            }
            if (properties.Metadata.ContainsKey("comment"))
            {
                blobIndividual.Comment = properties.Metadata["comment"];
            }
            blobList.Add(blobIndividual);
        }

        return blobList;
    }

    public async Task<string> GetBlob(string name, string containerName)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

        var blobClient = blobContainerClient.GetBlobClient(name);

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob)
    {
        BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

        var blobClient = blobContainerClient.GetBlobClient(name);

        var httpHeaders = new BlobHttpHeaders()
        {
            ContentType = file.ContentType
        };

        IDictionary<string, string> metadata =
         new Dictionary<string, string>();

        metadata.Add("title", blob.Title);
        metadata["comment"] = blob.Comment;

        var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metadata);

        //metadata.Remove("title");

        //await blobClient.SetMetadataAsync(metadata);

        if (result != null)
        {
            return true;
        }
        return false;
    }

}
