using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataAccess
{
    public enum enPaymentStatus : byte
    {
        Pending = 0,     // بانتظار الدفع
        Completed = 1,   // تم الدفع بنجاح
        Failed = 2,      // فشل الدفع
        Refunded = 3,// تم استرجاع المبلغ
        Cancelled = 4
    }
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public ReservationDTO Reservation { get; set; }

        public string Method { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentDetails { get; set; }
        public DateTime? TransactionDate { get; set; }
        public enPaymentStatus? Status { get; set; }

        public PaymentDTO(int PaymentID, int ReservationID, string Method, decimal Amount, string PaymentDetails, DateTime? TransactionDate, enPaymentStatus? status)
        {
            this.PaymentID = PaymentID;
            this.Reservation = clsReservationDA.GetReservationByReservationID(ReservationID);
            this.Method = Method;
            this.Amount = Amount;
            this.PaymentDetails = PaymentDetails;
            this.TransactionDate = TransactionDate;
            this.Status = status;
        }


    }


    public class clsPaymentDA
    {
        public static  List<PaymentDTO> GetAllPaymentsByReservationID(int ReservationID)
        {
            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllPaymentsByReservationID", connection))
                {

                    command.Parameters.AddWithValue("@ReservationID", ReservationID);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            paymentDTOs.Add(new PaymentDTO(

                                reader.GetInt32(reader.GetOrdinal("PaymentID")),

                                reader.GetInt32(reader.GetOrdinal("ReservationID")),

                                reader.GetString(reader.GetOrdinal("Method")),
                                reader.GetDecimal(reader.GetOrdinal("Amount")),

                               reader.IsDBNull(reader.GetOrdinal("PaymentDetails")) ? null : reader.GetString(reader.GetOrdinal("PaymentDetails")),

                              reader.IsDBNull(reader.GetOrdinal("TransactionDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                             reader.IsDBNull(reader.GetOrdinal("status")) ? (enPaymentStatus?)null : (enPaymentStatus?)reader.GetByte(reader.GetOrdinal("status"))

                                ));

                        }



                    }

                }
            }
            return paymentDTOs;
        }

        public static PaymentDTO GetPaymentsByPaymentID(int PaymentID)
        {


            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetPaymentsByPaymentID", connection))
                {

                    command.Parameters.AddWithValue("@PaymentID", PaymentID);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new PaymentDTO(

                                    reader.GetInt32(reader.GetOrdinal("PaymentID")),

                                    reader.GetInt32(reader.GetOrdinal("ReservationID")),

                                    reader.GetString(reader.GetOrdinal("Method")),
                                    reader.GetDecimal(reader.GetOrdinal("Amount")),

                                   reader.IsDBNull(reader.GetOrdinal("PaymentDetails")) ? null : reader.GetString(reader.GetOrdinal("PaymentDetails")),

                                  reader.IsDBNull(reader.GetOrdinal("TransactionDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                                 reader.IsDBNull(reader.GetOrdinal("status")) ? (enPaymentStatus?)null : (enPaymentStatus?)reader.GetByte(reader.GetOrdinal("status"))

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

        public static int AddNewPayment(PaymentDTO payment)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewPayment", connection))
                {

                    command.Parameters.AddWithValue("@ReservationID", payment.Reservation.ReservationID);
                    command.Parameters.AddWithValue("@Method", payment.Method);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@PaymentDetails", payment.PaymentDetails);
                    command.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", payment.Status);

                    command.CommandType = CommandType.StoredProcedure;
                    var outputIdParam = new SqlParameter("@PaymentID", SqlDbType.Int)
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

        public static bool UpdatePayment(int PaymentID, decimal Amount, string? PaymentDetails, enPaymentStatus? Status)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_UpdatePayment", connection))
                {
                    command.Parameters.AddWithValue("@PaymentID", PaymentID);
                    command.Parameters.AddWithValue("@Amount", Amount);
                    command.Parameters.AddWithValue("@PaymentDetails", PaymentDetails);
                    command.Parameters.AddWithValue("@Status", Status);

                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;

                }
            }




        }

        public static bool UpdatePaymentStatusByPaymentID(int PaymentID, enPaymentStatus? Status)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_UpdatePaymentStatusByPaymentID", connection))
                {
                    command.Parameters.AddWithValue("@PaymentID", PaymentID);

                    command.Parameters.AddWithValue("@Status", Status);

                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;

                }
            }
        }

        public static bool UpdatePaymentStatusByReservationID(int ReservationID, enPaymentStatus? Status)

        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_UpdatePaymentStatusByReservationID", connection))
                {
                    command.Parameters.AddWithValue("@ReservationID", ReservationID);

                    command.Parameters.AddWithValue("@Status", Status);

                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;
                }
            }
        }
       

     }
  }
        
