using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsReservationServiceBL
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        public int ReservationServiceID { get; set; }
        public int ReservationID { set; get; }

        public int ServiceID { get; set; }

        public decimal Price { set; get; }

        public clsReservationServiceBL()
        {
            this.ReservationServiceID = -1;
            this.ReservationID = -1;
            this.ServiceID = -1;
            this.Price = 0;
            this.Mode = enMode.AddNew;

        }
        public clsReservationServiceBL(ReservationServiceDTO ReservationService, enMode NewMode = enMode.AddNew)
        {
            this.ReservationServiceID = ReservationService.ReservationServiceID;
            this.ReservationID = ReservationService.Reservation.ReservationID;
            this.ServiceID = ReservationService.Service.ServiceID;
            this.Price = ReservationService.Price;
            this.Mode = NewMode;

        }

        public ReservationServiceDTO RSDTO
        {
            get
            {
                return new ReservationServiceDTO(this.ReservationServiceID, this.ReservationID, this.ServiceID, this.Price);
            }
        }

        public static List <ReservationServiceDTO> GetAllReservationsServices()
        {
            return clsReservationServiceDA.GetAllReservationsServices();
        }

        public static List<ReservationServiceDTO> GetAllReservationsServicesByReservationID(int ReservationID)
        {
            return clsReservationServiceDA.GetAllReservationsServicesByReservationID(ReservationID);
        }

        public static List<ReservationServiceDTO> GetAllReservationsServicesByServiceID(int ServiceID)
        {
            return clsReservationServiceDA.GetAllReservationsServicesByServiceID(ServiceID);
        }
        private  bool _AddNewReservationService()
        {
            if ((this.ReservationServiceID= clsReservationServiceDA.AddNewReservationService(this.ReservationID,this.ServiceID,this.Price)) != -1)
            {
                return true;

            }
            return false;
        }

       public static bool DeleteAllReservationServiceByReservationID(int ReservationID)
        {
            return clsReservationServiceDA.DeleteReservationServiceByReservationID(ReservationID);
        }
        public static bool DeleteAllReservationServiceByReservationIDAndserviceID(int ReservationID,int ServiceID)
        {
            return clsReservationServiceDA.DeleteReservationServiceByReservationIDAndServiceID(ReservationID,ServiceID);
        }
        public bool Save()
        {


           switch(this.Mode)
                {

                case enMode.AddNew:

                    if (_AddNewReservationService())
                    {
                        this.Mode=enMode.Update;
                        return true;
                    }
                    return false;
                    case enMode.Update:
                    return false;

            }


            return false;
        }

        public static bool  CheckReservationService(int ReservationID,int ServiceID)
        {
            return clsReservationServiceDA.CheckReservationService(ReservationID, ServiceID);   
        }
    }


}
