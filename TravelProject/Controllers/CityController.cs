using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpGet("GetCityByCountryID", Name = "GetCityByCountryID")]
        public ActionResult<CityDTO> GetCityCountryID(int CountryID)
        {
            if (CountryID < 1)
            {
                return BadRequest("ID Not Accepted");
            }

              List<CityDTO> cities= clsCityBL.GetCityByCountryID(CountryID);

            if (cities.Count > 0)
            {
                return Ok(cities);
            }
            else { return NotFound($"Not Found Cities With CountrID {CountryID}"); }
           

            





        }

        [AllowAnonymous]
        [HttpGet("GetCityByID", Name = "GetCityByID")]
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

        [AllowAnonymous]
        [HttpGet("GetCityByCityName", Name = "GetCityByCityName")]
        public ActionResult<CityDTO> GetCityByCityName(string CityName)
        {
            if (CityName == "")
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

        [Authorize(Roles = "Admin")]

        [HttpPost("AddNewCity", Name = "AddNewCity")]
        public ActionResult<CityDTO> AddNewCity(RequestAddNewCity Model)
        {
            if (Model == null)
            {
                return BadRequest("BadRequest");
            }

            clsCityBL cityBL = new clsCityBL();
            cityBL.CityName = Model.CityName;
            cityBL.CountryID = Model.CountryID;
            if (cityBL.Save())
            {
                CityDTO city = clsCityDA.GetCityByID(cityBL.CityID);
                if (city == null)
                {
                    return Ok("Cant Find NewCity");
                }
                else
                {
                    return Ok(city);
                }

            }
            else
            {
                return NotFound ("Field Add City");
            }

        }


        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateCity", Name = "UpdateCity")]
        public ActionResult<CityDTO> UpdateCity(RequestUpdateCity Model)
        {
            if (Model == null)
            {
                return BadRequest("BadRequest");
            }

            clsCityBL cityBL = new clsCityBL();
            cityBL.CityName = Model.CityName;
            cityBL.CountryID = Model.CountryID;
            cityBL.CityID = Model.CityID;
            cityBL.Mode = clsCityBL.enMode.Update;
            if (cityBL.Save())
            {
                CityDTO city = clsCityDA.GetCityByID(cityBL.CityID);
                if (city == null)
                {
                    return Ok("Cant Find City");
                }
                else
                {
                    return Ok(city);
                }

            }
            else
            {
                return NotFound("Field update City");
            }

        }



        public class RequestAddNewCity
        {
            public string CityName { get; set; }
            public int CountryID { get; set; }
        }
        public class RequestUpdateCity : RequestAddNewCity
        {
            public int CityID { get; set; }

        }
    }
}
