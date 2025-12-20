using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsCountryBL
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountryBL(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

       
        public clsCountryBL ()
        {
            this .CountryID = -1;
            this.CountryName = "";
        }

        public clsCountryBL(CountryDTO CountrySTO)
        {
            this.CountryID = CountrySTO.CountryID;
                this.CountryName=CountrySTO.CountryName;
        }
        
        public CountryDTO CountryDTO
        {
            get { return (new CountryDTO(this.CountryID, this.CountryName)); } 
        }

        public static clsCountryBL Find (int CountryID)
        {
            CountryDTO CountryDTO = clsCountryDA.GetCountryByID(CountryID);

            if (CountryDTO != null)
            {
                return new clsCountryBL(CountryDTO);
            }
            else
            {
                return null;
            }

        }

        static public List <CountryDTO> GetAllCountries()
        {
            return clsCountryDA.GetAllCountry();
        }
       
        public static clsCountryBL Find(string CountryName)
        {
            CountryDTO CountryDTO = clsCountryDA.GetCountryByName(CountryName);

            if (CountryDTO != null)
            {
                return new clsCountryBL(CountryDTO);
            }
            else
            {
                return null;
            }

        }


    }



}
