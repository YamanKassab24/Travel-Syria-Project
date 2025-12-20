using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Security.Cryptography;

namespace TravelDataAccess
{
    public class GlobalClass
    {
        public   static string _connectionString = "Server=sql6033.site4now.net;Database=db_ac183c_traveldb;User Id=db_ac183c_traveldb_admin;Password=sa123456;MultipleActiveResultSets=True;TrustServerCertificate=True;";
        //public static string _connectionString = "Server=.;Database=TravelDB;User Id=sa;Password=sa123456;MultipleActiveResultSets=True;TrustServerCertificate=True;";




        public static string ComputeHash(string input)
        {
            
            using (SHA256 sha256 = SHA256.Create())
            {
                
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));


                
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
