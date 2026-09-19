using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UnitOfWork db = new UnitOfWork();
            var list  = db.CustomerRepository.GetAllCustomers();
            db.Dispose();



            //Accounting_DBEntities db = new Accounting_DBEntities();

            //ICustomerRepository customer = new CustomerRepository(db);

            //Customers Addcustomer = new Customers()
            //{
            //    FullName = "rezatest",
            //    CustomerImage = "photo",
            //    Mobile = "09120922564"

            //};
            //customer.InsertCustomer(Addcustomer);
            //customer.Save();


            //var list = customer.GetAllCustomers();


        }
    }
}
