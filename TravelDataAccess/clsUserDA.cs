using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Win32.SafeHandles;

namespace TravelDataAccess
{
    public class UserDTO
    {


        public int UserID { get; set; }
        public string Role { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsActive { get; set; }
        public int PersonID { get; set; }
        public decimal WalletBalance { get; set; }

        public PersonDTO Person { get; set; }

        public UserDTO(int UserID, string Role, DateTime CreateAt, bool IsActive, int PersonID, decimal WalletBalance)
        {

            this.UserID = UserID;
            this.Role = Role;
            this.CreateAt = CreateAt;
            this.IsActive = IsActive;
            this.PersonID = PersonID;
            this.WalletBalance = WalletBalance;
            this.Person = clsPersonDA.GetPersonByID(this.PersonID);
        }


    }



    public class clsUserDA
    {


        public static  List<UserDTO> GetAllUsers()
        {
            List<UserDTO> Users = new List<UserDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {

                using (SqlCommand command = new SqlCommand("SP_GetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Users.Add(new UserDTO(
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Role")),
                                reader.GetDateTime(reader.GetOrdinal("CreateAt")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetDecimal(reader.GetOrdinal("WalletBalance"))

                            )
                            {

                            });

                        }


                    }


                }
            }
            return Users;
        }

        public static  UserDTO GetUserById(int UserID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {

                using (SqlCommand command = new SqlCommand("Sp_GetUserByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserDTO(
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Role")),
                                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetDecimal(reader.GetOrdinal("WalletBalance")));
                        }
                        else
                        {
                            return null;
                        }
                    }



                }
            }

        }

        public static UserDTO LoginWithEmailOrPhone(string EmailOrPhone,string Password)
        {
            
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command =new SqlCommand("Sp_LoginWithEmailOrPhone",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailOrPhone",EmailOrPhone);
                    command.Parameters.AddWithValue("@Password", Password);
                    connection.Open();

                   

                    object result = command.ExecuteScalar();
                    int userId = 0;

                    if (result != null && int.TryParse(result.ToString(), out userId))
                    {

                        UserDTO User = GetUserById(userId);
                        return User;
                       
                    }
                    else
                    {
                        
                        return null; 
                    }

                }

            }


        }


        public static int AddNewUser(UserDTO NewUser,PersonDTO NewPerson,string Password)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewUserWithPerson", connection))
                {
                    command.CommandType= CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", NewPerson.FirstName);
                    command.Parameters.AddWithValue("@LastName", NewPerson.LastName);
                    command.Parameters.AddWithValue("@Phone", NewPerson.Phone);
                    command.Parameters.AddWithValue("@Email", NewPerson.Email);
                    command.Parameters.AddWithValue("@Gender", NewPerson.IsMale);
                    command.Parameters.AddWithValue("DateOfBirth", NewPerson.DateOfBirth);
                    command.Parameters.AddWithValue("@Image", NewPerson.Image);
                    command.Parameters.AddWithValue("@CountryID", NewPerson.CountryID);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Role", NewUser.Role);
                    command.Parameters.AddWithValue("@IsActive", NewUser.IsActive);

                    var outputIdParam = new SqlParameter("@UserID", SqlDbType.Int)
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
}


