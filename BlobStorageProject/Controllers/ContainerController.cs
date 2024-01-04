using Azure.Storage.Blobs;
using BlobStorageProject.Models;
using BlobStorageProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlobStorageProject.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            var allContainer = await _containerService.GetAllContainer();
            return View(allContainer);
        }

        public async Task<IActionResult> Create(string containerName)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Container container)
        {
            if (!ModelState.IsValid)
            {
                return View(container);
            }

            await _containerService.CreateContainer(container.Name);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerService.DeleteContainer(containerName);
            return RedirectToAction(nameof(Index));
        }
    }
}
