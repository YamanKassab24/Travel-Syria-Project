using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;
namespace TravelBussinessLayer
{
    public class clsCityBL
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }

        public int CountryID { set; get; }

        public clsCountryBL Country { get; set; }

        public clsCityBL(CityDTO CityDto, enMode NewMode = enMode.AddNew)
        {
            this.CityID = CityDto.CityID;
            this.CityName = CityDto.CityName;
            this.CountryID = CityDto.Country.CountryID;
            this.Country = clsCountryBL.Find(this.CountryID);
            this.Mode = NewMode;
        }
        public CityDTO CityDTO
        {
            get
            {
                return new CityDTO(this.CityID, this.CityName, this.CountryID);
            }
        }
        public clsCityBL()
        {
            this.CityID = -1;
            this.CityName = "";
            this.CountryID = -1;
            this.Mode = enMode.AddNew;
        }

        public static clsCityBL GetCityByCityID(int CityID)
        {

            CityDTO City = clsCityDA.GetCityByID(CityID);
            if (City != null)
            {
                return new clsCityBL(City);
            }
            else
            {
                return null;
            }

        }
        public static List<CityDTO> GetCityByCountryID(int CountryID)
        {
            List<CityDTO> cities=new List<CityDTO>();
            cities = clsCityDA.GetAllCitiesByCountryID(CountryID);
            if (cities != null)
            {
                return cities;
            }
            else
            {
                return null;
            }

        }
        public static clsCityBL GetCityByCityName(string CityName)
        {

            CityDTO City = clsCityDA.GetCityByName(CityName);
            if (City != null)
            {
                return new clsCityBL(City);
            }
            else
            {
                return null;
            }

        }


        private bool _AddNewCity()
        {
            return ((this.CityID = clsCityDA.AddNewCity(this.CityName, this.CountryID)) != -1);
        }
        private bool _UpdateCity()
        {
            return clsCityDA.UpdateCity(this.CityID, this.CityName, this.CountryID);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCity())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateCity();

            }
            return false;
        }


    

        
    }
}
