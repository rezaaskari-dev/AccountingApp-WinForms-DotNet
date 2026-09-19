using Accounting.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Accounting.ViewModels.Customers;
namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {

        private Accounting_DBEntities db;


        public CustomerRepository(Accounting_DBEntities context)
        {
            db = context;
        }


        ////تمام ارتباط بانک ما با همین هست با همین = db
        //Accounting_DBEntities db = new Accounting_DBEntities();



        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Customers> GetAllCustomers()
        {
            return db.Customers.ToList();
        }
        //ما هیچ وقت نم اییم اینجا دستور ایف یا چیز های دیگری بنویسیم خل فقط ارتباط با دیتابیس
        public Customers GetCustomerById(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public int GetCustomerIdByName(string name)
        {
            return db.Customers.First(c => c.FullName == name).CustomerID;
        }

        public string GetCustomerNameById(int customerId)
        {
            return db.Customers.Find(customerId).FullName;
        }

        public IEnumerable<Customers> GetCustomersByFilter(string parameter)
        {
            return db.Customers.Where(c => c.FullName.Contains(parameter) || c.Email.Contains(parameter) || c.Mobile.Contains(parameter)).ToList();
        }

        public List<ListCustomersViewModel> GetNameCustomers(string filter = "")
        {
            if (filter == "")
            {
                return db.Customers.Select(c => new ListCustomersViewModel()
                {
                    CustomerID = c.CustomerID,
                    FullName = c.FullName
                }


                ).ToList();
            }



            return db.Customers.Where(c => c.FullName.Contains(filter)).Select(c => new ListCustomersViewModel()
            {
                CustomerID = c.CustomerID,
                FullName = c.FullName
            }


                ).ToList();
        }



        public bool InsertCustomer(Customers customer)
        {

            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }



        public bool UpdateCustomer(Customers customer)
        {
            //  try
            // {
            var local = db.Set<Customers>()
                .Local
                .FirstOrDefault(f => f.CustomerID == customer.CustomerID);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(customer).State = EntityState.Modified;
            return true;
            // }
            //  catch
            // {
            //      return false;
            // }
        }
    }
}
