using BlobStorageProject.Models;
using BlobStorageProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlobStorageProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContainerService _containerService;
        public HomeController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            var hierarchy = await _containerService.GetAllContainerAndBlobs();
            return View(hierarchy);
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
