using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Service;
using System.Reflection;
using TravelBussinessLayer;
using TravelDataAccess;
namespace TravelProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {


        [AllowAnonymous]
        [HttpGet("GetAllTrips", Name = "GetAllTrips")]
        public ActionResult<TripDTO> GetAllTrips()
        {

            List<TripDTO> trips = new List<TripDTO>();
            trips = clsTripBL.GetAllTrips();
            if (trips.Count <= 0)
            {

                return NotFound("Not Found Trips ");
            }
            return Ok(trips);

        }


        [AllowAnonymous]
        [HttpGet("GetTripByID", Name = "GetTripByID")]
        public ActionResult<TripDTO> GetTripByID(int TripID)
        {

            if (TripID <= 0)
            {
                return BadRequest("Bad request Chose Another ID");
            }
            clsTripBL Trip = clsTripBL.GetTripByID(TripID);
            if (Trip != null)
            {
                return Ok(Trip.TDTO);
            }
            else
            {
                return NotFound("Not Found Trip With This ID");
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpPost("AddNewTrip", Name = "AddNewTrip")]
        public  ActionResult<TripDTO> AddNewTrip(TripRequest Model)
        {

            if (Model == null ||
                Model.CompanyID <= 0 ||
                Model.DepartureCityID <= 0 ||
                Model.ArrivalCityID <= 0 ||
                Model.Price <= 0)
            {
                return BadRequest("Bad Request: invalid trip data.");
            }


            clsTripBL tripBL = new clsTripBL();

            tripBL.CompanyID = Model.CompanyID;
            tripBL.DepartureCityID = Model.DepartureCityID;
            tripBL.ArrivalCityID = Model.ArrivalCityID;
            tripBL.DepartureTime = Model.DepartureTime;
            tripBL.ArrivalTime = Model.ArrivalTime;
            tripBL.Price = Model.Price;
            tripBL.Status = Model.Status;
            tripBL.Details = Model.Details;
            tripBL.Rating = Model.Rating;
            tripBL.SeatsAvailable = Model.MaxSeats;
            tripBL.MaxSeats = Model.MaxSeats;

            if (tripBL.Save())
            {

              

                return tripBL.TDTO;
            }
            else
            {
                return NotFound("Failed to add trip.");
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateTrip", Name = "UpdateTrip")]
        public ActionResult<TripDTO>UpdateTrip(int TripID,TripRequest Model)
        {


            clsTripBL tripBL = new clsTripBL();
            tripBL.TripID = TripID;
            tripBL.CompanyID = Model.CompanyID;
            tripBL.DepartureCityID = Model.DepartureCityID;
            tripBL.ArrivalCityID = Model.ArrivalCityID;
            tripBL.DepartureTime = Model.DepartureTime;
            tripBL.ArrivalTime = Model.ArrivalTime;
            tripBL.Price = Model.Price;
            tripBL.Status = Model.Status;
            tripBL.Details = Model.Details;
            tripBL.Rating = Model.Rating;
           
            tripBL.MaxSeats=Model.MaxSeats;
            tripBL.Mode = clsTripBL.enMode.Update;

            if (tripBL.Save())
            {
               tripBL=clsTripBL.GetTripByID(TripID);
                return tripBL.TDTO;
            }
            else
            {
                return NotFound("Failed to Update trip.");
            }







        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("DeletTripByTripID", Name = "Delete TripByTripID")]
        public ActionResult DeleteTripByTripID(int TripID)
        {

            if (TripID <= 0)
            {
                return BadRequest("BadRequest Chose Another TripID");
            }
            if (clsTripBL.DelteTrip(TripID))
            {
                return Ok("Deleted Successfuly");
            }
            else
            {
                return NotFound($"Not Found Trip With ID {TripID}");
            }
        }
        


    }
  public class TripRequest
    {


   public int CompanyID                      {set; get;}
   public int DepartureCityID               {set; get;}
   public int ArrivalCityID                {set; get;}
   public DateTime DepartureTime           {set; get;}
   public DateTime ArrivalTime             {set; get;}
   public decimal Price                    {set; get;}
   public enTripStatus   Status             {set; get;}
   public string?   Details                 {set; get;}
   public byte? Rating                       { set; get; }

   public int MaxSeats {set; get;}
  


    }
   

}
