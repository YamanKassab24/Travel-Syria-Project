using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataAccess
{

    public class CompanyDTO
    {

        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

    public  CompanyDTO (int CompanyID,string CompanyName ,string Address ,string Phone ,DateTime CreatedAt,bool IsActive)
        {
            this .CompanyID = CompanyID;    
            this .CompanyName = CompanyName;
            this .Address = Address;
            this .Phone = Phone;
            this .CreatedAt = CreatedAt;
            this .IsActive = IsActive;
        }
        public CompanyDTO()
        {
            this.CompanyID = -1;
            this.CompanyName = "";
            this.Address = "";
            this.Phone = "";
            this.CreatedAt = DateTime.Now;
            this .IsActive = false;
        }

    }

    public class clsCompanyDA
    {
        public static  List <CompanyDTO > GetAllCompanies()
        {

            List <CompanyDTO> Companies = new List <CompanyDTO> ();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command =new SqlCommand ("Sp_GetAllCompanies ", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                     
                    connection.Open ();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Companies.Add(new CompanyDTO(
                                 reader.GetInt32(reader.GetOrdinal("CompanyID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                ));

                        }
                    }

                }

            }

            return Companies;




        }
        public static  CompanyDTO GetCompanyByID(int CompanyID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetCompanyByID ", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyID", CompanyID);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CompanyDTO(
                                     reader.GetInt32(reader.GetOrdinal("CompanyID")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Address")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                    );

                        }
                        else
                        {
                            return null;
                        }
                    }

                }

            }


        }

        public static int AddNewCompany(CompanyDTO NewCompany)
        {
            int companyID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_AddNewCompany", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CompanyName", NewCompany.CompanyName);
                        command.Parameters.AddWithValue("@Address", NewCompany.Address);
                        command.Parameters.AddWithValue("@Phone", NewCompany.Phone);
                        command.Parameters.AddWithValue("@IsActive", NewCompany.IsActive);

                        SqlParameter outputIdParam = new SqlParameter(
                            "@CompanyID",
                            SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        companyID = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return companyID;
        }

      
        public static bool UpdateCompany(CompanyDTO Company)
        {
           

            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_UpdateCompany", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CompanyID", Company.CompanyID );
                        command.Parameters.AddWithValue("@CompanyName", Company.CompanyName);
                        command.Parameters.AddWithValue("@Address", Company.Address);
                        command.Parameters.AddWithValue("@Phone", Company.Phone);
                        command.Parameters.AddWithValue("@IsActive", Company.IsActive);

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return false;
        }

        public static bool DeleteCompany(int companyID)
        {
         

            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                using (SqlCommand command = new SqlCommand("Sp_DeleteCompany", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyID", companyID);

                    connection.Open();
                     command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return false;
        }

    }
}
