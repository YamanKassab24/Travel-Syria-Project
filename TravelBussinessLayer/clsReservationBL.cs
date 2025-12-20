using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public  class clsReservationBL
    {
        public enum enMode { AddNew=0,Update=1};
        public  enMode Mode = enMode.AddNew;

        public  int ReservationID {  get; set; }
        public int UserID { get; set; }
        public int TripID { get; set; }

      public   int seatsCount { get; set; }

        public DateTime ReservationDate { get; set; }
      public   decimal TotalPrice { get; set; } 

       public  enReservationStatus Status { get; set; }



        public clsReservationBL(ReservationDTO Reservation, enMode NewMode=enMode.AddNew)
        {
            this.ReservationID = Reservation.ReservationID;
            this.UserID=Reservation.User.UserID;
            this.TripID = Reservation.Trip.TripID;
            this.seatsCount = Reservation.SeatsCount;
            this.TotalPrice= Reservation.TotalPrice;
            this.ReservationDate = Reservation.ReservationDate;
            this.Status=Reservation.Status;
            this.Mode= NewMode;

        }

        public clsReservationBL()
        {
            this.ReservationID = -1;
            this.UserID = -1;
            this.TripID = -1;
            this.seatsCount = -1;
            this.TotalPrice = 0;
            this.ReservationDate =DateTime.Now;
            this.Status = enReservationStatus.Cancelled;
            this.Mode = enMode.AddNew;

        }

        public ReservationDTO RDTO
        {

            get
            {
                return new ReservationDTO(this.ReservationID,this.UserID,this.TripID,this.seatsCount,this.ReservationDate,this.TotalPrice,this.Status);
            }
        }

        public static List<ReservationDTO> GetAllReservations()
        {

            return clsReservationDA.GetAllReservations();
        }
        public static List<ReservationDTO> GetAllReservationsByUserID(int UserID)
        {

            return clsReservationDA.GetAllReservationsByUserID(UserID);
        }
        public static List<ReservationDTO> GetAllReservationsByTripID(int TripID)
        {

            return clsReservationDA.GetAllReservationsByTripID(TripID);
        }
         
        public static  clsReservationBL GetReservationByReservationID(int ReservationID)
        {
           ReservationDTO reservation =clsReservationDA.GetReservationByReservationID(ReservationID);
            if (reservation != null)
            {


                return new clsReservationBL(reservation);
            }
           else
            {
                return null;
            }

        }
       private  bool _AddNewReservation()
        {
            this.ReservationID = clsReservationDA.AddNewReservation(RDTO);
            return (this.ReservationID != -1);
        }
        private bool _UpdateReservation()
        {

            return clsReservationDA.UpdateReservation(this.seatsCount, this.ReservationID,this.TotalPrice);

        }
        public static bool DeleteReservationByReservationID(int ReservationID)
        {

            return clsReservationDA.DeleteReservationByReservationID(ReservationID);
        }
        public static bool DeleteReservationByUserID(int UserID)
        {

            return clsReservationDA.DeleteReservationByUserID(UserID);
        }


        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewReservation())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    case enMode.Update:
                    _UpdateReservation();
                    return true;

            }
            return false;



        }
    }
}
