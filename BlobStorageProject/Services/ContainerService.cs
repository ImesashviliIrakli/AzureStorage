
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlobStorageProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public ContainerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerName = new();
            await foreach(BlobContainerItem blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                containerName.Add(blobContainerItem.Name);
            }

            return containerName;
        }

        public Task<List<string>> GetAllContainerAndBlobs()
        {
            throw new NotImplementedException();
        }
    }
}
