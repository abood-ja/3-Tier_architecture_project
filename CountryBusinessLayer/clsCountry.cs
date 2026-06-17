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

        public string Code { set; get; }

        public string PhoneCode { set; get; }


        public clsCountry() {
            this.CountryID = -1;
            this.CountryName = "";
            this.Code = "";
            this.PhoneCode = "";

            Mode = enMode.AddNew; ;
        }

        private clsCountry(int CountryID, string CountryName,string Code,string PhoneCode)
        {
            this.CountryID= CountryID;
            this.CountryName=CountryName;
            this.Code=Code;
            this.PhoneCode=PhoneCode;
            Mode = enMode.Update;
        }

        public static clsCountry FindCountryByID(int CountryID)
        {
            string CountryName = "";
            string Code = "";
            string PhoneCode = "";

            if (clsCountryDataAccess.GetCountryInfoByID(CountryID,ref CountryName,ref Code,ref PhoneCode))
            {
                return new clsCountry(CountryID, CountryName,Code,PhoneCode);
            }
            else
            {
                return null;
            }
        }

        public static clsCountry FindCountryByName(string CountryName)
        {
            int CountryID = -1;
            string Code = "";
            string PhoneCode = "";

            if (clsCountryDataAccess.GetCountryInfoByName(ref CountryID,  CountryName,ref Code,ref PhoneCode))
            {
                return new clsCountry(CountryID, CountryName, Code, PhoneCode);
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewCountry()
        {
            this.CountryID = clsCountryDataAccess.AddNewCountry(this.CountryName,this.Code,this.PhoneCode);
            return this.CountryID != -1;
        }

        private bool _UpdateCountry()
        {

            return clsCountryDataAccess.UpdateCountry(this.CountryID,this.CountryName,this.Code,this.PhoneCode);
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
