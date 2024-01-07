using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AzureFunction
{
    public class ResizeImageOnBlobUpload
    {
        [FunctionName("ResizeImageOnBlobUpload")]
        public void Run([BlobTrigger("function-container/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
            [Blob("function-container-sm/{name}", FileAccess.Write)] Stream myblobOutput,
            string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using Image<Rgba32> input = Image.Load<Rgba32>(myBlob, out IImageFormat format);

            input.Mutate(x => x.Resize(500, 400));

            input.Save(myblobOutput, format);
        }
    }
}
