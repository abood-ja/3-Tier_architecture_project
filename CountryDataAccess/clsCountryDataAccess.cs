using System;
using System.Data;
using System.Data.SqlClient;

namespace CountryDataAccessLayer
{
    public class clsCountryDataAccess
    {

        public static bool GetCountryInfoByID(int CountryID, ref string CountryName, ref string Code,
            ref string PhoneCode)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from Countries where CountryID=@CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryName = (string)reader["CountryName"];
                    Code = reader["Code"] == DBNull.Value ? "" : (string)reader["Code"];
                    PhoneCode = reader["PhoneCode"]==DBNull.Value?"": (string)reader["PhoneCode"];

                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        public static int AddNewCountry(string CountryName,string Code,string PhoneCode)
        {
            int CountryID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"
    INSERT INTO Countries
        (CountryName, Code, PhoneCode)
    VALUES
        (@CountryName, @Code, @PhoneCode);

    SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);

            if (Code != null)
            {
                command.Parameters.AddWithValue("@Code", Code);
            }
            else
            {
                command.Parameters.AddWithValue("@Code", System.DBNull.Value);
            }

            if (PhoneCode != null)
            {
                command.Parameters.AddWithValue("@PhoneCode", PhoneCode);
            }
            else
            {
                command.Parameters.AddWithValue("@PhoneCode", System.DBNull.Value);
            }

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    CountryID = insertedID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return CountryID;
        }

        public static bool UpdateCountry(int CountryID, string CountryName, string Code, string PhoneCode)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"
    UPDATE Countries
    SET
        CountryName = @CountryName,
        Code = @Code,
        PhoneCode = @PhoneCode
    WHERE CountryID = @CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            command.Parameters.AddWithValue("@Code", Code);
            command.Parameters.AddWithValue("@PhoneCode", PhoneCode);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;
        }

        public static bool DeleteCountry(int CountryID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"delete from Countries 
                           where CountryID=@CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;
        }

        public static DataTable GetAllCountries()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from Countries ";
            SqlCommand command = new SqlCommand(query, connection);
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;

        }

        public static bool IsCountryExistByID(int CountryID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select isFound=1 from Countries where CountryID=@CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool IsCountryExistByName(string CountryName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select isFound=1 from Countries where CountryName=@CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool GetCountryInfoByName(ref int CountryID, string CountryName,ref string Code, ref string PhoneCode)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from Countries where CountryName=@CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryID = (int)reader["CountryID"];
                    Code = reader["Code"] == DBNull.Value ? "" : (string)reader["Code"];
                    PhoneCode = reader["PhoneCode"] == DBNull.Value ? "" : (string)reader["PhoneCode"];

                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }
    }
}
