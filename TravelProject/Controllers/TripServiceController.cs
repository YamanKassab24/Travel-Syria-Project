using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using TravelBussinessLayer;
using TravelDataAccess;

namespace TravelProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripServiceController : ControllerBase
    {


        [AllowAnonymous]
        [HttpGet("GetAllTripServices", Name = "GetAllTripServices")]
        public ActionResult<TripServiceDTO> GetAllTripServices()
        {
            List<TripServiceDTO> TripServices = clsTripServiceBL.GetAllTripService();

            if (TripServices.Count <= 0)
            {

                return NotFound("Not Found TripServices");
            }
            else
            {
                return Ok(TripServices);
            }

        }
        [AllowAnonymous]
        [HttpGet("GetTripServiceByTripServiceID", Name = "GetTripServiceByTripServiceID")]
        public ActionResult<TripServiceDTO> GetTripServiceByID(int TripServiceID)
        {
            if (TripServiceID <= 0)
            {
                return BadRequest("Bad request Chose Another ID");
            }

            clsTripServiceBL TripService = clsTripServiceBL.GetTripServiceByTripServicID(TripServiceID);
            if (TripService == null)
            {
                return NotFound($"Not Found Service WithID : {TripServiceID}");

            }
            return Ok(TripService.TSDTO);



        }
        [AllowAnonymous]
        [HttpGet("GetTripServicesByTripID", Name = "GetTripServicesByTripID")]
        public ActionResult<TripServiceDTO> GetTripServicesByTripID(int TripID)
        {

            if (TripID <= 0)
            {
                return BadRequest("bad request Chose Another ID");
            }
            List<TripServiceDTO> TripServices = clsTripServiceBL.GetTripServiceByTripID(TripID);

            if (TripServices.Count <= 0)
            {

                return NotFound("Not Found TripServices");
            }
            else
            {
                return Ok(TripServices);
            }

        }
        [AllowAnonymous]
        [HttpGet("GetTripServicesByServiceID", Name = "GetTripServicesByServiceID")]
        public ActionResult<TripServiceDTO> GetTripServicesByServiceID(int ServiceID)
        {

            if (ServiceID <= 0)
            {
                return BadRequest("bad request Chose Another ID");
            }
            List<TripServiceDTO> TripServices = clsTripServiceBL.GetTripServiceByServiceID(ServiceID);

            if (TripServices.Count <= 0)
            {

                return NotFound("Not Found TripServices");
            }
            else
            {
                return Ok(TripServices);
            }

        }
        [Authorize(Roles = "Admin")]

        [HttpPost("AddNewTripService",Name ="AddNewTripService")]
        public ActionResult<TripServiceDTO> AddNewTripService(AddNewTripServiceRequest Model)
        {
            if(Model == null)
            {
                return BadRequest();

            }
            clsTripServiceBL tripServiceBL = new clsTripServiceBL();
            tripServiceBL.TripID = Model.TripID;
            tripServiceBL.ServiceID = Model.ServiceID;
            tripServiceBL.Price = Model.Price;
            tripServiceBL.IsActive = Model.IsActive;

            if (tripServiceBL.Save())
            {
                return Ok(tripServiceBL.TSDTO);
            }
            else { 
            return BadRequest("Field Added TripService");
            }
            


        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateTripService", Name = "UpdateTripService")]
        public ActionResult<TripServiceDTO> UpdateTripService(UpdateTripServiceRequest Model)
        {
            if (Model == null)
            {
                return BadRequest();

            }
            clsTripServiceBL tripServiceBL = new clsTripServiceBL();
            tripServiceBL.TripID = Model.TripID;
            tripServiceBL.ServiceID = Model.ServiceID;
            tripServiceBL.Price = Model.Price;
            tripServiceBL.IsActive = Model.IsActive;
            tripServiceBL.TripServiceID = Model.TripServiceID;
            tripServiceBL.Mode = clsTripServiceBL.enMode.Update;
            if (tripServiceBL.Save())
            {
                return Ok(tripServiceBL.TSDTO);
            }
            else
            {
                return BadRequest("Field Added TripService");
            }



        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteTripServiceByServiceID", Name = "DeleteTripServiceByServiceID")]
        public ActionResult DeleteTripServiceByServiceID(int ServiceID)
        {

            if (ServiceID<=0)
            {
                return BadRequest("Bad Request Chose Another ID ");
            }
            if (clsTripServiceBL.DeleteTripServiceByServiceID(ServiceID))
            {
                return Ok("Deleted Successfuly");
            }
            else {
                return NotFound("Not Found ID");
                    }


        }


        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteTripServiceByTripID", Name = "DeleteTripServiceByTripID")]
        public ActionResult DeleteTripServiceByTripID(int TripID)
        {

            if (TripID <= 0)
            {
                return BadRequest("Bad Request Chose Another ID ");
            }
            if (clsTripServiceBL.DeleteTripServiceByTripID(TripID))
            {
                return Ok("Deleted Successfuly");
            }
            else
            {
                return NotFound("Not Found ID");
            }


        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteTripServiceByTripServiceID", Name = "DeleteTripServiceByTripServiceID")]
        public ActionResult DeleteTripServiceByTripServiceID(int TripServiceID)
        {

            if (TripServiceID <= 0)
            {
                return BadRequest("Bad Request Chose Another ID ");
            }
            if (clsTripServiceBL.DeleteTripServiceByTripServiceID(TripServiceID))
            {
                return Ok("Deleted Successfuly");
            }
            else
            {
                return NotFound("Not Found ID");
            }


        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteTripServiceByTripIDAndServiceID", Name = "DeleteTripServiceByTripIDAndServiceID")]
        public ActionResult DeleteTripServiceByTripIDAndServiceID(int TripID,int ServiceID)
        {

            if (TripID <=0 ||ServiceID <= 0)
            {
                return BadRequest("Bad Request Chose Another ID ");
            }
            if (clsTripServiceBL.DeleteTripServiceByTripIdAndServiceID(TripID,ServiceID))
            {
                return Ok("Deleted Successfuly");
            }
            else
            {
                return NotFound("Not Found ID");
            }


        }
    }
    public class AddNewTripServiceRequest
    {
         public int TripID {  get; set; } 
           public int  ServiceID { get; set; }
            public decimal Price { get; set; }
           public bool   IsActive { get; set; }



    }
    public class UpdateTripServiceRequest: AddNewTripServiceRequest
    {
     public   int TripServiceID { get; set; }


    }
}
