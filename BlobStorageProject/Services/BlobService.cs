
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using BlobStorageProject.Models;

namespace BlobStorageProject.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();

            var blobString = new List<string>();

            await foreach (var item in blobs)
            {
                blobString.Add(item.Name);
            }

            return blobString;
        }

        public async Task<List<Blob>> GetAllBlobsWithUri(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            
            var blobs = blobContainerClient.GetBlobsAsync();

            var blobList = new List<Blob>();

            string sas = "";

            // SAS Token Logic For Container
            if (blobContainerClient.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuildler = new BlobSasBuilder
                {
                    BlobContainerName = blobContainerClient.Name,
                    Resource = "c",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                sasBuildler.SetPermissions(BlobSasPermissions.Read);

                sas = blobContainerClient.GenerateSasUri(sasBuildler).AbsoluteUri.Split('?')[1].ToString() ;
            }

            await foreach(var item in blobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(item.Name);

                Blob singleBlob = new Blob
                {
                    Uri = blobClient.Uri.AbsoluteUri + "?" + sas
                };

                //// SAS Token Logic For Individual Blob
                //if (blobClient.CanGenerateSasUri)
                //{
                //    BlobSasBuilder sasBuildler = new BlobSasBuilder
                //    {
                //        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                //        BlobName = blobClient.Name,
                //        Resource = "b",
                //        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                //    };
                //    sasBuildler.SetPermissions(BlobSasPermissions.Read);
                //    singleBlob.Uri = blobClient.GenerateSasUri(sasBuildler).AbsoluteUri;
                //}

                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();

                if (blobProperties.Metadata.ContainsKey("title"))
                {
                    singleBlob.Title = blobProperties.Metadata["title"];
                }

                if (blobProperties.Metadata.ContainsKey("comment"))
                {
                    singleBlob.Title = blobProperties.Metadata["comment"];
                }

                blobList.Add(singleBlob);
            }

            return blobList;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> metaData = new Dictionary<string, string>();

            metaData.Add("title", blob.Title);
            metaData["comment"]= blob.Comment;

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metadata: metaData);
            
            // Set MetaData To Remove Title
            // Testing Purpose only
            //metaData.Remove("title");
            //await blobClient.SetMetadataAsync(metaData);

            if (result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            return await blobClient.DeleteIfExistsAsync();
        }

    }
}
