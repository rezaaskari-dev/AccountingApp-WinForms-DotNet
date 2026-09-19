using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Repositories
{

    public interface ICustomerRepository
    {
        List<Customers> GetAllCustomers();


        IEnumerable<Customers> GetCustomersByFilter(string parameter);

        List<ListCustomersViewModel> GetNameCustomers(string filter = "");
        int GetCustomerIdByName(string name);

        Customers GetCustomerById(int customerId);
        bool InsertCustomer(Customers customer);
        bool UpdateCustomer(Customers customer);
        //کاستومر بگیره فعال میشه
        bool DeleteCustomer(Customers customer);
        //ایدی بگیره فعال یشه 
        bool DeleteCustomer(int customerId);
        string GetCustomerNameById(int customerId);





    }
}
