using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TravelDataAccess
{
    public class TripImageDTO
    {
        public int ImageID { get; set; }
        public int TripID { get; set; }

        
        public string FileName { get; set; }

       
        public string ImageUrl { get; set; }

        public TripImageDTO(int imageID, int tripID, string fileName)
        {
            ImageID = imageID;
            TripID = tripID;
            FileName = fileName;
        }
    }

    public class clsTripImageDA
    {
        public static bool AddNewTripImage(string fileName, int tripID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_AddNewTripImage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TripID", tripID);
                    // نخزن اسم الملف في العمود ImageUrl (أو غيّر اسم العمود لاحقاً لو حابب)
                    command.Parameters.AddWithValue("@ImageUrl", fileName);

                    connection.Open();

                    // بما أن الـ SP ممكن يكون فيه SET NOCOUNT ON،
                    // ExecuteNonQuery() ممكن يرجع -1 حتى لو الإضافة نجحت.
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public static List<TripImageDTO> GetAllTripImageByTripID(int tripID)
        {
            List<TripImageDTO> tripImages = new List<TripImageDTO>();

            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllTripImageByTripID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TripID", tripID);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int imageID = reader.GetInt32(reader.GetOrdinal("ImageID"));
                            int tripIdDb = reader.GetInt32(reader.GetOrdinal("TripID"));

                            // العمود في الداتابيس اسمو ImageUrl حالياً، لكن فيه فقط اسم الملف
                            string fileName = reader.GetString(reader.GetOrdinal("ImageUrl"));

                            tripImages.Add(new TripImageDTO(imageID, tripIdDb, fileName));
                        }
                    }
                }
            }

            return tripImages;
        }

        public static bool DeleteTripImageByImageID(int ImageID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteImageTripByImageID", connection))
                {

                   command.CommandType= CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ImageID",ImageID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;







                }





            }





        }

        public static bool DeleteTripImageByTripID(int TripID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_DeleteImageTripByTripID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TripID", TripID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;

                }


            }

        }


    }
}