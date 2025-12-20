using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;

namespace TravelBussinessLayer
{
   public  class clsCompanyBL
    {
      public  enum enMode {AddNew = 0,Update=1}
      public  enMode Mode { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public clsCompanyBL()
        {
            this .CompanyID = 0;
            this.CompanyName =" ";
            this .Address =" ";
            this .Phone =" ";
            this .CreatedAt = DateTime.Now;
            this .IsActive = true;
        }
        public clsCompanyBL (CompanyDTO companyDTO,enMode Mode =enMode.AddNew)
        {
            this .CompanyID = companyDTO.CompanyID; 
            this .CompanyName = companyDTO.CompanyName;
            this .Address = companyDTO.Address;
            this .Phone = companyDTO.Phone;
            this .CreatedAt = companyDTO.CreatedAt;
            this .IsActive = companyDTO.IsActive;
            this.Mode=Mode;
        }
        public CompanyDTO CDTO
        {
            get
            {
                return new CompanyDTO(this.CompanyID, this.CompanyName, this.Address, this.Phone, this.CreatedAt, this.IsActive);
            }
        }


        public static  List <CompanyDTO> GetAllCompanies()
        {
            return clsCompanyDA.GetAllCompanies();
        }
    


        public static clsCompanyBL GetCompanyByID(int CompanyID)
        {

            CompanyDTO Company = clsCompanyDA.GetCompanyByID(CompanyID);
            if (Company != null)
            {
                return new clsCompanyBL(Company);

            }
            else
            {
                return null;
            }

        }

        private bool _AddNewCompany()
        {
         return  ((this.CompanyID = clsCompanyDA.AddNewCompany(this.CDTO))!=-1);
        }
        private bool _UpdateCompany()
        {

            return clsCompanyDA.UpdateCompany(this.CDTO);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew: 
                    if (_AddNewCompany())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    case enMode.Update:
                    return _UpdateCompany();


            }
            return false;
        }

        public static bool DeleteCompany(int companyID)
        {
         
            clsCompanyBL company = clsCompanyBL.GetCompanyByID(companyID);

            if (company == null)
                return false;

            return clsCompanyDA.DeleteCompany(companyID);
        }

    }
}
