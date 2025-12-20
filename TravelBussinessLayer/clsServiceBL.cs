using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsServiceBL
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }

        public clsServiceBL(ServiceDTO Service, enMode NewMode = enMode.AddNew)
        {
            this.ServiceID = Service.ServiceID;
            this.ServiceName = Service.ServiceName;
            this.Description = Service.Description;
            this.Image = Service.Image;
            this.IsActive = Service.IsActive;
            this.Mode = NewMode;

        }
        public clsServiceBL()
        {
            this.ServiceID = -1;
            this.ServiceName = "";
            this.Description = "";
            this.Image = "";
            this.IsActive = false;
            this.Mode = enMode.AddNew;

        }

        public ServiceDTO SDTO
        {
            get
            {
                return new ServiceDTO(this.ServiceID, this.ServiceName, this.Description, this.Image, this.IsActive);
            }

        }


        public static List<ServiceDTO> GetAllService()
        {
            return clsServiceDA.GetAllService();

        }
        public static clsServiceBL GetServiceByID(int ID)
        {
            ServiceDTO ServiceDTO = clsServiceDA.GetServiceByID(ID);
            if (ServiceDTO != null)
            {

                return new clsServiceBL(ServiceDTO);
            }
            else
            {
                return null;
            }





        }

        public static bool AddNewServiceImage(int ServiceID, string ImagePath)
        {
            return clsServiceDA.AddNewSeviceImage(ImagePath, ServiceID);

        }

        public static string GetServiceImage(int ServiceID, string baseUrl)
        {

            string image = clsServiceDA.GetServiceImageByServiceID(ServiceID);



            image = $"{baseUrl}/api/images/GetImage/{image}";


            return image;
        }

        private bool _AddNewService()
        {
            return ((this.ServiceID = clsServiceDA.AddNewService(SDTO)) != -1);
        }
        private bool _UpdateService()
        {
            return (clsServiceDA.UpdateService(SDTO));
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (_AddNewService())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:

                    return _UpdateService();
            }
            return false;

        }

        public static bool DeleteService(int ServiceID)
        {

            return clsServiceDA.DeleteService(ServiceID);
        }
    }
}
