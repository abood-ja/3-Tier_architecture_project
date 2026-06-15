using ContactsDataAccessLayer;
using System.Reflection;

namespace ContactsBusinessLayer
{
    public class clsContact
    {
        public clsContact(int iD, string firstName, string lastName, string email, string phone, string address, DateTime dateOfBirth, int countryID, string imagePath)
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
    }
}
