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

        private  int CountryID { set; get; }

        public CountryDTO Country { set; get; }
        public CityDTO(int CityID, string CityName, int countryID)
        {

            this.CityID = CityID;
            this.CityName = CityName;
           this.CountryID = countryID;
           this .Country=clsCountryDA.GetCountryByID(countryID);
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

        public static List<CityDTO> GetAllCitiesByCountryID(int CountryID)
        {
            List<CityDTO > cities = new List<CityDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllCitiesByCountryID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             cities.Add( new CityDTO(
                                reader.GetInt32(reader.GetOrdinal("CityID")),
                                reader.GetString(reader.GetOrdinal("CityName")),
                                reader.GetInt32(reader.GetOrdinal("CountryID"))


                                ));

                        }
                        



                    }

                }


            }
            return cities;
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

        public static int AddNewCity(string CityName, int CountryID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_AddNewCity", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CityName", CityName);
                        command.Parameters.AddWithValue("@CountryID", CountryID);
                       
                        var outputIdParam = new SqlParameter("@CityID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        connection.Open();

                        command.ExecuteNonQuery();

                        return (int)outputIdParam.Value;
                    }

                }
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.ToString());
            }

            return -1;


        }

        public static bool UpdateCity(int CityID,string CityName, int CountryID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_UpdateCity", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CityName", CityName);
                        command.Parameters.AddWithValue("@CountryID", CountryID);
                        command.Parameters.AddWithValue("@CityID",CityID);
                       

                        connection.Open();

                        command.ExecuteNonQuery();

                        return true ;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false ;
            }

         




        }


    }
}
