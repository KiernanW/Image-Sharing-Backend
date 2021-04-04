using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.IO;

namespace imagesharing
{

    public static class GetImage
    {
        [FunctionName("GetImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string path = Directory.GetCurrentDirectory();
            log.LogInformation(path.ToString());
            var cookieValue = req.Cookies["MyCookie"];
            log.LogInformation(cookieValue);

            Image theImage = Image.FromFile(@"C:\home\site\wwwroot/antlion.bmp");

            if (cookieValue != "MyValue"){
                return new BadRequestResult();
            }else{
                return new FileContentResult(ImageToByteArray(theImage), "image/jpeg");
            }
        }

        private static byte[] ImageToByteArray(Image image)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }
    }

}
