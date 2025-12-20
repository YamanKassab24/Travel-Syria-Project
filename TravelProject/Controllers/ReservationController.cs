using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBussinessLayer;
using TravelDataAccess;

namespace TravelProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]




    public class ReservationController : ControllerBase
    {
        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllReservations", Name = "GetAllReservation")]

        public ActionResult<ReservationDTO> GetAllReservation()
        {

            List<ReservationDTO> reservations = clsReservationBL.GetAllReservations();
            if (reservations.Count > 0)
            {
                return Ok(reservations);


            }
            else
            {
                return NotFound("Not Found Reservation");
            }


        }

        [Authorize]
        [HttpGet("GetAllReservationsByUserID", Name = "GetAllReservationByUserID")]

        public ActionResult<ReservationDTO> GetAllReservationByUserID(int UserID)
        {
            if (UserID <= 0)
            {
                return BadRequest("Bad Request chose another UserID");
            }
            List<ReservationDTO> reservations = clsReservationBL.GetAllReservationsByUserID(UserID);
            if (reservations.Count > 0)
            {
                return Ok(reservations);


            }
            else
            {
                return NotFound("Not Found Reservation");
            }


        }

        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllReservationsByTripID", Name = "GetAllReservationByTripID")]

        public ActionResult<ReservationDTO> GetAllReservationByTripID(int TripID)
        {
            if (TripID <= 0) 
            {
                    return BadRequest("bad request Chose another TripID");
            }

            List<ReservationDTO> reservations = clsReservationBL.GetAllReservationsByTripID(TripID);

            if (reservations.Count > 0)
            {
                return Ok(reservations);


            }
            else
            {
                return NotFound("Not Found Reservation");
            }


        }
        [Authorize]
        [HttpPost("AddNewReservation",Name ="AddNewReservation")]
        public ActionResult<ReservationDTO> AddNewReservation(RequestAddNewReservation Model)
        {

            if (Model == null||Model.UserID<=0||Model.TripID<=0||Model.seatsCount<=0) 
            {
                return BadRequest("BadRequest");
             }
            clsReservationBL reservationBL = new clsReservationBL();
            reservationBL.UserID = Model.UserID;
            reservationBL.TripID = Model.TripID;
            reservationBL.seatsCount = Model.seatsCount;
            reservationBL.ReservationDate = Model.ReservationDate;
            reservationBL.TotalPrice = Model.TotalPrice;
            reservationBL.Status = Model.Status;

            if (reservationBL.Save())
            {
                return reservationBL.RDTO;
            }
            else
            {
               return NotFound("Field Add Reservation");
            }



        }
        [Authorize]
        [HttpDelete("DeleteReservationByReservationID", Name = "DeleteReservationByReservationID")]
        public ActionResult DeleteReservationByReservationID(int reservationID)
        {

            if (reservationID < 1)
            {
                return BadRequest($"Not accepted ID {reservationID}");
            }

            if (clsReservationBL.DeleteReservationByReservationID(reservationID))

                return Ok($"Reservations with ID {reservationID} has been deleted.");
            else
                return NotFound($"Reservations with ID {reservationID} not found. no rows deleted!");
        }
        [Authorize]
        [HttpGet("GetReservationByReservationID", Name = "GetReservationByReservationID")]
        public ActionResult<ReservationDTO> GetReservationByReservationID(int ReservationID)
        {
            if (ReservationID < 1)
            {
                return BadRequest("Bad request Choser Another ID");
            }
            clsReservationBL reservationBL = clsReservationBL.GetReservationByReservationID(ReservationID);
            if (reservationBL != null)
            {

                return reservationBL.RDTO;
            }
            else
            {
                return NotFound("Not Found reservation");
            }

        }
        [Authorize]
        [HttpDelete("DeleteReservationByUserID", Name = "DeleteReservationByUserID")]
        public ActionResult DeleteReservationByUserID(int UserID)
        {

            if (UserID < 1)
            {
                return BadRequest($"Not accepted ID {UserID}");
            }

            if (clsReservationBL.DeleteReservationByUserID(UserID))

                return Ok($"Reservations with UserID {UserID} has been deleted.");
            else
                return NotFound($"Reservations with UserID {UserID} not found. no rows deleted!");
        }

        [Authorize]
        [HttpPut("UpdateReservation", Name = "UpdateReservation")]
        public ActionResult UpdateReservation(RequestupdateReservation model)
        {
            if (model.ReservationID < 1 || model.SeatsCount < 1)
            {
                return BadRequest("Bad Request");
            }

            clsReservationBL reservationBL = new clsReservationBL();
            reservationBL.seatsCount = model.SeatsCount;
            reservationBL.ReservationID = model.ReservationID;
            reservationBL.TotalPrice = model.TotalPrice;
            reservationBL.Mode = clsReservationBL.enMode.Update;
            if (reservationBL.Save())
            {
                return Ok("Update Successfuly");
            }
            else
            {
                return NotFound("Update Field");
            }
        }

 }
    public class RequestAddNewReservation
    {
        public int ReservationID { get; set; }
        public int UserID { get; set; }
        public int TripID { get; set; }

        public int seatsCount { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime ReservationDate { get; set; }
        public enReservationStatus Status { get; set; }


    }
    public class RequestupdateReservation
    {
     public int ReservationID           { set; get;  }
     public int SeatsCount              { set; get;  }
     public decimal TotalPrice         { set; get;  }

    }

}
