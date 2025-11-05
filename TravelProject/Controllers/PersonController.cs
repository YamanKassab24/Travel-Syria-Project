using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelDataAccess;
using TravelBussinessLayer;


namespace TravelProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet("GetPersonByID", Name = "GetPersonByID")]
        public ActionResult<PersonDTO> GetPersonByID(int PersonID)
        {
            if (PersonID < 1)
            {

                return BadRequest("This ID Not Accepted");

            }
            clsPersonBl Person = clsPersonBl.GetPersonByID(PersonID);


            if (Person == null)
            {
                return NotFound("Not Found Person");
            }


            return Person.PDTO;
        }



    [HttpPost(Name = "AddNewPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PersonDTO> AddNewPerson(PersonDTO PersonDTO)
        {
            //we validate the data here
            if (PersonDTO == null || string.IsNullOrEmpty(PersonDTO.FirstName))
            {
                return BadRequest("Invalid student data.");
            }


            clsPersonBl Person = new clsPersonBl(new PersonDTO(PersonDTO.PersonID, PersonDTO.FirstName, PersonDTO.LastName, PersonDTO.Email, PersonDTO.Phone, PersonDTO.IsMale, PersonDTO.DateOfBirth, PersonDTO.Image, PersonDTO.CountryID));
            Person.Save();

            PersonDTO.PersonID = Person.PersonID;

            
            return CreatedAtRoute("AddNewPerson", new { PersonID = PersonDTO.PersonID }, PersonDTO);

        }
    }
    }
