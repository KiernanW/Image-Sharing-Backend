using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;


namespace imagesharing
{

    public static class Website
    {
        const string siteLoc = @"C:\home\site\wwwroot\site\";

        [FunctionName("Website")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string filePath = req.Query["filePath"];

            string fullFilePath = siteLoc + filePath;

            FileStream theFile = File.Open(fullFilePath, FileMode.Open, FileAccess.Read);

            byte[] returnVal = Website.ReadFully(theFile);

            theFile.Close();

            return new FileContentResult(returnVal, "text/html");
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16*1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}

