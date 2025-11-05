using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsPersonBl
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        public int PersonID { set; get; }
        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string Phone { set; get; }

        public string Email { set; get; }

        public bool IsMale { set; get; }

        public DateTime DateOfBirth { set; get; }

        public string Image { set; get; }

        public int CountryID { set; get; }


        public clsPersonBl()
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Phone = "";
            this.Email = "";
            this.IsMale = true;
            this.DateOfBirth = DateTime.Now;
            this.Image = "";
            this.CountryID = -1;
            Mode = enMode.AddNew;





        }
        public clsPersonBl(PersonDTO PersonDto, enMode NewMode = enMode.AddNew)
        {


            this.PersonID = PersonDto.PersonID;
            this.FirstName = PersonDto.FirstName;
            this.LastName = PersonDto.LastName;
            this.Phone = PersonDto.Phone;
            this.Email = PersonDto.Email;
            this.IsMale = PersonDto.IsMale;
            this.DateOfBirth = PersonDto.DateOfBirth;
            this.Image = PersonDto.Image;
            this.CountryID = PersonDto.CountryID;
            Mode = NewMode;

        }
        public PersonDTO PDTO
        {
            get
            {
                return (new PersonDTO(this.PersonID, this.FirstName, this.LastName, this.Phone, this.Email, this.IsMale, this.DateOfBirth, this.Image, this.CountryID));
            }
        }

        public static clsPersonBl GetPersonByID(int PersonID)
        {

            PersonDTO Person = clsPersonDA.GetPersonByID(PersonID);
            if (Person != null)
            {

                return new clsPersonBl(Person);


            }
            else
            {
                return null;
            }


        }

        private bool _AddNewPerson(PersonDTO Person)
        {
            this.PersonID = clsPersonDA.AddNewPerson(Person);

            return (this.PersonID != -1);


        }

        public bool Save()
        {

            switch (Mode)
            {

                case enMode.AddNew:


                    if (_AddNewPerson(this.PDTO))
                    {
                        Mode = enMode.Update;
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
