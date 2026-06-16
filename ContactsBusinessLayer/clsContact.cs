using ContactsDataAccessLayer;
using System.Data;
using System.Reflection;

namespace ContactsBusinessLayer
{
    public class clsContact
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        private clsContact(int iD, string firstName, string lastName, string email, string phone, string address, DateTime dateOfBirth, int countryID, string imagePath)
        {
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            DateOfBirth = dateOfBirth;
            CountryID = countryID;
            ImagePath = imagePath;
            Mode = enMode.Update;
        }

        public clsContact()
        {
            this.ID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.Address = "";
            this.DateOfBirth=DateTime.Now;
            this.CountryID = -1;
            this.ImagePath = "";
            Mode=enMode.AddNew;



        }
        public int ID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public DateTime DateOfBirth { set; get; }

        public string ImagePath { set; get; }

        public int CountryID { set; get; }

        public static clsContact Find(int ID)
        {
            string FirstName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int CountryID = -1;

            if (clsContactDataAccess.GetContactInfoByID(ID,ref FirstName,ref LastName, ref  Email,
                ref  Phone,ref Address,ref DateOfBirth,ref CountryID,ref ImagePath))
            {
                return new clsContact(ID,FirstName,LastName,Email,Phone,Address,DateOfBirth,CountryID,ImagePath);   
            }

            else
            {
                return null;
            }




        }

        private bool _AddNewContact()
        {
            this.ID=clsContactDataAccess.AddNewContact(this.FirstName,this.LastName,this.Email,this.Phone,this.Address,
                this.DateOfBirth,this.CountryID,this.ImagePath);
            return this.ID != -1;
        }

        private bool _UpdateContact()
        {

            return clsContactDataAccess.UpdateContact(this.ID, this.FirstName, this.LastName, this.Email, this.Phone, this.Address,
                this.DateOfBirth, this.CountryID, this.ImagePath);
        }

        public static bool DeleteContact(int ID)
        {
            return clsContactDataAccess.DeleteContact(ID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewContact())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return  _UpdateContact();
            }


            return false;

        }

        public static DataTable GetAllContacts()
        {
            return clsContactDataAccess.GetAllContacts();
        }
    }
}
