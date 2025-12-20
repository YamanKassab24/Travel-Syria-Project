using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsTripBL
    {

        public enum enMode { AddNew=0,Update =1}
        public enMode Mode = enMode.AddNew;
        public int TripID { get; set; }
        public int CompanyID { get; set; }

        public int DepartureCityID { get; set; }
        public int ArrivalCityID { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public enTripStatus? Status { get; set; }

        public string Details { get; set; }

        public byte? Rating { get; set; }

        public int SeatsAvailable { set; get; }

        public int MaxSeats { set; get; }
        public clsTripBL(TripDTO Trip,enMode Mode=enMode.AddNew)
        { 
        this.TripID=Trip.TripID;
            this.CompanyID = Trip.Company.CompanyID;
            this.DepartureCityID=Trip.DepartureCity.CityID;
            this.ArrivalCityID=Trip.ArrivalCity.CityID;
            this.DepartureTime = Trip.DepartureTime;    
            this.ArrivalTime = Trip.ArrivalTime;
            this.Price = Trip.Price;
           this.CreatedAt=Trip.CreatedAt;
            this.Status = Trip.Status;
            this.Details = Trip.Details;
            this.Rating = Trip.Rating;
            this.MaxSeats = Trip.MaxSeats;
            this.SeatsAvailable = Trip.SeatsAvailable;
            this.Mode=Mode;
        
        }
        public clsTripBL()
        {
            this.TripID = -1;
            this.CompanyID = -1;
            this.DepartureCityID = -1;
            this.ArrivalCityID = -1;
            this.DepartureTime = DateTime.Now;
            this.ArrivalTime = DateTime.Now;
            this.Price = 0;
            this.CreatedAt = DateTime.Now;
            this.Status = enTripStatus.Cancelled;
            this.Details = "";
            this.Rating = 0;
            this.MaxSeats = 0;
            this.SeatsAvailable = 0;


            this.Mode = Mode;

        }

        public TripDTO TDTO
        {

            get
            {
                return new TripDTO(this.TripID,this.CompanyID,this.DepartureCityID,this.ArrivalCityID,this.DepartureTime,this.ArrivalTime,this.Price,this.CreatedAt,this.Status,this.Details,this.Rating,this.SeatsAvailable,this.MaxSeats);
            }
        }

        public static  List<TripDTO> GetAllTrips()
        {

            List<TripDTO> Trips = clsTripDA.GetAllTrips();

           return Trips;

        }
        public static clsTripBL GetTripByID(int TripID)
        {
           TripDTO trip = clsTripDA.GetTripByID(TripID);
            if (trip != null)
            {

                return new clsTripBL(trip);


            }
            else
            {
                return null;
            }

        }


        private bool _AddNewTrip()
        {
            this.TripID = clsTripDA.AddNewTrip(TDTO);
            return (this.TripID != -1);
        }
        private bool _UpdateTrip()
        {
           return clsTripDA.UpdateTrip(TDTO);
        }
        public bool Save()
        {
            switch (this.Mode)
            {

                 case enMode.AddNew:
                    
                        if (_AddNewTrip())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                         return false;
                  

                   case enMode.Update:

                   return  _UpdateTrip();
                  


            }
            return false;
        }

        public static bool DelteTrip(int TripID)
        {
            return clsTripDA.DeleteTrip(TripID);
        }
    }
}
