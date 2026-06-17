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
                Console.WriteLine($"{Country1.CountryID},  {Country1.CountryName}");
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
                Console.WriteLine($"{Country1.CountryID},  {Country1.CountryName}");
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
                Console.WriteLine($"{row["CountryID"]},     {row["CountryName"]} ");
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
        
        }
    }
}
