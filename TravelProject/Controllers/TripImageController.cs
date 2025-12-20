using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using TravelBussinessLayer;

namespace TravelProject.Controllers
{
    [Route("api/TripImage")]
    [ApiController]
    public class TripImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public TripImageController(IWebHostEnvironment env)
        {
            _env = env;
        }

        private string GetUploadDirectory()
        {
            // نحاول نستخدم WebRootPath
            var webRoot = _env.WebRootPath;

            // لو null (ما تعرّف) نبنيه من ContentRootPath + "wwwroot"
            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");

                if (!Directory.Exists(webRoot))
                    Directory.CreateDirectory(webRoot);
            }

            // wwwroot/MyUploads
            var uploadDirectory = Path.Combine(webRoot, "MyUploads");

            if (!Directory.Exists(uploadDirectory))
                Directory.CreateDirectory(uploadDirectory);

            return uploadDirectory;
        }

        // POST: api/TripImage/Upload
        [Authorize(Roles = "Admin")]

        [HttpPost("Upload", Name = "UploadImage")]
        public async Task<IActionResult> UploadImage(int TripID, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadDirectory = GetUploadDirectory();

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            try
            {
                bool saved = clsTripImageBl.AddNewTripImage(TripID, fileName);

                if (!saved)
                {
                    // لو حبيت تنظّف الملف لو الـ DB ما قبلت مثلاً
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    return BadRequest("Bad request: failed to save image to database.");
                }
            }
            catch (SqlException ex) when (ex.Number == 547) // FK violation TripID
            {
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                return BadRequest("TripID غير موجود في جدول الرحلات. تأكد أنك تستخدم TripID صحيح.");
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/api/TripImage/GetImage/{fileName}";
            return Ok(new { fileName, imageUrl });
        }

        // GET: api/TripImage/GetImage/{fileName}
       
        // وندعم كمان الراوت القديم /api/images/GetImage/{fileName} لو كان مخزّن بالداتابيس

        [HttpGet("GetImage/{fileName}")]
        [HttpGet("/api/images/GetImage/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var uploadDirectory = GetUploadDirectory();
            var filePath = Path.Combine(uploadDirectory, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found.");

            var mimeType = GetMimeType(filePath);
            var image = System.IO.File.OpenRead(filePath);

            return File(image, mimeType);
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }
        [AllowAnonymous]
        // GET: api/TripImage/GetAllTripImages/{tripId}
        [HttpGet("GetAllTripImages/{tripId}")]
        public IActionResult GetAllTripImages(int tripId)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var images = clsTripImageBl.GetAllTripImages(tripId, baseUrl);

            return Ok(images);
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete ("DelteImageTripByImageID",Name ="DeleteImageTripByImageID")]
        public ActionResult DeleteImageByImageID(int ImageID)
        {


            if (ImageID <= 0)
            {

                return BadRequest("BadRequest Chose Another ID");
            }

            if(clsTripImageBl.DeleteTripImageByImageID(ImageID))
            {
                return Ok("Deleted Successfuly");
            }
            else
            {
                return NotFound($"Not Found Image with ID : {ImageID}");
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("DelteImageTripByTripID", Name = "DeleteImageTripByTripID")]

        public ActionResult DeleteImageByTripID(int TripID)
        {


            if (TripID <= 0)
            {

                return BadRequest("BadRequest Chose Another ID");
            }

            if (clsTripImageBl.DeleteTripImageByTripID(TripID))
            {
                return Ok("Deleted Successfuly");
            }
            else
            {
                return NotFound($"Not Found Image with ID : {TripID}");
            }
        }
    }
}