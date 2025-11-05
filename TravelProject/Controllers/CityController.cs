using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBussinessLayer;
using TravelDataAccess;

namespace TravelProject.Controllers
{
    [Route("api/City")]
    [ApiController]
    public class CityController : ControllerBase
    {
        [HttpGet("GetCityByID",Name ="GetCityByID")]
          public ActionResult<CityDTO> GetCityID(int CityID)
          {
            if (CityID < 1)
            {
                return BadRequest("ID Not Accepted");
            }

            clsCityBL City = clsCityBL.GetCityByCityID(CityID);

            if (City == null)
            {
                return NotFound("Country Not Found");
            }

            CityDTO CityDTo = City.CityDTO;

            return Ok(CityDTo);





        }

        [HttpGet("GetCityByCityName", Name = "GetCityByCityName")]
        public ActionResult<CityDTO> GetCityByCityName(string CityName)
        {
            if (CityName=="")
            {
                return BadRequest("ID Not Accepted");
            }

            clsCityBL City = clsCityBL.GetCityByCityName(CityName);

            if (City == null)
            {
                return NotFound("Country Not Found");
            }

            CityDTO CityDTo = City.CityDTO;

            return Ok(CityDTo);





        }



    }
}
