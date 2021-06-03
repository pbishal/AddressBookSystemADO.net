using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookADO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to AddressBook System program using ADO.NET");
            ///Creating Instance object of AddressBookRepository class.
            AddressBookRepository repository = new AddressBookRepository();
            ///UC1 Creating a method for checking for the validity of the connection.
            //repository.EnsureDataBaseConnection();
            AddNewContactDetails();
            Console.ReadLine();


        }
        /// UC 3: Adds the new contact into DB table.
        public static void AddNewContactDetails()
        {
            AddressBookRepository repository = new AddressBookRepository();
            AddressBookModel model = new AddressBookModel();
            model.FirstName = "niraj";
            model.LastName = "Kumar";
            model.Address = "Narwa";
            model.City = "Jamshedpur";
            model.State = "Jharkhand";
            model.Zip = 562258;
            model.PhoneNumber = 9469665685;
            model.EmailId = "nirk@gmail.com";
            //model.AddressBookName = "Niraj";
            //model.AddressBookType = "Friend";
            Console.WriteLine(repository.AddDataToTable(model) ? "Record inserted successfully\n" : "Failed");
        }
    }
}
