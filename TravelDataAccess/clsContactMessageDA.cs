using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataAccess
{
    public class ContactMessageDTO
    {
       public int  ContactMessageID      {set; get; }
        public string     Name           {set; get; }
         public string Email             {set; get; }
         public string phone             {set; get; }
          public string Message          {set; get; }
         public DateTime CreatedAt { set; get; }

        public ContactMessageDTO(int ContactMessageID,string Name ,string Email ,string Phone ,string Message ,DateTime CreatedAt ) 
        {
        this .ContactMessageID = ContactMessageID;
            this .Name = Name;
            this .Email = Email;
             this .phone = Phone;
            this .Message = Message;
            this .CreatedAt = CreatedAt;

        }



    }

    public class clsContactMessageDA
    {
        public static List<ContactMessageDTO> GetAllContactMessages()
        {
            List<ContactMessageDTO> contactMessages = new List<ContactMessageDTO>();
            using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetAllContactMessages", connection))
                {
                    command.CommandType=CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            contactMessages.Add(
                                new ContactMessageDTO
                                (
                                   reader.GetInt32(reader.GetOrdinal("ContactMessageID")),
                                   reader.GetString(reader.GetOrdinal("Name")),
                                   reader.GetString(reader.GetOrdinal("Email")),
                                   reader.GetString(reader.GetOrdinal("Phone")),
                                   reader.GetString(reader.GetOrdinal("Message")),
                                   reader.GetDateTime(reader .GetOrdinal("CreatedAt"))

                                 )
                                );

                        }
                    }

                }

            }

            return contactMessages;
        }

        public static int  AddNewContactMessage (ContactMessageDTO Message)
        {
            int ContactMessageID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalClass._connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_AddNewContactMessage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Name", Message.Name);
                        command.Parameters.AddWithValue("@Email", Message.Email);
                        command.Parameters.AddWithValue("@Phone", Message.phone);
                        command.Parameters.AddWithValue("@Message", Message.Message);

                        SqlParameter outputIdParam = new SqlParameter(
                            "@ContactMessageID",
                            SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        ContactMessageID = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return ContactMessageID;


        }
            
        

    }
}
