using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public  class clsContactMessageBL
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }

        public int ContactMessageID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string phone { set; get; }
        public string Message { set; get; }
        public DateTime CreatedAt { set; get; }


       public clsContactMessageBL(ContactMessageDTO contactMessageDTO,enMode NewMode=enMode.AddNew )
        {
            this.ContactMessageID = contactMessageDTO.ContactMessageID;
            this.Name = contactMessageDTO.Name;
            this.Email = contactMessageDTO.Email;
            this.phone = contactMessageDTO.phone;
            this.Message = contactMessageDTO.Message;
            this.CreatedAt=contactMessageDTO.CreatedAt;
            this.Mode = NewMode;

        }

        public clsContactMessageBL()
        {
            this.ContactMessageID = -1;
            this.Name = "";
            this.Email = "";
            this.phone = "";
            this.Message = "";
            this.CreatedAt =DateTime.Now;


        }
        public ContactMessageDTO CMDTO
        {

            get
            {

                return new ContactMessageDTO(this.ContactMessageID,this.Name,this.Email,this.phone,this.Message,this.CreatedAt);
            }
        }

        public static List <ContactMessageDTO> GetAllContactMessages()
        {
            return clsContactMessageDA.GetAllContactMessages();
        }

        private bool _AddNewContactMessage( )
        {
            return ((this.ContactMessageID = clsContactMessageDA.AddNewContactMessage(this.CMDTO)) != -1);
        }

        public bool Save()
        {

            switch (this.Mode)
            {
                case enMode.AddNew: 


                    if (_AddNewContactMessage())
                    {
                        this.Mode= enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    case enMode.Update:

                    return false; 

            }
            return false;


        }
    }
}
