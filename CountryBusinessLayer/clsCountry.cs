using CountryDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesBusinessLayer
{
    public class clsCountry
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int CountryID { set; get; }
        public string CountryName { set; get; }
        public clsCountry() {
            this.CountryID = -1;
            this.CountryName = "";
            Mode = enMode.AddNew; ;
        }

        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID= CountryID;
            this.CountryName=CountryName;
            Mode = enMode.Update;
        }

        public static clsCountry FindCountryByID(int CountryID)
        {
            string CountryName = "";
            if (clsCountryDataAccess.GetCountryInfoByID(CountryID,ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountry FindCountryByName(string CountryName)
        {
            int CountryID = -1;
            if (clsCountryDataAccess.GetCountryInfoByName(ref CountryID,  CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewCountry()
        {
            this.CountryID = clsCountryDataAccess.AddNewCountry(this.CountryName);
            return this.CountryID != -1;
        }

        private bool _UpdateCountry()
        {

            return clsCountryDataAccess.UpdateCountry(this.CountryID,this.CountryName);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateCountry();
            }


            return false;

        }

        public static bool DeleteCountry(int CountryID)
        {
            return clsCountryDataAccess.DeleteCountry(CountryID);
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries();
        }

        public static bool IsCountryExistByID(int CountryID)
        {
            return clsCountryDataAccess.IsCountryExistByID(CountryID);
        }

        public static bool IsCountryExistByName(string CountryName)
        {
            return clsCountryDataAccess.IsCountryExistByName(CountryName);
        }
    }
}
