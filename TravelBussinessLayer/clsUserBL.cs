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

        clsPersonBl PersonBl=new clsPersonBl();
        clsUserBL(UserDTO UserDTO ,enMode NewMode=enMode.AddNew)
        {
        UserID = UserDTO.UserID;
        Role = UserDTO.Role;
        CreateAt = UserDTO.CreateAt;
        IsActive = UserDTO.IsActive;
        PersonID = UserDTO.PersonID;
         PersonBl = clsPersonBl.GetPersonByID(UserDTO.PersonID);
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

                return new clsUserBL(User,enMode.Update);


            }
            else
            {
                return null;
            }


        }
        public static UserDTO LoginWithJwt(string EmailOrPhone, string Password)
        {
            var user = clsUserDA.LoginWithEmailOrPhone(EmailOrPhone, Password);
            if (user == null)
                return null;

            
            return user;
        }

        private bool _AddNewUser(AddNewUserRequest RequestNewUser)
        {
            UserDTO userDTO = new UserDTO();
            userDTO .UserID = RequestNewUser.UserID;
            userDTO .Role = RequestNewUser.Role;    
            userDTO.Person.FirstName=RequestNewUser.firstName;
            userDTO.Person.LastName=RequestNewUser.lastName;
            userDTO.Person.Email=RequestNewUser.email;
            userDTO.Person.Phone=RequestNewUser.phone;
            userDTO .CreateAt=RequestNewUser .createAt;
            userDTO.Person.DateOfBirth=RequestNewUser .dateOfBirth;
            userDTO.Person.Image=RequestNewUser.image;
            userDTO.Person.CountryID=RequestNewUser.countryID;
            userDTO.Person.IsMale = RequestNewUser.isMale;
            this.UserID = clsUserDA.AddNewUser(userDTO, RequestNewUser.password);

            return (this.UserID != -1);


        }
  
       
     static  public  List<UserDTO>GetAllUsers()
        {
            return clsUserDA.GetAllUsers();

        }
        public AddNewUserRequest AddNewUserRequest { get; set; } = new AddNewUserRequest();
        public bool Save()
        {

            switch (Mode)
            {

                case enMode.AddNew:


                    if (_AddNewUser(this.AddNewUserRequest))
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

        public static bool DeleteUserByUserID(int UserID)
        {
           return clsUserDA.DeleteUserByUserID(UserID); 

        }


        public static bool UpdatePassword(int UserID,string CurrentPassword, string Password)
        {
            return clsUserDA.UpdatePassword(UserID,CurrentPassword, Password);
        }
        public static bool IsCorrectCurrentPassword(int UserID ,string Password)
        {
            return clsUserDA.IsCorrectCurrentPassword(UserID, Password);
        }
    }

    public class AddNewUserRequest
    {



        public int UserID { set; get; }
        public string firstName { set; get; }
        public string lastName { set; get; }
        public string phone { set; get; }
        public string email { set; get; }
        public bool isMale { set; get;  }
        public DateTime dateOfBirth { set; get; }
        public string image { set; get; }
        public int countryID { set; get; }
        public DateTime createAt { set; get; }
        public string Role { get; set; }
        public string password { set; get; }
    }
}

