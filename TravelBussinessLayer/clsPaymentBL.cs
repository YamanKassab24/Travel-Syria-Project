using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsPaymentBL
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }
        public int PaymentID { get; set; }
        public int ReservationID { get; set; }

        public string Method { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentDetails { get; set; }

        public DateTime? TransactionDate { get; set; }

        public enPaymentStatus? Status { get; set; }


        public clsPaymentBL()
        {
            this.PaymentID = -1;
            this.ReservationID = -1;
            this.Method = " ";
            this.Amount = 0;
            this.PaymentDetails = null;
            this.TransactionDate = null;
            this.Status = null;
            this.Mode = enMode.AddNew;

        }

        public clsPaymentBL(PaymentDTO PDTO, enMode NewMode = enMode.AddNew)
        {
            this.PaymentID = PDTO.PaymentID;
            this.ReservationID = PDTO.Reservation.ReservationID;
            this.Method = PDTO.Method;
            this.Amount = PDTO.Amount;
            this.PaymentDetails = PDTO.PaymentDetails;
            this.TransactionDate = PDTO.TransactionDate;
            this.Status = PDTO.Status;
            this.Mode = NewMode;

        }

        public PaymentDTO PDTO
        {
            get
            {
                return new PaymentDTO(this.PaymentID, this.ReservationID, this.Method, this.Amount, this.PaymentDetails, this.TransactionDate, this.Status);
            }
        }

        public static List<PaymentDTO> GetAllPaymentsByReservationID(int ReservationID)
        {
            return clsPaymentDA.GetAllPaymentsByReservationID(ReservationID);
        }
        public static  PaymentDTO GetPaymentsByPaymentID(int PaymentID)
        {
            return clsPaymentDA.GetPaymentsByPaymentID(PaymentID);
        }

        private bool _AddNewPayment()
        {
            return ((this.PaymentID = clsPaymentDA.AddNewPayment(PDTO)) != -1);
        }
        private bool _UpdatePaymentByPaymentID()
        {
            return clsPaymentDA.UpdatePayment(this.PaymentID, this.Amount, this.PaymentDetails, this.Status);
        }


        public bool Save()
        {

            switch (this.Mode)
            {

                case enMode.AddNew:
                    if (_AddNewPayment())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePaymentByPaymentID();


            }
            return false;

        }

        public static bool UpdateStatusByPaymentID(int PaymentID, enPaymentStatus? Status)
        {

            return clsPaymentDA.UpdatePaymentStatusByPaymentID(PaymentID, Status);
        }

        public static bool UpdateStatusByReservationID(int ReservationID, enPaymentStatus? Status)
        {
       return clsPaymentDA.UpdatePaymentStatusByReservationID(ReservationID, Status);
        }



         
    }
}
