using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication.ExtendedProtection;
using TravelBussinessLayer;
using TravelDataAccess;

namespace TravelProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllServices",Name = "GetAllServices")]
        public ActionResult<ServiceDTO> GetAllServices()
        {
            List<ServiceDTO> Services = clsServiceBL.GetAllService();

            if (Services.Count <= 0)
            {

                return NotFound("Not Found Services");
            }
            else
            {
                return Ok(Services);
            }

        }
        [Authorize]
        [HttpGet("GetServiceByID",Name ="GetServiceByID")]
        public ActionResult<ServiceDTO> GetServiceByID(int ServiceID)
        {
            if (ServiceID < 0)
            {
                return BadRequest("Bad request Chose Another ID");
            }
           
            clsServiceBL Service = clsServiceBL.GetServiceByID(ServiceID);
            if (Service == null)
            {
                return NotFound($"Not Found Service WithID : {ServiceID}");

            }
          return Ok(Service.SDTO);



        }
        [Authorize]
        [HttpPost("UploadImage", Name = "UploadImageService")]
        public async Task<IActionResult> UploadImageService(int ServiceID, IFormFile imageFile)
        {
            // Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");

            // Directory where files will be uploaded
            var uploadDirectory = @"C:\MyUploads";

            // Generate a unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Ensure the uploads directory exists, create if it doesn't
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            if (clsServiceBL.AddNewServiceImage (ServiceID, fileName))
            {
                return Ok(new { filePath });


            }
            else
            {
                return BadRequest(" Badrequest");
            }
            // Return the file path as a response
        }
        [Authorize]
        [HttpGet("GetSericeImages/{ServiceId}")]
        public IActionResult GetAllTripImages(int ServiceId)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var images = clsServiceBL.GetServiceImage(ServiceId, baseUrl);

            return Ok(images);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("AddNewService",Name ="AddNewService")]
        public ActionResult<ServiceDTO> AddNewService(string ServiceName ,string Description,bool IsActive = true)
        {
            if (ServiceName == null || Description == null) 
                return BadRequest("BadRequest");

            clsServiceBL serviceBL = new clsServiceBL();
            serviceBL.ServiceName = ServiceName;
            serviceBL.Description = Description;
            serviceBL.IsActive = IsActive;
            if(serviceBL.Save())
            {
                return Ok(serviceBL.SDTO);
            }
            else
            {
                return NotFound();
            }



        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateService", Name = "UpdateService")]
        public ActionResult<ServiceDTO> UpdateService(int ServiceID,string ServiceName, string Description, bool IsActive = true)
        {
            if (ServiceName == null || Description == null)
                return BadRequest("BadRequest");

            clsServiceBL serviceBL = new clsServiceBL();
            serviceBL.ServiceID = ServiceID;
            serviceBL.ServiceName = ServiceName;
            serviceBL.Description = Description;
            serviceBL.IsActive = IsActive;
            serviceBL.Mode = clsServiceBL.enMode.Update;
            if (serviceBL.Save())
            {
                return Ok(serviceBL.SDTO);
            }
            else
            {
                return NotFound();
            }



        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteServiceWithServiceID", Name = "DeleteServiceWithServiceID")]
        public ActionResult DeleteServiceWithServiceID(int ServiceID)
        {

            if (ServiceID <= 0) return BadRequest("BadRequest");
            if (clsServiceBL.DeleteService(ServiceID))
                return Ok($"Deleted Service With ID : {ServiceID} Successfuly");
            else { return NotFound($"Not Found Service WithID : {ServiceID}");  }

        }

    }
}
