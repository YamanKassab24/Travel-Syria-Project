using Microsoft.AspNetCore.Http;
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

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.EmailOrPhone) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Email/Phone and Password are required.");

            var user = clsUserDA.LoginWithEmailOrPhone(model.EmailOrPhone, model.Password);

            if (user == null)
                return Unauthorized("Invalid credentials!");

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


        [HttpPost("AddNewUser",Name ="AddNewUser")]

        public ActionResult<RequesAddNewUser> AddNewUser(RequesAddNewUser NewUser)
        {
            //we validate the data here
            if (NewUser == null || string.IsNullOrEmpty(NewUser.Password))
            {
                return BadRequest("Invalid student data.");
            }

            //newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;
            clsUserBL User = new clsUserBL();
            
            User.RequestNewUser.NewUser = NewUser.NewUser;
          
            User.RequestNewUser.Password = NewUser.Password;
           User.Save();
            NewUser.NewUser.UserID=User.UserID;
            //we return the DTO only not the full student object
            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("AddNewUser", new { UserID = NewUser.NewUser.UserID }, NewUser.NewUser);

        }



    }

    public class LoginRequest
    {
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
    }









}

