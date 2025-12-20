using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataAccess
{
    public class TripServiceDTO
    {
    public int TripServiceID { get; set; }
    public int TripID { get; set; }
    public ServiceDTO Service { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }


        public TripServiceDTO(int TripServiceID,int TripID,int ServiceID,decimal Price ,bool IsActive)
        { 
        
            this.TripServiceID = TripServiceID; 
            this.TripID = TripID;
            this.Service=clsServiceDA.GetServiceByID(ServiceID);
            this.Price = Price;
            this.IsActive = IsActive;
        
        }

    }
    public  class clsTripServiceDA
    {


        public static List<TripServiceDTO> GetAllTripService()
        {
            List<TripServiceDTO> Tripservices = new List<TripServiceDTO>();

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllTripServices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tripservices.Add(new TripServiceDTO(
                                    reader.GetInt32(reader.GetOrdinal("TripServiceID")),
                                         reader.GetInt32(reader.GetOrdinal("TripID")),
                                         reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                    reader .GetDecimal(reader.GetOrdinal("Price")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                ));


                        }



                    }

                }

            }
            return Tripservices;

        }

        public static TripServiceDTO GetTripServiceByTripServiceID(int TripServiceID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetTripServiceByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TripServiceID", TripServiceID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TripServiceDTO(
                                    reader.GetInt32(reader.GetOrdinal("TripServiceID")),
                                         reader.GetInt32(reader.GetOrdinal("TripID")),
                                         reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                    reader.GetDecimal(reader.GetOrdinal("Price")),
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

        public static List<TripServiceDTO> GetTripServiceByTripID(int TripID)
        {
            List<TripServiceDTO> TripServices = new List<TripServiceDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetTripServiceByTripID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TripID", TripID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TripServices.Add( new TripServiceDTO(
                                    reader.GetInt32(reader.GetOrdinal("TripServiceID")),
                                         reader.GetInt32(reader.GetOrdinal("TripID")),
                                         reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                    reader.GetDecimal(reader.GetOrdinal("Price")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                ));

                        }
                       


                    }




                }


            }


            return TripServices;


        }

        public static List<TripServiceDTO> GetTripServiceByServiceID(int ServiceID)
        {
            List<TripServiceDTO> TripServices = new List<TripServiceDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetTripServiceByServiceID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TripServices.Add(new TripServiceDTO(
                                    reader.GetInt32(reader.GetOrdinal("TripServiceID")),
                                         reader.GetInt32(reader.GetOrdinal("TripID")),
                                         reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                    reader.GetDecimal(reader.GetOrdinal("Price")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                ));

                        }



                    }




                }


            }


            return TripServices;


        }

        public static int AddNewTripService(TripServiceDTO tripServiceDTO)
        {

            using (SqlConnection connection =new SqlConnection (GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewTripService", connection))
                {
                    command.Parameters.AddWithValue("@TripID", tripServiceDTO.TripID);
                    command.Parameters.AddWithValue("@ServiceID", tripServiceDTO.Service.ServiceID);
                    command.Parameters.AddWithValue("@Price", tripServiceDTO.Price);
                    command.Parameters.AddWithValue("@IsActive", tripServiceDTO.IsActive);

                    command.CommandType = CommandType.StoredProcedure;


                    var outputIdParam = new SqlParameter("@TripServiceID", SqlDbType.Int)
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

        public static bool UpdateTripService(TripServiceDTO tripServiceDTO)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_UpdateTripService", connection))
                {
                    command.Parameters.AddWithValue("@TripID", tripServiceDTO.TripID);
                    command.Parameters.AddWithValue("@ServiceID", tripServiceDTO.Service.ServiceID);
                    command.Parameters.AddWithValue("@Price", tripServiceDTO.Price);
                    command.Parameters.AddWithValue("@IsActive", tripServiceDTO.IsActive);
                    command.Parameters.AddWithValue("@TripServiceID", tripServiceDTO.TripServiceID);
                    command.CommandType = CommandType.StoredProcedure;


                 

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;

                }




            }







        }

        public static bool DeleteTripserviceByServiceID(int ServiceID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteTripServiceByServiceID", connection))
                {
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;




                }
            }



        }

        public static bool DeleteTripserviceByTripID(int TripID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteTripServiceByTripID", connection))
                {
                    command.Parameters.AddWithValue("@TripID", TripID);
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;




                }
            }



        }

        public static bool DeleteTripserviceByTripServiceID(int TripServiceID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteTripServiceByTripServiceID", connection))
                {
                    command.Parameters.AddWithValue("@TripServiceID", TripServiceID);
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;




                }
            }



        }

        public static bool DeleteTripserviceByTripIdAndServiceId(int TripID,int ServiceID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteTripServiceByTripIDAndServiceID", connection))
                {
                    command.Parameters.AddWithValue("@TripID", TripID);
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);

                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;




                }
            }



        }
    }
}
