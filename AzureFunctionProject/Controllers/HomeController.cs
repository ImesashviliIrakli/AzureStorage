using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureFunctionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace AzureFunctionProject.Controllers
{
    public class HomeController : Controller
    {
        private const string URL = "http://localhost:7088/api/OnSalesUploadWriteToQueue";

        private readonly ILogger<HomeController> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        static readonly HttpClient client = new HttpClient();
        public HomeController(ILogger<HomeController> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        //

        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest, IFormFile file)
        {
            salesRequest.Id = Guid.NewGuid().ToString();

            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await client.PostAsync(URL, content);

                string result = response.Content.ReadAsStringAsync().Result;
            }

            if(file != null)
            {
                string fileName = salesRequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient("function-container");
                var blobClient = blobContainerClient.GetBlobClient(fileName);

                var httpheaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };

                await blobClient.UploadAsync(file.OpenReadStream(), httpheaders);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
