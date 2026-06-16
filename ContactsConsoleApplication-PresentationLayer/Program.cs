using ContactsBusinessLayer;
using System.Data;

namespace ContactsConsoleApplication_PresentationLayer
{
    public class Program
    {
        static void testFindContact(int ID)
        {
            clsContact Contact1 = clsContact.Find(ID);
            if (Contact1 != null) {
                Console.WriteLine(Contact1.FirstName+ " " + Contact1.LastName);
                Console.WriteLine(Contact1.Email);
                Console.WriteLine(Contact1.Phone);
                Console.WriteLine(Contact1.Address);
                Console.WriteLine(Contact1.DateOfBirth);
                Console.WriteLine(Contact1.CountryID);
                Console.WriteLine(Contact1.ImagePath);
            }
            else
            {
                Console.WriteLine($"Contat [{ID}] is not found ");
            }
        }
        static void testAddNewContact()
        {
            clsContact Contact1 = new clsContact();
            Contact1.FirstName = "abood";
            Contact1.LastName = "jarrar";
            Contact1.Email = "abood@gmail.com";
            Contact1.Phone = "059928";
            Contact1.Address = "nablus";
            Contact1.DateOfBirth = new DateTime(2000, 2, 1, 10, 20, 0);
            Contact1.CountryID = 1;
            Contact1.ImagePath = "";
            if (Contact1.Save())
            {
                Console.WriteLine($"contact with the ID [{Contact1.ID}]  was added succesfully!");
            }
        }
        static void testUpdateContact(int ID)
        {
            clsContact Contact1=clsContact.Find(ID);
            if (Contact1!=null)
            {
                Contact1.FirstName = "abood";
                Contact1.LastName = "jarrar";
                Contact1.Email = "abood@gmail.com";
                Contact1.Phone = "059928";
                Contact1.Address = "nablus";
                Contact1.DateOfBirth = new DateTime(2000, 2, 1, 10, 20, 0);
                Contact1.CountryID = 1;
                Contact1.ImagePath = "";
                if (Contact1.Save())
                {
                    Console.WriteLine($"contact with the id [{ID}] updated succesfully");
                }
            }
        }
        
        static void testDeleteContact(int ID)
        {
            if (clsContact.DeleteContact(ID))
            {
                Console.WriteLine($"the contact with the id[{ID}] was deleted");
            }
            else
            {
                Console.WriteLine($"Failed to delete the contact");
            }
        }
        static void testListContacts()
        {
            DataTable dataTable = clsContact.GetAllContacts();
            Console.WriteLine("Contacts Data: ");
            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"{row["ContactID"]},     {row["FirstName"]} {row["LastName"]}");
            }
        }
        static void Main(string[] args)
        {
            testListContacts();
        }
    }
}
