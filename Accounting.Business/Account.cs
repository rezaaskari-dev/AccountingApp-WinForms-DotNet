using Accounting.DataLayer.Context;
using Accounting.ViewModels.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Business
{
    public class Account
    {
        public static ReportViewModel ReportFormMain()
        {
            ReportViewModel viewModel = new ReportViewModel();
            using (UnitOfWork db = new UnitOfWork())
            {
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);
                var recive = db.AccountingRepository.Get(a => a.TypeID == 1 && a.DateTitle >= startDate && a.DateTitle <= endDate)
                    .Select(a => a.Amount).ToList();

                var pay = db.AccountingRepository.Get(a => a.TypeID == 2 && a.DateTitle >= startDate && a.DateTitle <= endDate)
                   .Select(a => a.Amount).ToList();
                viewModel.Recive = recive.Sum();
                viewModel.Pay = pay.Sum();
                viewModel.AccountBalance = (recive.Sum() - pay.Sum());


            }
            return viewModel;

        }
    }
}
