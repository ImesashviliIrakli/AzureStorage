using System;
using System.IO;
using System.Linq;
using AzureFunction.Data;
using AzureFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunction
{
    public class ChangeStatusOnImageResize
    {
        private readonly AzureFunctionDbContext _context;
        public ChangeStatusOnImageResize(AzureFunctionDbContext context)
        {
            _context = context;
        }

        [FunctionName("ChangeStatusOnImageResize")]
        public void Run([BlobTrigger("function-container-sm/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var fileName = Path.GetFileNameWithoutExtension(name);

            SalesRequest salesRequest = _context.SalesRequests.FirstOrDefault(x => x.Id == fileName);

            if (salesRequest != null)
            {
                salesRequest.Status = "Resized";

                _context.Update(salesRequest);
                _context.SaveChanges();
            }
        }
    }
}
