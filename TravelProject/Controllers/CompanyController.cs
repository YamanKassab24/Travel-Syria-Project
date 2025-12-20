using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBussinessLayer;
using TravelDataAccess;
namespace TravelProject.Controllers
{
    [Route("api/Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllCompanies", Name = "GetAllCompanies")]
        public ActionResult<CompanyDTO> GetAllCompanies()
        {
            List<CompanyDTO> Companies = clsCompanyBL.GetAllCompanies();
            if (Companies == null)
            {
                return NotFound("Not Found Companies");
            }

            return Ok(Companies);

        }

        [Authorize]
        [HttpGet("GetCompanyID", Name = "GetCompanyByID")]
       public ActionResult<CompanyDTO> GetCompanyByID(int CompanyID)
        {

            if (CompanyID <= 0)
            {
                return BadRequest("Bad request Chose another ID");
            }
            clsCompanyBL Company = clsCompanyBL.GetCompanyByID(CompanyID);
            if (Company != null)
            {
                return Ok(Company.CDTO);

            }
            else
            {
                return NotFound("Not Found Company With This ID Chose another");
            }



        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewCompany",Name ="AddNewCompany")]
        public ActionResult<CompanyDTO> AddNewCompany([FromBody] RequestAddNewCompany Model)
        {
            if (Model == null)
                return BadRequest("Invalid company data");

            clsCompanyBL company = new clsCompanyBL();
            company.CompanyName=Model.CompanyName;
            company.Address=Model.Address;
            company.Phone=Model.Phone;
            company.IsActive=Model.IsActive;
            company.Mode = clsCompanyBL.enMode.AddNew;

            if (company.Save())
            {
                CompanyDTO companyDTO = clsCompanyDA.GetCompanyByID(company.CompanyID);
                return Ok(companyDTO);

            }


            return StatusCode(500, "Failed to add company");
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateCompany",Name ="UpdateCompany")]
        public ActionResult UpdateCompany(RequestUpdateCompany Model )
        {
           clsCompanyBL companyBL = new clsCompanyBL();

           companyBL.CompanyID=Model.CompanyID;
            companyBL.CompanyName=Model.CompanyName;
            companyBL.Address=Model.Address;
            companyBL.Phone=Model.Phone;
            companyBL.IsActive=Model.IsActive;


            companyBL.Mode = clsCompanyBL.enMode.Update;

            if (companyBL.Save())
                return Ok("updated Successfuly");

            return StatusCode(500, "Update failed");
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteCompany", Name = "DeleteCompany")]
        public ActionResult DeleteCompany(int companyID)
        {
            bool result = clsCompanyBL.DeleteCompany(companyID);

            if (!result)
                return NotFound("Company not found or delete failed");

            return Ok(new { message = "Company deleted successfully" });
        }
    }

    public class RequestAddNewCompany
    {


        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        
        public bool IsActive { get; set; }
    }

    public class RequestUpdateCompany : RequestAddNewCompany
    {
        public int CompanyID { get; set; }
    }

}
    
