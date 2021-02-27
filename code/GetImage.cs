using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace imagesharing
{

    public static class GetImage
    {
        [FunctionName("GetImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var cookieValue = req.Cookies["MyCookie"];
            log.LogInformation(req.Cookies.ToString());

            Image theImage = Image.FromFile("C:/Work/Comp 4580 Security/Leaky_github/image_site_backend/images/antlion.bmp");

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
