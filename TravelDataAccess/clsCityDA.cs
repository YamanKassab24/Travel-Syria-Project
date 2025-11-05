using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TravelDataAccess
{
   public  class CityDTO
    {
        public int CityID { set; get; }
        public string CityName { set; get; }

        public int CountryID { set; get; }

        public clsCountryDA.CountryDTO Country { set; get; }
        public CityDTO(int CityID, string CityName, int countryID)
        {

            this.CityID = CityID;
            this.CityName = CityName;
           this.CountryID = countryID;
           this .Country=clsCountryDA.GetCountryByID(this.CountryID);
        }

    }

  public class clsCityDA
    {
     
        public static CityDTO GetCityByID(int CityID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetCityByCityID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CityID", CityID);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (new CityDTO(
                                reader.GetInt32(reader.GetOrdinal("CityID")),
                                reader.GetString(reader.GetOrdinal("CityName")),
                                reader.GetInt32(reader.GetOrdinal("CountryID"))


                                ));

                        }
                        else
                        {
                            return null;
                        }



                    }

                }


            }

        }
        public static CityDTO GetCityByName(string  CityName)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetCityByCityName", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CityName", CityName);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (new CityDTO(
                                reader.GetInt32(reader.GetOrdinal("CityID")),
                                reader.GetString(reader.GetOrdinal("CityName")),
                                reader.GetInt32(reader.GetOrdinal("CountryID"))


                                ));

                        }
                        else
                        {
                            return null;
                        }



                    }

                }


            }

        }
    }
}
