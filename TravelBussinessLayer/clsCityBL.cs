using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;
namespace TravelBussinessLayer
{
   public   class clsCityBL
    {

        public int CityID { get; set; }
        public string CityName { get; set; }

        public int CountryID { set ; get; }

        public clsCountryBL Country { get; set; }

        public clsCityBL(CityDTO CityDto)
        {
            this .CityID = CityDto.CityID;
            this .CityName = CityDto.CityName;
            this .CountryID = CityDto.CountryID;
            this.Country = clsCountryBL.Find(this.CountryID);

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
            this .CountryID = -1;
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


    }
}
