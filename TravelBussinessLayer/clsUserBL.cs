using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;
using Service;
namespace TravelBussinessLayer
{
    public  class clsUserBL
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int UserID { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsActive { get; set; }
        public int PersonID { get; set; }
        public decimal WalletBalance { get; set; }


        clsUserBL(UserDTO UserDTO ,enMode NewMode=enMode.AddNew)
        {
        UserID = UserDTO.UserID;
        Role = UserDTO.Role;
        CreateAt = UserDTO.CreateAt;
        IsActive = UserDTO.IsActive;
        PersonID = UserDTO.PersonID;
        WalletBalance = UserDTO.WalletBalance;
         Mode = NewMode;
        
        }
        public clsUserBL()
        {
            UserID = -1;
            Role = "";
            CreateAt = DateTime.Now;
            IsActive = false;
            PersonID = -1;
            WalletBalance = 0000;
            Mode = enMode.AddNew;

        }


        public UserDTO UDTO
        {
           get
            {
                return new UserDTO(this.UserID, this.Role, this.CreateAt, this.IsActive, this.PersonID, this.WalletBalance);
            }
        }

        public static clsUserBL GetUserByID(int UserID)
        {
            UserDTO User=clsUserDA.GetUserById(UserID);
            
            if (User != null)
            {

                return new clsUserBL(User);


            }
            else
            {
                return null;
            }


        }
        public static string LoginWithJwt(string EmailOrPhone, string Password)
        {
            var user = clsUserDA.LoginWithEmailOrPhone(EmailOrPhone, Password);
            if (user == null) 
                return null;

            string secretKey = "ProcrastinateNowStudyLater12345678";
            string issuer = "Travel";
            string audience = "TravelApp";
            int expiryMinutes = 1000;

            return JwtService.GenerateToken(user, secretKey, issuer, audience, expiryMinutes);
        }

        private bool _AddNewUser(RequesAddNewUser RequestNewUser)
        {
            this.UserID = clsUserDA.AddNewUser(RequestNewUser.NewUser, RequestNewUser.NewUser.Person, RequestNewUser.Password);

            return (this.UserID != -1);


        }
        public RequesAddNewUser RequestNewUser { get; set; } = new RequesAddNewUser();

        public bool Save()
        {

            switch (Mode)
            {

                case enMode.AddNew:


                    if (_AddNewUser(this.RequestNewUser))
                    {
                        Mode = enMode.Update;
                        return true;

                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return false;


            }
            return false;
        }
    }


    public class RequesAddNewUser
    {
        
        public UserDTO NewUser { get; set; }
        public string Password { get; set; }
    }
}

