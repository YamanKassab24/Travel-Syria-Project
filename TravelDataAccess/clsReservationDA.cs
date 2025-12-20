using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataAccess
{
    public enum enReservationStatus : byte
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2,
        Completed = 3
    }

    public class ReservationDTO
    {
        public int ReservationID { get; set; }
        public UserDTO User { get; set; }
        public TripDTO Trip { get; set; }
        public int SeatsCount { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public enReservationStatus Status { get; set; }


        public ReservationDTO(int ReservationID, int UserID, int TripID, int SeatsCount, DateTime ReservationDate,decimal TotalPrice, enReservationStatus Status)
        {
            this.ReservationID = ReservationID;
            this.User = clsUserDA.GetUserById(UserID);
            this.Trip = clsTripDA.GetTripByID(TripID);
            this.SeatsCount = SeatsCount;
            this.ReservationDate = ReservationDate;
            this.TotalPrice =TotalPrice;
            this.Status = Status;

        }

    }



    public class clsReservationDA
    {
        public static List<ReservationDTO> GetAllReservations()
        {
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reservations.Add(new ReservationDTO(

                                reader.GetInt32(reader.GetOrdinal("ReservationID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetInt32(reader.GetOrdinal("TripID")),
                                reader.GetInt32(reader.GetOrdinal("SeatsCount")),
                                reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                (enReservationStatus)reader.GetByte(reader.GetOrdinal("Status"))

                                ));


                        }
                    }

                }


            }

            return reservations;
        }

        public static ReservationDTO GetReservationByReservationID(int ReservationID)
        {
           
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetReservationByReservationID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReservationID", ReservationID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new ReservationDTO(

                                 reader.GetInt32(reader.GetOrdinal("ReservationID")),
                                 reader.GetInt32(reader.GetOrdinal("UserID")),
                                 reader.GetInt32(reader.GetOrdinal("TripID")),
                                 reader.GetInt32(reader.GetOrdinal("SeatsCount")),
                                 reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                reader.GetDecimal(reader.GetOrdinal("TotalPrice")),

                                 (enReservationStatus)reader.GetByte(reader.GetOrdinal("Status"))

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


        public static List<ReservationDTO> GetAllReservationsByUserID(int UserID)
        {
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllReservationByUserID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reservations.Add(new ReservationDTO(

                                reader.GetInt32(reader.GetOrdinal("ReservationID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetInt32(reader.GetOrdinal("TripID")),
                                reader.GetInt32(reader.GetOrdinal("SeatsCount")),
                                reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                reader.GetDecimal(reader.GetOrdinal("TotalPrice")),

                                (enReservationStatus)reader.GetByte(reader.GetOrdinal("Status"))

                                ));


                        }
                    }

                }


            }

            return reservations;
        }
        public static List<ReservationDTO> GetAllReservationsByTripID(int TripID)
        {
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllReservationByTripID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TripID", TripID);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            reservations.Add(new ReservationDTO(

                                reader.GetInt32(reader.GetOrdinal("ReservationID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetInt32(reader.GetOrdinal("TripID")),
                                reader.GetInt32(reader.GetOrdinal("SeatsCount")),
                                reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                reader.GetDecimal(reader.GetOrdinal("TotalPrice")),

                                (enReservationStatus)reader.GetByte(reader.GetOrdinal("Status"))

                                ));


                        }
                    }

                }


            }

            return reservations;
        }

        public static int AddNewReservation(ReservationDTO Reservation)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewReservation", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", Reservation.User.UserID);
                    command.Parameters.AddWithValue("@TripID", Reservation.Trip.TripID);
                    command.Parameters.AddWithValue("@SeatsCount", Reservation.SeatsCount);
                    command.Parameters.AddWithValue("@TotalPrice", Reservation.TotalPrice);
                    command.Parameters.AddWithValue("@ReservationDate", Reservation.ReservationDate);
                    command.Parameters.AddWithValue("@Status", (byte)Reservation.Status);

                    var outputIdParam = new SqlParameter("@ReservationID", SqlDbType.Int)
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


        public static bool DeleteReservationByReservationID(int ReservationID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteAllReservationByReservationID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReservationID", ReservationID);
                    connection.Open();
                    int RowsAffected = (int)command.ExecuteScalar();
                    return (RowsAffected > 0);

                }
            }


        }
        public static bool DeleteReservationByUserID(int UserID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteAllReservationByUserID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    int RowsAffected = (int)command.ExecuteScalar();
                    return (RowsAffected > 0);

                }
            }

        }

        public static bool UpdateReservation(int SeatsCount, int ReservationID,decimal TotalPrice)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_UpdateReservation", connection))
                    {

                        command.Parameters.AddWithValue("@NewSeatsCount", SeatsCount);
                        command.Parameters.AddWithValue("@ReservationID", ReservationID);
                        command.Parameters.AddWithValue("@TotalPrice", TotalPrice);

                        command.CommandType = CommandType.StoredProcedure;

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
    }

}



