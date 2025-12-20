using Microsoft.AspNetCore.Mvc;
using TravelBussinessLayer;
using TravelDataAccess;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace TravelProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactMessageController : ControllerBase
    {
        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllContactMessages", Name = "GetAllContactMessages")]
        public ActionResult<ContactMessageDTO> GetAllContactMessages()
        {
            var list = clsContactMessageBL.GetAllContactMessages();
            if(list != null)
            return Ok(list);
            else
                return NotFound();
        }
        [AllowAnonymous]
        [HttpPost("AddNewContactMessage", Name = "AddNewContactMessage")]
        public ActionResult<ContactMessageDTO> AddNewContactMessage([FromBody] AddNewContactMessageRequest model)
        {
            if (model == null)
                return BadRequest("Model is null.");

            
            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest("Name is required.");

            if (string.IsNullOrWhiteSpace(model.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(model.Message))
                return BadRequest("Message is required.");

    
            clsContactMessageBL cm = new clsContactMessageBL();
            cm.Name = model.Name.Trim();
            cm.Email = model.Email.Trim();
            cm.phone = model.Phone?.Trim() ?? "";
            cm.Message = model.Message.Trim();
            cm.CreatedAt = System.DateTime.Now;
            cm.Mode = clsContactMessageBL.enMode.AddNew;

            if (cm.Save())
            {
               
                return Ok(cm.CMDTO);
            }

            return StatusCode(500, "Failed to add contact message.");
        }
    }

    // Request Model
    public class AddNewContactMessageRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Message { get; set; }
    }
}
