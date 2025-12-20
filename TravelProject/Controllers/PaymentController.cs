using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using TravelBussinessLayer;
using TravelDataAccess;     

namespace TravelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        #region DTOs for requests

        public class CreatePaymentRequest
        {
            public int ReservationID { get; set; }
            public string Method { get; set; }

            public decimal Amount { get; set; }
            public string PaymentDetails { get; set; }

            public enPaymentStatus? Status { get; set; }
        }

        public class UpdateStatusByPaymentIDRequest
        {
            public int PaymentID { get; set; }
            public enPaymentStatus? Status { get; set; }
        }

        public class UpdateStatusByReservationIDRequest
        {
            public int ReservationID { get; set; }
            public enPaymentStatus? Status { get; set; }
        }
        public class UpdatePaymentByPaymentIDRequest
        {
            public int PaymentID { get; set; }

            public decimal Amount { get; set; }

            public string PaymentDetails { get; set; }

            public enPaymentStatus? Status { get; set; }
        }

        [Authorize]
        [HttpGet("GetAllPaymentsByReservationID", Name = "GetAllPaymentsByReservationID")]
        public ActionResult<List<PaymentDTO>> GetAllPaymentsByReservationID(int reservationID)
        {
            try
            {

                List<PaymentDTO> payments = clsPaymentBL.GetAllPaymentsByReservationID(reservationID);

                if (payments == null || payments.Count == 0)
                    return NotFound($"No payments found for ReservationID = {reservationID}.");

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while getting payments: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("GetPaymentByPaymentID", Name = "GetPaymentByPaymentID")]
        public ActionResult<PaymentDTO> GetPaymentByPaymentID(int paymentID)
        {
            try
            {

                PaymentDTO payment = clsPaymentBL.GetPaymentsByPaymentID(paymentID);

                if (payment == null)
                    return NotFound($"Payment with ID = {paymentID} was not found.");

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while getting payment: {ex.Message}");
            }
        }



        [Authorize]
        [HttpPost("AddNewPayment", Name = "AddNewPayment")]
        public ActionResult<PaymentDTO> AddNewPayment([FromBody] CreatePaymentRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var paymentBL = new clsPaymentBL();

                paymentBL.ReservationID = model.ReservationID;
                paymentBL.Method = model.Method;
                paymentBL.Amount = model.Amount;
                paymentBL.PaymentDetails = model.PaymentDetails;
                paymentBL.Status = model.Status;

                bool isSaved = paymentBL.Save();

                if (!isSaved)
                    return BadRequest("Failed to add payment.");


                var helper = new clsPaymentBL();
                var dto = clsPaymentBL.GetPaymentsByPaymentID(paymentBL.PaymentID);

                if (dto == null)
                    return Ok(new
                    {
                        paymentBL.PaymentID,
                        message = "Payment added, but could not load PaymentDTO. Check mapping."
                    });

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while creating payment: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateStatusByPaymentID", Name = "UpdateStatusByPaymentID")]
        public IActionResult UpdateStatusByPaymentID([FromBody] UpdateStatusByPaymentIDRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                bool result = clsPaymentBL.UpdateStatusByPaymentID(model.PaymentID, model.Status);

                if (!result)
                    return NotFound($"Payment with ID = {model.PaymentID} was not found or status not updated.");

                return Ok(new
                {
                    message = "Payment status updated successfully.",
                    model.PaymentID,
                    model.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating payment status: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateStatusByReservationID", Name = "UpdateStatusByReservationID")]
        public IActionResult UpdateStatusByReservationID([FromBody] UpdateStatusByReservationIDRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                bool result = clsPaymentBL.UpdateStatusByReservationID(model.ReservationID, model.Status);

                if (!result)
                    return NotFound($"No payments found for ReservationID = {model.ReservationID} or status not updated.");

                return Ok(new
                {
                    message = "Payments status updated successfully for reservation.",
                    model.ReservationID,
                    model.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating payments status by reservation: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("UpdatePaymentByPaymentID", Name = "UpdatePaymentByPaymentID")]
        public ActionResult<PaymentDTO> UpdatePaymentByPaymentID([FromBody] UpdatePaymentByPaymentIDRequest model)
        {



            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var paymentBL = new clsPaymentBL();


                paymentBL.PaymentID = model.PaymentID;
                paymentBL.Amount = model.Amount;
                paymentBL.PaymentDetails = model.PaymentDetails;
                paymentBL.Status = model.Status;
                paymentBL.Mode=clsPaymentBL.enMode.Update;
                bool isSaved = paymentBL.Save();

                if (!isSaved)
                    return BadRequest("Failed to add payment.");


                var helper = new clsPaymentBL();
                var dto = clsPaymentBL.GetPaymentsByPaymentID(paymentBL.PaymentID);

                if (dto == null)
                    return Ok(new
                    {
                        paymentBL.PaymentID,
                        message = "Payment added, but could not load PaymentDTO. Check mapping."
                    });

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while creating payment: {ex.Message}");
            }
            #endregion
        }
    }
}