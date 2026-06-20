using System;
using System.Data;
using System.Data.SqlClient;
using StackExchange.Redis;
namespace ContactsDataAccessLayer
{
    public class clsContactDataAccess
    {

        private static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost:6379");

        private static readonly IDatabase _cache = _redis.GetDatabase();
        public static bool GetContactInfoByID(int ID,ref string FirstName, ref string LastName,
            ref string Email, ref string Phone, ref string Address,
            ref DateTime DateOfBirth, ref int CountryID,ref string ImagePath
            )
        {
            bool isFound = false;
            string key = $"contact:{ID}";
            string cashedContactData = _cache.StringGet(key);
            if (!string.IsNullOrEmpty(cashedContactData))
            {
                string[] data = cashedContactData.Split("|");
                FirstName = data[0];
                LastName = data[1];
                Email = data[2];
                Phone = data[3];
                Address = data[4];
                DateOfBirth = DateTime.Parse(data[5], null, System.Globalization.DateTimeStyles.RoundtripKind);
                CountryID = int.Parse(data[6]);
                ImagePath = data[7];
                Console.WriteLine("Retrieved from Redis");
                return true;
            }
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

                    ///save to the cashe 
                    string contactData =$"{FirstName}|{LastName}|{Email}|{Phone}|{Address}|{DateOfBirth:O}|{CountryID}|{ImagePath}";
                    _cache.StringSet(key, contactData, TimeSpan.FromMinutes(5));
                    Console.WriteLine("Saved to Redis");
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
        public static bool UpdateContact(int ID, string FirstName, string LastName,
             string Email, string Phone, string Address,
            DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            int rowsAffected = 0;
            SqlConnection connection =new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query= @"Update  Contacts  
                            set FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address, 
                                DateOfBirth = @DateOfBirth,
                                CountryID = @CountryID,
                                ImagePath =@ImagePath
                                where ContactID = @ContactID";
            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ContactID", ID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            if (ImagePath != null)
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
                    rowsAffected=command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                }
                finally
                {
                    connection.Close();
                }
            if (rowsAffected > 0)
            {
                _cache.KeyDelete($"contact:{ID}");

            }
            return rowsAffected > 0;
        }

        public static bool DeleteContact(int ContactID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query= @"Delete from Contacts 
                                where ContactID = @ContactID";
            SqlCommand command=new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@ContactID", ContactID);
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

            if (rowsAffected > 0)
            {
                _cache.KeyDelete($"contact:{ContactID}");

            }
            return rowsAffected > 0;
        }

        public static DataTable GetAllContacts()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from Contacts";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader=command.ExecuteReader();
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

        public static bool IsContactExist(int ContactID)
        {
            bool isFound = false;
            string key = $"contact:{ContactID}";
            string cashedContactData = _cache.StringGet(key);
            if (!string.IsNullOrEmpty(cashedContactData))
            {
                return true;
            }
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select isFound=1 from Contacts where ContactID=@ContactID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ContactID", ContactID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
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
        
        public static bool GetCountryInfoByID( int CountryID, ref string CountryName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from Countries where CountryID=@CountryID";
            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                connection.Open();
                SqlDataReader reader=command.ExecuteReader();
                if (reader.Read())
                {
                    CountryName = (string)reader["CountryName"];
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

        public static int AddNewCountry(string CountryName)
        {
            int CountryID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"
                INSERT INTO Countries
                  (CountryName)
                VALUES
                (@CountryName);

                SELECT SCOPE_IDENTITY();";
            SqlCommand command=new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);
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

            }
            finally
            {
                connection.Close();
            }
            return CountryID;
        }

        public static bool UpdateCountry(int CountryID,string CountryName)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE Countries
                            SET CountryName = @CountryName
                            WHERE CountryID=@CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            command.Parameters.AddWithValue("@CountryName", CountryName);
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
            catch(Exception ex)
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
            SqlCommand command=new SqlCommand(query,connection);
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader= command.ExecuteReader();
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
                SqlDataReader reader=command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch(Exception ex)
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

        public static bool GetCountryInfoByName( ref int CountryID,string CountryName)
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
