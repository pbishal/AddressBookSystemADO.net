using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookADO
{
    public class AddressBookRepository
    {
        /// Ensuring the established connection using the Sql connection specifying the property. 
        public static SqlConnection connection { get; set; }

        ///UC1 Creating a method for checking for the validity of the connection.

        public void EnsureDataBaseConnection()
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            using (connection)
            {
                Console.WriteLine("The Connection is created");
            }
            connection.Close();
        }

        /// UC2 Getting all the stored records in the address book service table by fetching all the records

        public void GetAllContact()
        {
            ///Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            AddressBookModel model = new AddressBookModel();
            try
            {
                using (connection)
                {
                    /// Query to get all the data from the table
                    string query = @"select * from dbo.Address_Book";
                    /// Impementing the command on the connection fetched database table
                    SqlCommand command = new SqlCommand(query, connection);
                    ///Opening the connection.
                    connection.Open();
                    /// executing the sql data reader to fetch the records
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        /// Mapping the data to the employee model class object
                        while (reader.Read())
                        {
                            model.FirstName = reader.GetString(0);
                            model.LastName = reader.GetString(1);
                            model.Address = reader.GetString(2);
                            model.City = reader.GetString(3);
                            model.State = reader.GetString(4);
                            model.Zip = reader.GetInt32(5);
                            model.PhoneNumber = reader.GetInt32(6);
                            model.EmailId = reader.GetString(7);

                            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", model.FirstName, model.LastName,
                                model.Address, model.City, model.State, model.Zip, model.PhoneNumber, model.EmailId, model.AddressBookType, model.AddressBookName);
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            /// Catching the null record exception
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /// Always ensuring the closing of the connection
            finally
            {
                connection.Close();
            }
        }

        /// UC3 Method to insert contact to the table using a stored procedure

        public bool AddDataToTable(AddressBookModel model)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                /// Using the connection established
                using (connection)
                {
                    /// Implementing the stored procedure
                    SqlCommand command = new SqlCommand("dbo.spAddContactDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@AddressDetails", model.Address);
                    command.Parameters.AddWithValue("@City", model.City);
                    command.Parameters.AddWithValue("@StateName", model.State);
                    command.Parameters.AddWithValue("@Zip", model.Zip);
                    command.Parameters.AddWithValue("@PhoneNo", model.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", model.EmailId);
                    command.Parameters.AddWithValue("@addressBookType", model.AddressBookType);
                    command.Parameters.AddWithValue("@addressBookName", model.AddressBookName);

                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    /// Return the result of the transaction i.e. the dml operation to update data
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            /// Catching any type of exception generated during the run time
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        ///  UC 4 Ability to Edit the contactType of the existing contact.

        public bool EditContactUsingName(string FirstName, string LastName, string addressBookType)
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception.
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = @"update dbo.Address_Book set addressBookType = @parameter1
                    where FirstName = @parameter2 and LastName = @parameter3";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@parameter1", addressBookType);
                    command.Parameters.AddWithValue("@parameter2", FirstName);
                    command.Parameters.AddWithValue("@parameter3", LastName);
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            /// Catching any type of exception generated during the run time 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
