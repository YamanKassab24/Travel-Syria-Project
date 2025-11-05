using System.Reflection.Metadata;
using System;
using System.Data;
using Microsoft.Data.SqlClient;


namespace TravelDataAccess
{
    public class clsCountryDA
    {
        public class CountryDTO
        {
            public int CountryID { get; set; }
            public string CountryName { get; set; }


       public CountryDTO(int CountryID,string CountryName)
         {

                this .CountryID = CountryID;
                this .CountryName = CountryName;

         }

        }

       public static  CountryDTO GetCountryByID(int CountryID)
        {
            using (SqlConnection connection =new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetCountryByID", connection)) 
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CountryID",CountryID);
                    connection.Open();


                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new CountryDTO(reader.GetInt32(reader .GetOrdinal("CountryID")),reader.GetString(reader.GetOrdinal("CountryName")));
                        }
                        else
                        {
                            return null;
                        }

                    }
                }

            }
        }
        public static CountryDTO GetCountryByName(string CountryName)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetCountryByName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    connection.Open();


                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new CountryDTO(reader.GetInt32(reader.GetOrdinal("CountryID")), reader.GetString(reader.GetOrdinal("CountryName")));
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
