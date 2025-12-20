using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataAccess
{
    public class ReservationServiceDTO
    {
       public int ReservationServiceID {  get; set; }
        public ReservationDTO Reservation { get; set; }
        public ServiceDTO Service { get; set; }

        public decimal Price { get; set; }


        public ReservationServiceDTO(int ReservationServiceID, int ReservationID, int ServiceID, decimal Price)
        {
            this.ReservationServiceID = ReservationServiceID;
            this.Reservation = clsReservationDA.GetReservationByReservationID(ReservationID);
            this.Service=clsServiceDA.GetServiceByID(ServiceID);
            this.Price = Price;

        }


    }


    public  class clsReservationServiceDA
    {


        public static List<ReservationServiceDTO> GetAllReservationsServices()
        {
            List<ReservationServiceDTO> reservations = new List<ReservationServiceDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllReservationService", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new ReservationServiceDTO(
                                 reader.GetInt32(reader.GetOrdinal("ReservationServiceID")),
                                 reader.GetInt32(reader.GetOrdinal("ReservationID")),
                                 reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                 reader.GetDecimal(reader.GetOrdinal("Price"))

                                ));
                        }

                    }

                }

            }





            return reservations;
        }

        public static List<ReservationServiceDTO> GetAllReservationsServicesByReservationID(int ReservationID)
        {
            List<ReservationServiceDTO> reservations = new List<ReservationServiceDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllReservationServiceByReservationID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReservationID", ReservationID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new ReservationServiceDTO(
                                 reader.GetInt32(reader.GetOrdinal("ReservationServiceID")),
                                 reader.GetInt32(reader.GetOrdinal("ReservationID")),
                                 reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                 reader.GetDecimal(reader.GetOrdinal("Price"))

                                ));
                        }

                    }

                }

            }





            return reservations;
        }

        public static List<ReservationServiceDTO> GetAllReservationsServicesByServiceID(int ServiceID)
        {
            List<ReservationServiceDTO> reservations = new List<ReservationServiceDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllReservationServiceByServiceID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new ReservationServiceDTO(
                                 reader.GetInt32(reader.GetOrdinal("ReservationServiceID")),
                                 reader.GetInt32(reader.GetOrdinal("ReservationID")),
                                 reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                 reader.GetDecimal(reader.GetOrdinal("Price"))

                                ));
                        }

                    }

                }

            }





            return reservations;
        }

        public static int AddNewReservationService(int ReservationID,int ServiceID,decimal Price)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewReservationService", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReservationID", ReservationID);
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    command.Parameters.AddWithValue("@Price", Price);
                   
                    var outputIdParam = new SqlParameter("@ReservationServiceID", SqlDbType.Int)
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


        public static bool DeleteReservationServiceByReservationID(int ReservationID)
        {

            using (SqlConnection connection=new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteAllReservationServiceByReservationID", connection))
                {
                    command.Parameters.AddWithValue("@ReservationID",ReservationID);
                    command.CommandType=CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;

                }

            }
            return false;
        }

        public static bool DeleteReservationServiceByReservationIDAndServiceID(int ReservationID,int ServiceID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteReservationServiceByReservationIDAndServiceID", connection))
                {
                    command.Parameters.AddWithValue("@ReservationID", ReservationID);
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;

                }

            }
            return false;
        }


        public static bool CheckReservationService(int ReservationID, int ServiceID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_CheckReservationService", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ReservationID", ReservationID);
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);

                    connection.Open();

                    object result = command.ExecuteScalar();

           
                    if (result == null || result == DBNull.Value)
                        return false;

                    int found = Convert.ToInt32(result);

                    return found == 1;
                }
            }
        }
    }
}
