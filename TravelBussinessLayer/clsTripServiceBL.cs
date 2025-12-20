using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsTripServiceBL
    {

     public    enum enMode { AddNew=0, Update=1}
     public    enMode Mode { get; set; }
        public int TripServiceID { get; set; }
        public int TripID { get; set; }
        public int ServiceID { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public clsTripServiceBL(TripServiceDTO TripService,enMode NewMode=enMode.AddNew)
        { 
            this.TripServiceID = TripService.TripServiceID;
            this.TripID = TripService.TripID;
            this.ServiceID=TripService.Service.ServiceID;
            this.Price = TripService.Price;
            this.IsActive =TripService.IsActive;
            this.Mode = NewMode;
        
        }

        public clsTripServiceBL()
        {
            this.TripServiceID = -1;
            this.TripID = -1;
            this.ServiceID = -1;
            this.Price = -1;
            this.IsActive =false ;
            this.Mode = enMode.AddNew;

        }

        public TripServiceDTO TSDTO
        {

            get
            {
                return new TripServiceDTO(this.TripServiceID, this.TripID, this.ServiceID, this.Price, this.IsActive);
            }
        }



        public static List<TripServiceDTO> GetAllTripService()
        {
            return clsTripServiceDA.GetAllTripService();

        }


        public static clsTripServiceBL GetTripServiceByTripServicID(int ID)
        {
            TripServiceDTO TripService = clsTripServiceDA.GetTripServiceByTripServiceID(ID);
            if (TripService != null)
            {

                return new clsTripServiceBL(TripService); ;
            }
            else
            {
                return null;
            }





        }

        public static List<TripServiceDTO> GetTripServiceByTripID(int TripID)
        {
            return clsTripServiceDA.GetTripServiceByTripID(TripID);

        }

        public static List<TripServiceDTO> GetTripServiceByServiceID(int ServiceID)
        {
            return clsTripServiceDA.GetTripServiceByServiceID(ServiceID);

        }


       private bool _AddNewTripServiceID()
        {
            return ((this.TripServiceID=clsTripServiceDA.AddNewTripService(TSDTO)) != -1);
        }
        private bool _UpdateTripServiceID()
        {
            return clsTripServiceDA.UpdateTripService(TSDTO);
        }
        public bool Save()
        {

            switch (this.Mode)
            {
                case enMode.AddNew: 


                    if (_AddNewTripServiceID())
                    {
                   this .Mode = enMode.Update;
                        return true;
                    }
                    else { return false; }
                    case enMode.Update:
                    return _UpdateTripServiceID();
                   
            }
            return false;

        }


        public static bool DeleteTripServiceByServiceID(int ServiceID)
        {
            return clsTripServiceDA.DeleteTripserviceByServiceID(ServiceID);
        }

        public static bool DeleteTripServiceByTripID(int TripID)
        {
            return clsTripServiceDA.DeleteTripserviceByTripID(TripID);
        }

        public static bool DeleteTripServiceByTripServiceID(int TripServiceID)
        {
            return clsTripServiceDA.DeleteTripserviceByTripServiceID(TripServiceID);
        }

        public static bool DeleteTripServiceByTripIdAndServiceID(int TripID,int ServiceID)
        {
            return clsTripServiceDA.DeleteTripserviceByTripIdAndServiceId(TripID,ServiceID);
        }
    }
}
