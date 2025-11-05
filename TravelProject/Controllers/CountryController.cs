using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using TravelBussinessLayer;
using TravelDataAccess;

namespace TravelProject.Controllers
{
    [Route("api/Country")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        [HttpGet( "GetCountryByID",Name = "GetCountryByID")]
        public ActionResult<clsCountryDA.CountryDTO> GetCountryByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest("ID Not Accepted");
            }

            clsCountryBL Country = clsCountryBL.Find(ID);

            if (Country == null)
            {
                return NotFound("Country Not Found");
            }

            clsCountryDA.CountryDTO CountryDto = Country.CountryDTO;

            return Ok(CountryDto);

        }


        [HttpGet("GetCountryByName",Name = "GetCountryByName")]
        public ActionResult<clsCountryDA.CountryDTO> GetCountryByName(string CountryName)
        {

            if (CountryName == "")
            {
                return BadRequest("Should Have Value");
            }


            clsCountryBL Country = clsCountryBL.Find(CountryName);

            if (Country == null)
            {
                return NotFound("Not Found This Country");

            }

            clsCountryDA.CountryDTO CountryDto = Country.CountryDTO;

            return CountryDto;

        }



    }
}
