using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TravelDataAccess
{

   public class ServiceDTO
    {
     public  int  ServiceID { get; set; }
     public  string   ServiceName { get; set; }
     public  string   Description  { get; set; }
     public  string?  Image {  get; set; }
     public   bool  IsActive { get; set; }


        public ServiceDTO(int ServiceID,string ServiceName ,string Description,string? Image,bool IsActive) 
        { 
            this .ServiceID = ServiceID;
            this .ServiceName = ServiceName;
            this .Description = Description;
            this .Image = Image;
            this .IsActive = IsActive;
        }

    }





    public class clsServiceDA
    {
        public static List<ServiceDTO> GetAllService()
        {
            List<ServiceDTO> services = new List<ServiceDTO>();

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllServices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            services.Add(new ServiceDTO(
                                    reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                    reader.GetString(reader.GetOrdinal("ServiceName")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                ));


                        }



                    }

                }

            }
            return services;

        }
        public static ServiceDTO GetServiceByID(int ServiceID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetServiceByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ServiceDTO(
                                    reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                    reader.GetString(reader.GetOrdinal("ServiceName")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
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



        public static bool AddNewSeviceImage(string ImgaePath, int ServiceID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewServiceImage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    command.Parameters.AddWithValue("@Image", ImgaePath);

                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;



                }








            }

        }



        public static string GetServiceImageByServiceID(int ServiceID)
        {


            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetServiceImage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(reader.GetOrdinal("Image"));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

        }


        public static int AddNewService(ServiceDTO service)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewService", connection))
                {
                    command.Parameters.AddWithValue("@ServiceName", service.ServiceName);
                    command.Parameters.AddWithValue("@Description", service.Description);
                    command.Parameters.AddWithValue("@IsActive", service.IsActive);
                    command.CommandType = CommandType.StoredProcedure;
                    var outputIdParam = new SqlParameter("@ServiceID", SqlDbType.Int)
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

        public static bool UpdateService(ServiceDTO service)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_UpdateService", connection))
                {
                    command.Parameters.AddWithValue("@ServiceID", service.ServiceID);

                    command.Parameters.AddWithValue("@ServiceName", service.ServiceName);
                    command.Parameters.AddWithValue("@Description", service.Description);
                    command.Parameters.AddWithValue("@IsActive", service.IsActive);
                    command.CommandType = CommandType.StoredProcedure;



                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;




                }
            }
        }

        public static bool DeleteService(int ServiceID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteService", connection))
                {
                    command.Parameters.AddWithValue("@ServiceID",ServiceID);
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;




                }
            }



        }




    }
    }

            