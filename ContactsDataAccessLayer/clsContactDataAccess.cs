using System;
using System.Data;
using System.Data.SqlClient;

namespace ContactsDataAccessLayer
{
    public class clsContactDataAccess
    {
        public static bool GetContactInfoByID(int ID,ref string FirstName, ref string LastName,
            ref string Email, ref string Phone, ref string Address,
            ref DateTime DateOfBirth, ref int CountryID,ref string ImagePath
            )
        {
            bool isFound = false;
            SqlConnection connection=new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from Contacts where ContactID=@ContactID";
            SqlCommand command=new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@ContactID", ID);
            try
            {
                connection.Open();
                SqlDataReader reader= command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    FirstName = reader["FirstName"] == DBNull.Value ? "" : (string)reader["FirstName"];
                    LastName = reader["LastName"] == DBNull.Value ? "" : (string)reader["LastName"];
                    Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"];
                    Phone = reader["Phone"] == DBNull.Value ? "" : (string)reader["Phone"];
                    Address = reader["Address"] == DBNull.Value ? "" : (string)reader["Address"];
                    ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];

                    CountryID = reader["CountryID"] == DBNull.Value ? -1 : (int)reader["CountryID"];
                    DateOfBirth = reader["DateOfBirth"] == DBNull.Value
                        ? DateTime.MinValue
                        : (DateTime)reader["DateOfBirth"];

                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound=false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();

            }
            return isFound;
        }
        public static int AddNewContact( string FirstName,  string LastName,
             string Email,  string Phone,  string Address,
            DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            int ContactID = -1;
            SqlConnection connection =new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query= @"INSERT INTO Contacts (FirstName, LastName, Email, Phone, Address,DateOfBirth, CountryID,ImagePath)
                             VALUES (@FirstName, @LastName, @Email, @Phone, @Address,@DateOfBirth, @CountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);

            if(ImagePath != null)
            {
            command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
            command.Parameters.AddWithValue("@ImagePath",System.DBNull.Value);
            }
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                    ContactID = insertedID;
                }
            }
            catch (Exception ex) {

            }
            finally
            {
                connection.Close();
            }

            return ContactID;




        }
    }
}
