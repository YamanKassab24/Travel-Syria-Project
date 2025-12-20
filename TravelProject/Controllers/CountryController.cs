using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpGet( "GetCountryByID",Name = "GetCountryByID")]
        public ActionResult<CountryDTO> GetCountryByID(int ID)
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

            CountryDTO CountryDto = Country.CountryDTO;

            return Ok(CountryDto);

        }

        [AllowAnonymous]
        [HttpGet("GetCountryByName",Name = "GetCountryByName")]
        public ActionResult<CountryDTO> GetCountryByName(string CountryName)
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

            CountryDTO CountryDto = Country.CountryDTO;

            return CountryDto;

        }
        [AllowAnonymous]
        [HttpGet("GetAllCountries", Name = "GetAllCountries")]
        public ActionResult<IEnumerable<CountryDTO>> GetAllCountries()
        {

            List<CountryDTO> Countries = new List<CountryDTO>();
            Countries = clsCountryBL.GetAllCountries();
            if (Countries.Count == 0)
            {
                return NotFound("No Countries Found");
            }

            return Ok(Countries);
        }
    }
}
