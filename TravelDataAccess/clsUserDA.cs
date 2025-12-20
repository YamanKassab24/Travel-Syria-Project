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

        public PersonDTO Person { get; set; } = new PersonDTO(
        -1, "", "", "", "", true, DateTime.Now, "", -1
    );

        public UserDTO(int UserID, string Role, DateTime CreateAt, bool IsActive, int PersonID, decimal WalletBalance)
        {

            this.UserID = UserID;
            this.Role = Role;
            this.CreateAt = CreateAt;
            this.IsActive = IsActive;
            this.PersonID = PersonID;
            this.WalletBalance = WalletBalance;
            if(this.PersonID!=-1)
            this.Person = clsPersonDA.GetPersonByID(this.PersonID);
        }
        public UserDTO()
        {
            this.UserID = -1;
            this.Role = "";
            this.CreateAt = DateTime.Now;
            this.IsActive = true;
            this.WalletBalance = 0;
           
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
                        while (reader.Read())
                        {
                            Users.Add(new UserDTO(
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Role")),
                                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetDecimal(reader.GetOrdinal("WalletBalance"))

                            )
                            );

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


        public static int AddNewUser(UserDTO NewUser,string Password)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewUserWithPerson", connection))
                {
                    command.CommandType= CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName",NewUser.Person.FirstName);
                    command.Parameters.AddWithValue("@LastName", NewUser.Person.LastName);
                    command.Parameters.AddWithValue("@Phone", NewUser.Person.Phone);
                    command.Parameters.AddWithValue("@Email", NewUser.Person.Email);
                    command.Parameters.AddWithValue("@Gender", NewUser.Person.IsMale);
                    command.Parameters.AddWithValue("DateOfBirth", DateTime.Now);
                    command.Parameters.AddWithValue("@Image", NewUser.Person.Image);
                    command.Parameters.AddWithValue("@CountryID", NewUser.Person.CountryID);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Role", NewUser.Role);
                    command.Parameters.AddWithValue("@IsActive", true);

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

        public static bool IsCorrectCurrentPassword(int UserID , string Password)
        {
            bool IsFound = false;


            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_IsCorrectCurantPassword", connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            IsFound = true;
                        }



                    }


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return IsFound;
        }
        public static bool UpdatePassword (int UserID,string CurrentPassword,string NewPassword)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_UpdatePassword ", connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@NewPassword",NewPassword);
                        command.Parameters.AddWithValue("@CurrentPassword", CurrentPassword);

                        command.CommandType = CommandType.StoredProcedure;

                        //SqlDataReader reader = command.ExecuteReader();
                        //if (reader.HasRows)
                        //{
                        //    IsFound = true;
                        //}
                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;

                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }




        }
        public static bool DeleteUserByUserID(int UserID )
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command= new SqlCommand("Sp_DeleteUserByUserID",connection))
                {

                    command.Parameters.AddWithValue("@UserID",UserID);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                    return  true;


                }



            }



        }
       

    }
}


