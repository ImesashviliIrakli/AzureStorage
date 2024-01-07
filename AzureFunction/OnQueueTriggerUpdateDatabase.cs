using System;
using AzureFunction.Data;
using AzureFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunction
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureFunctionDbContext _context;

        public OnQueueTriggerUpdateDatabase(AzureFunctionDbContext context)
        {
            _context = context;
        }

        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("SalesRequestInBound", Connection = "AzureWebJobsStorage")] SalesRequest myQueueItem,

            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            myQueueItem.Status = "Submitted";
            _context.SalesRequests.Add(myQueueItem);
            _context.SaveChanges();
        }
    }
}
