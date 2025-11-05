using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataAccess
{
    public class PersonDTO
    {
        public int PersonID { set; get;  }
        public  string FirstName { set; get; }

        public string LastName { set; get; }

        public string Phone { set; get; }

        public string Email { set; get; }

        public bool IsMale { set; get; }

        public DateTime DateOfBirth {  set; get; }

        public string Image    { set; get; }

        public int CountryID { set; get; }

        public clsCountryDA.CountryDTO Country { set; get; }

        public PersonDTO(int PersonID,string FirstName,string LastName ,string Phone ,string Email, bool IsMale, DateTime DateOfBirth ,string Image ,int CountryID) 
        {
            

            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.Email = Email;
            this.IsMale = IsMale;
            this.DateOfBirth = DateOfBirth;
            this.Image = Image;
            this.CountryID = CountryID;
            this.Country = clsCountryDA.GetCountryByID(this.CountryID);
        
        }    
    }

    public class clsPersonDA
    {


        public static  PersonDTO GetPersonByID(int PersonID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetPersonByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return (new PersonDTO(
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                 reader.GetString(reader.GetOrdinal("phone")),
                                  reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetBoolean(reader.GetOrdinal("Gender")),
                                  reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                  reader.GetString(reader.GetOrdinal("Image")),
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

        public static int AddNewPerson(PersonDTO Person)
        {
            using (var connection = new SqlConnection(GlobalClass._connectionString))
            using (var command = new SqlCommand("Sp_AddNewPerson", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", Person.FirstName);
                command.Parameters.AddWithValue("@LastName", Person.LastName);
                command.Parameters.AddWithValue("@Phone", Person.Phone);
                command.Parameters.AddWithValue("@Email", Person.Email);

                command.Parameters.AddWithValue("@Gender", Person.IsMale);
                command.Parameters.AddWithValue("DateOfBirth", Person.DateOfBirth);
                command.Parameters.AddWithValue("@Image", Person.Image);
                command.Parameters.AddWithValue("@CountryID", Person.CountryID);
                var outputIdParam = new SqlParameter("@PersonID", SqlDbType.Int)
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
}
