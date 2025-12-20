using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
 

namespace TravelDataAccess
{

   public enum enTripStatus : byte { Created = 0, InProgress = 1, Completed = 2, Cancelled = 3 }

    public class TripDTO
    {


        public int TripID { get; set; }
        public CompanyDTO  Company { get; set; }

        public CityDTO DepartureCity { get; set; }
        public CityDTO ArrivalCity { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public enTripStatus? Status { get; set; }

        public string? Details { get; set; }

        public byte? Rating { get; set; }

        public int SeatsAvailable { set; get; }

       public int MaxSeats { set; get; }
        public TripDTO(int TripID,int CompanyID,int DepartureCityID,int ArrivalCityID ,DateTime DepartureTime,DateTime ArrivalTime ,decimal Price ,DateTime CreatedAt,enTripStatus? status,string Details,byte? Rating,int SeatsAvailable,int MaxSeats) 
        { 
        this.TripID = TripID;
         this.Company = clsCompanyDA.GetCompanyByID(CompanyID);
            this.DepartureCity = clsCityDA.GetCityByID(DepartureCityID);
            this.ArrivalCity=clsCityDA.GetCityByID(ArrivalCityID);
            this.DepartureTime = DepartureTime;
            this.ArrivalTime = ArrivalTime;
            this.Price = Price;
            this.CreatedAt = CreatedAt;
            this.Status = status;
            this.Details = Details;
            this.Rating = Rating;
            this.SeatsAvailable = SeatsAvailable;
            this.MaxSeats = MaxSeats;
        }
        public TripDTO()
        {

            this.TripID = -1;
            this.Company = null;
            this.DepartureCity = null;
            this.ArrivalCity = null;
            this.DepartureTime = DateTime.Now;
            this.ArrivalTime = DateTime.Now;
            this.Price = 0;
            this.CreatedAt = DateTime.Now;
            this.Status = enTripStatus.Cancelled;
            this.Details = " ";
            this.Rating = 0;
            this.MaxSeats = -1;
            this.SeatsAvailable = -1;


        }


    }


    public class clsTripDA
    {
        public static List<TripDTO> GetAllTrips()
        {
            List<TripDTO> trips = new List<TripDTO>();
            using (SqlConnection connection=new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllTrips", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();


                    using (var reader = command.ExecuteReader())

                    {

                        while (reader.Read())
                        {
                            trips.Add(
                                new TripDTO(
                                    reader.GetInt32(reader.GetOrdinal("TripID")),
                                    reader.GetInt32(reader.GetOrdinal("CompanyID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartureCityID")),
                                    reader.GetInt32(reader.GetOrdinal("ArrivalCityID")),
                                    reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                                    reader.GetDateTime(reader.GetOrdinal("ArrivalTime")),
                                    reader.GetDecimal(reader.GetOrdinal("Price")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.IsDBNull(reader.GetOrdinal("Status"))
                                        ? null
                                        : (enTripStatus?)reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.IsDBNull(reader.GetOrdinal("Details"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Details")),
                                    reader.IsDBNull(reader.GetOrdinal("Rating"))
                                        ? (byte?)null
                                        : reader.GetByte(reader.GetOrdinal("Rating")),
                                    reader.GetInt32(reader.GetOrdinal("SeatsAvailable")),
                                    reader.GetInt32(reader.GetOrdinal("MaxSeats"))

                                )
                            );
                        }
                    }




                }


            }

            return trips;
        }

        public static TripDTO GetTripByID(int TripID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetTripByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("TripID",TripID);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new TripDTO(
                                     reader.GetInt32(reader.GetOrdinal("TripID")),
                                     reader.GetInt32(reader.GetOrdinal("CompanyID")),
                                     reader.GetInt32(reader.GetOrdinal("DepartureCityID")),
                                     reader.GetInt32(reader.GetOrdinal("ArrivalCityID")),
                                     reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                                     reader.GetDateTime(reader.GetOrdinal("ArrivalTime")),
                                     reader.GetDecimal(reader.GetOrdinal("Price")),
                                     reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                     reader.IsDBNull(reader.GetOrdinal("Status"))
                                         ? null
                                         : (enTripStatus?)reader.GetByte(reader.GetOrdinal("Status")),
                                     reader.IsDBNull(reader.GetOrdinal("Details"))
                                         ? null
                                         : reader.GetString(reader.GetOrdinal("Details")),
                                     reader.IsDBNull(reader.GetOrdinal("Rating"))
                                         ? (byte?)null
                                         : reader.GetByte(reader.GetOrdinal("Rating")),
                                          reader.GetInt32(reader.GetOrdinal("SeatsAvailable")),
                                    reader.GetInt32(reader.GetOrdinal("MaxSeats"))
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

        public static int AddNewTrip(TripDTO trip)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewTrip", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CompanyID", trip.Company.CompanyID);
                    command.Parameters.AddWithValue("@DepartureCityID", trip.DepartureCity.CityID);
                    command.Parameters.AddWithValue("@ArrivalCityID", trip.ArrivalCity.CityID);
                    command.Parameters.AddWithValue("@DepartureTime", trip.DepartureTime);
                    command.Parameters.AddWithValue("@ArrivalTime", trip.ArrivalTime);
                    command.Parameters.AddWithValue("@Price", trip.Price);
                    command.Parameters.AddWithValue("@Status", (byte)trip.Status);

             
                    command.Parameters.AddWithValue("@Details",
                        (object?)trip.Details ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Rating",
                        (object?)trip.Rating ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MaxSeats",trip.MaxSeats);
                    
                    var outputIdParam = new SqlParameter("@TripID", SqlDbType.Int)
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

        public static bool UpdateTrip(TripDTO trip)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_UpdateTrip", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    
                    command.Parameters.AddWithValue("@TripID", trip.TripID);

                    command.Parameters.AddWithValue("@CompanyID", trip.Company.CompanyID );

                    command.Parameters.AddWithValue("@DepartureCityID",trip.DepartureCity.CityID);

                    command.Parameters.AddWithValue("@ArrivalCityID",trip.ArrivalCity.CityID);

                    command.Parameters.AddWithValue("@DepartureTime",
                        trip.DepartureTime);

                    command.Parameters.AddWithValue("@ArrivalTime", trip.ArrivalTime);

                    command.Parameters.AddWithValue("@Price", trip.Price);

                    command.Parameters.AddWithValue("@Status", trip.Status);

                    command.Parameters.AddWithValue("@Details", trip.Details);

                    command.Parameters.AddWithValue("@Rating",trip.Rating);

                    command.Parameters.AddWithValue("@MaxSeats", trip.MaxSeats);

                    connection.Open();
                     command.ExecuteNonQuery();

                    return true;
                }
            }
        }

        public static bool DeleteTrip(int TripID)
        {

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_UpdateTripStatus", connection))
                {
                    command.Parameters.AddWithValue("@StatusTrip", 3);
                    command.Parameters.AddWithValue("@TripID", TripID);

                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;

                }
            }



        }
    }
}
