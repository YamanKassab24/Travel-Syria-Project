using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;
using TravelBussinessLayer;
using TravelDataAccess;

namespace TravelProject.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet ("GetUserByID",Name ="GetUserByID")]
        public ActionResult<UserDTO> GetUserByID(int UserID)
        {



            if (UserID < 1)
            {

                return BadRequest("This ID Not Accepted");

            }
           
            clsUserBL User=clsUserBL.GetUserByID(UserID);

            if (User == null)
            {
                return NotFound("Not Found User");
            }


            return User.UDTO;





        }


        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.emailOrPhone) || string.IsNullOrEmpty(model.password))
                return BadRequest("Email/Phone and Password are required.");

            var user = clsUserBL.LoginWithJwt(model.emailOrPhone, GlobalClass.ComputeHash( model.password));
            if (user == null)
                return null;
            var jwtSettings = _configuration.GetSection("JwtSettings");
            string secretKey = jwtSettings["SecretKey"];
            string issuer = jwtSettings["Issuer"];
            string audience = jwtSettings["Audience"];
            int expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);

            string token = JwtService.GenerateToken(user, secretKey, issuer, audience, expiryMinutes);

            return Ok(new
            {
                User = user,
                Token = token
            });
        }

        [AllowAnonymous]
        [HttpPost("AddNewUser", Name = "AddNewUser")]

        public ActionResult<UserDTO> AddNewUser([FromBody] AddNewUserRequest Model)
        {
            //we validate the data here
            if (Model == null || string.IsNullOrEmpty(Model.password))
            {
                return BadRequest("Invalid student data.");
            }

            //newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;
            clsUserBL User = new clsUserBL();

            User.AddNewUserRequest.firstName = Model.firstName;
            User.AddNewUserRequest.lastName = Model.lastName;
            User.AddNewUserRequest.phone = Model.phone;
            User.AddNewUserRequest.email = Model.email;
            User.AddNewUserRequest.dateOfBirth = DateTime.Now;
            User.AddNewUserRequest.image = " ";
            User.AddNewUserRequest.countryID = 1;
            User.AddNewUserRequest.Role = Model.Role;
            User.AddNewUserRequest.createAt = DateTime.Now;
          
            User.AddNewUserRequest.password =GlobalClass.ComputeHash(  Model.password);
           User.Save();
            Model.UserID=User.UserID;
            Model.password = GlobalClass.ComputeHash(Model.password);
            return CreatedAtRoute("AddNewUser", new { UserID = Model.UserID }, Model);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers",Name = "GetAllUsers")]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
        {
            List<UserDTO> Users = clsUserBL.GetAllUsers();

            if (Users == null || Users.Count == 0)
            {
                return NotFound("No Found Users");

            }

            return Ok(Users);
        }

        [Authorize]

        [HttpPut("UpdateUser",Name ="UpdateUser")]
        public ActionResult<UserDTO>UpdateUser(UpdateRequest updateRequest)
        {
            if (updateRequest == null||updateRequest.UserID<=0 )
            {
                return BadRequest("Not Valid User");
            }

        
            clsUserBL User = clsUserBL.GetUserByID(updateRequest.UserID);
            if (User == null)
            {
                return NotFound("Not Found User");
            }
            clsPersonBl Person = clsPersonBl.GetPersonByID(User.PersonID);
            if (Person == null)
            {
                return NotFound("Not Found Person");
            }
            Person.FirstName = updateRequest.FirstName;
            Person.LastName = updateRequest.LastName;
            Person.Email = updateRequest.Email;
            Person.Phone = updateRequest.Phone;
            Person.DateOfBirth = updateRequest.DateOfBirth;
            Person.CountryID = updateRequest.CountryID;
            Person.Image = updateRequest.Image;
            Person .IsMale = updateRequest.IsMale;
            Person.Mode = clsPersonBl.enMode.Update;
            Person.Save();
            return Ok(User.UDTO);
        }

        [Authorize]
        [HttpDelete("DeleteUserByUserID", Name = "DeleteUserByUserID")]
        public ActionResult DeleteUserbyUserID(int UserID)
        {

            if(UserID <= 0)
            {
                return BadRequest("Bad Request Chose Another ID");
            }
            if (clsUserBL.DeleteUserByUserID(UserID))
            {
                return Ok($"Delete User With ID : {UserID} Successfuly");
            }
            else
            {
                return NotFound($"Not Found User with ID : {UserID}");
            }

        }

        [Authorize]
        [HttpPut("UpdatePassword", Name = "UpdatePassword")]
        public ActionResult UpdatePassword(int UserID,string CurrentPassword, string Newpassword)
        {
            bool IsUpdate=false;
            if (UserID <= 0 ||string.IsNullOrEmpty(Newpassword))
            {
                return BadRequest("BadRequest");
            }


            if (clsUserBL.IsCorrectCurrentPassword(UserID, GlobalClass.ComputeHash( CurrentPassword)))
            {
                 IsUpdate = clsUserBL.UpdatePassword(UserID, GlobalClass.ComputeHash(CurrentPassword), GlobalClass.ComputeHash(Newpassword));

            }
            else
            {
                return NotFound ("Current password Wrong");
            }

            if (IsUpdate) { return Ok("Updated Successfuly"); }
            else
            {
                return NotFound("Not Found User");
            }


        }


        [Authorize]
        [HttpGet("IsCorrectCurrentPassword", Name = "IsCorrectCurrentPassword")]
        public ActionResult<bool> IsCorrectCurrentPassword(int UserID, string password)
        {
            if (UserID <= 0 || string.IsNullOrEmpty(password))
            {
                return BadRequest("BadRequest");
            }

            bool IsUpdate = clsUserBL.IsCorrectCurrentPassword(UserID, GlobalClass.ComputeHash(password));


            if (IsUpdate) { return Ok(true); }
            else
            {
                return NotFound(false);
            }


        }



    }

    public class LoginRequest
    {
        public string emailOrPhone { get; set; }
        public string password { get; set; }
    }
    public class AddNewUserRequest
    {



      public  int UserID { set; get;  }
        public string firstName { set; get; }
        public string lastName { set; get; }
        public string phone { set; get; }
        public string email { set; get; }
        public bool isMale { set; get;  }
        public DateTime dateOfBirth { set; get; }
        public string image { set; get; }
        public int countryID { set;  get;  }
        public string createAt { set; get;  }
        public    string Role {  get; set; }
        public string password { set; get; }
    }

    public class  UpdateRequest
    {
    public     int UserID { set; get; }

public string   FirstName     {set;get;}
public  string   LastName     {set;get;}
public   string  Phone        {set;get;}
public  string  Email         {set;get;}
public    bool IsMale         {set;get;}
public DateTime   DateOfBirth {set;get;}
public    string Image        {set;get;}
public int CountryID         { set; get; }


    }







}

