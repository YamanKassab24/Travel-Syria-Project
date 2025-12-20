using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TravelBussinessLayer;
using TravelDataAccess;

namespace TravelProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationServiceController : ControllerBase
    {
        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllReservationService",Name ="GetAllReservationService")]
        public ActionResult<ReservationServiceDTO> GetAllReservationService()
        {


           List<ReservationServiceDTO> reservationServiceDTOs = new List<ReservationServiceDTO>();
            reservationServiceDTOs = clsReservationServiceBL.GetAllReservationsServices();
            if (reservationServiceDTOs.Count > 0)
            {

                return Ok(reservationServiceDTOs);

            }
            else
            {
                return NotFound("Not Found ReservationsServices");
            }

        }

        [Authorize]
        [HttpPost("AddNewReservationService",Name ="AddNewReservationService")]
        public ActionResult<ReservationServiceDTO> AddNewReservationService(RequestModelReservationService Model)
        {

            if (Model.ReservationID <= 0 || Model.ServiceID <= 0)
            {
                return BadRequest("Bad Request Cannot  Add ReservationService");

            }
            clsReservationServiceBL reservationServiceBL = new clsReservationServiceBL();
            reservationServiceBL.ReservationID = Model.ReservationID;
            reservationServiceBL.ServiceID = Model.ServiceID;
            reservationServiceBL.Price = Model.Price;
            if (reservationServiceBL.Save())
            {
                return Ok(reservationServiceBL.RSDTO);

            }
            else
            {
                return NotFound("Not Found ReservationServiceID");
            }

        }

        [Authorize]
        [HttpDelete("DeleteReservationServiceByReservationID", Name = "DeleteReservationServiceByReservationID")]
        public ActionResult DeleteReservationServiceByReservationID(int ReservationID)
        {
            if(ReservationID <= 0)
            {
                return BadRequest("BadRequest Chose Another ReservationID");
            }
            if (clsReservationServiceBL.DeleteAllReservationServiceByReservationID(ReservationID))
            {
                return Ok("Delete Successfuly");
            }
            else
            {
                return NotFound();
            }


        }

        [Authorize]
        [HttpGet("GetAllReservationServiceByReservationID", Name = "GetAllReservationServiceReservationID")]
        public ActionResult<ReservationServiceDTO> GetAllReservationServiceReservationID(int ReservationID)
        {


            List<ReservationServiceDTO> reservationServiceDTOs = new List<ReservationServiceDTO>();
            reservationServiceDTOs = clsReservationServiceBL.GetAllReservationsServicesByReservationID(ReservationID);
            if (reservationServiceDTOs.Count > 0)
            {

                return Ok(reservationServiceDTOs);

            }
            else
            {
                return NotFound("Not Found ReservationsServices");
            }

        }

        [Authorize]
        [HttpGet("GetAllReservationServiceByServiceID", Name = "GetAllReservationServiceByServiceID")]
        public ActionResult<ReservationServiceDTO> GetAllReservationServiceByServiceID(int ServiceID)
        {


            List<ReservationServiceDTO> reservationServiceDTOs = new List<ReservationServiceDTO>();
            reservationServiceDTOs = clsReservationServiceBL.GetAllReservationsServicesByServiceID(ServiceID);
            if (reservationServiceDTOs.Count > 0)
            {

                return Ok(reservationServiceDTOs);

            }
            else
            {
                return NotFound("Not Found ReservationsServices");
            }

        }

        [Authorize]
        [HttpDelete("DeleteReservationServiceByReservationIDAndServiceID", Name = "DeleteReservationServiceByReservationIDAndServiceID")]
        public ActionResult DeleteReservationServiceByReservationIDAndServiceID(int ReservationID,int ServiceID)
        {
            if (ReservationID <= 0|| ServiceID<=0)
            {
                return BadRequest("BadRequest Chose Another ReservationID or ServiceID");
            }
            if (clsReservationServiceBL.DeleteAllReservationServiceByReservationIDAndserviceID(ReservationID,ServiceID))
            {
                return Ok("Delete Successfuly");
            }
            else
            {
                return NotFound();
            }


        }
        [Authorize]
        [HttpGet("CheckReservationService",Name = "CheckReservationService")]
        public ActionResult<bool> CheckReservationService(int ReservationID,int ServiceID)
        {
            if (ReservationID <= 0 || ServiceID <= 0)
            {
                return BadRequest(false);
            }
            if(clsReservationServiceBL.CheckReservationService(ReservationID, ServiceID))
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }



        }

    }
    public class RequestModelReservationService
    {
        public int ReservationID { get; set; }
        public int ServiceID { get; set; }

        public decimal Price { get; set; }





    }
}
