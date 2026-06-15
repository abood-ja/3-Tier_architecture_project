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
    }
}
