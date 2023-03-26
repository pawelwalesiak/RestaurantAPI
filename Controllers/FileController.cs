using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{

    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        public ActionResult GetFile([FromQuery] string fileName) 
        { 
            var rootPaht = Directory.GetCurrentDirectory();

            var filePath = $"{rootPaht}/PrivateFiles/{fileName}";

            var fileExists = System.IO.File.Exists(filePath);

            if (!fileExists) 
            { 
                return NotFound();
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);           

            var fileContents  = System.IO.File.ReadAllBytes(filePath);

            // wersja pliku czy to txt czy iinny
            return File(fileContents, contentType , fileName);

        }

    }
}
