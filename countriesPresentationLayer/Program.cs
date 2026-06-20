using System.Data;
using CountriesBusinessLayer;

namespace CountriesPresentationLayer
{
    public class Program
    {
        static void testFindCountryByID(int ID)
        {
            clsCountry Country1 = clsCountry.FindCountryByID(ID);
            if (Country1 != null)
            {
                Console.WriteLine($"{Country1.CountryID},  {Country1.CountryName} , {Country1.PhoneCode}");
            }
            else
            {
                Console.WriteLine($"Country [{ID}] is not found ");
            }
        }

        static void testFindCountryByName(string CountryName)
        {
            clsCountry Country1 = clsCountry.FindCountryByName(CountryName);
            if (Country1 != null)
            {
                Console.WriteLine($"{Country1.CountryID},  {Country1.CountryName} , {Country1.PhoneCode}");

            }
            else
            {
                Console.WriteLine($"{CountryName} is not found ");
            }
        }
        static void testAddNewCountry()
        {
            clsCountry Country1 = new clsCountry();
            Country1.CountryName = "country 1";
            Country1.Code = "123";
            Country1.PhoneCode = "970";
            if (Country1.Save())
            {
                Console.WriteLine($"Country with the ID [{Country1.CountryID}]  was added succesfully!");
            }
        }
        static void testUpdateCountry(int ID)
        {
            clsCountry Country1 = clsCountry.FindCountryByID(ID);
            if (Country1 != null)
            {
                Country1.CountryName =  "new country";
                Country1.Code = "123";
                Country1.PhoneCode = "970";
                if (Country1.Save())
                {
                    Console.WriteLine($"country with the id [{ID}] updated succesfully");
                }
            }
        }

        static void testDeleteCountry(int ID)
        {
            if (clsCountry.DeleteCountry(ID))
            {
                Console.WriteLine($"the country with the id[{ID}] was deleted");
            }
            else
            {
                Console.WriteLine($"Failed to delete the country");
            }
        }
        static void testListCountries()
        {
            DataTable dataTable = clsCountry.GetAllCountries();
            Console.WriteLine("Countries Data: ");
            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"{row["CountryID"]},     {row["CountryName"]} , {row["PhoneCode"]}");
            }
        }

        static void testIsCountryExistByID(int ID)
        {
            if (clsCountry.IsCountryExistByID(ID))
            {
                Console.WriteLine("Yes country is there");
            }
            else
            {
                Console.WriteLine("No country is not there");
            }
        }

        static void testIsCountryExistByName(string CountryName)
        {
            if (clsCountry.IsCountryExistByName(CountryName))
            {
                Console.WriteLine("Yes country is there");
            }
            else
            {
                Console.WriteLine("No country is not there");
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                string choice;
                Console.WriteLine("1: find country by id");
                Console.WriteLine("2: find country by name");
                choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.WriteLine("enter ID: ");
                    string id;
                    id = Console.ReadLine();
                    testFindCountryByID(int.Parse(id));
                }
                if (choice == "2")
                {
                    Console.WriteLine("enter name: ");
                    string name;
                    name = Console.ReadLine();
                    testFindCountryByName(name);
                }
            }
        }
    }
}
