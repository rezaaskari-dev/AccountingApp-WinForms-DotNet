using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity.Infrastructure;

namespace Accounting.App.Accounting
{
    public partial class frmReport : Form
    {
        public int TypeID = 0;

        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomersViewModel> list = new List<ListCustomersViewModel>();

                list.Add(new ListCustomersViewModel()
                {
                    CustomerID = 0,
                    FullName = "انتخاب کنید"
                });

                list.AddRange(db.CustomerRepository.GetNameCustomers());

                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }

            if (TypeID == 1)
            {
                this.Text = "گزارش دریافتی ها";
            }
            else
            {
                this.Text = "گزارش پرداختی ها";
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result =
                    new List<DataLayer.Accounting>();

                DateTime? startDate;
                DateTime? endDate;

                // فیلتر بر اساس طرف حساب
                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId =
                        int.Parse(cbCustomer.SelectedValue.ToString());

                    result.AddRange(
                        db.AccountingRepository.Get(
                            a => a.TypeID == TypeID &&
                                 a.CustumerID == customerId
                        )
                    );
                }
                else
                {
                    result.AddRange(
                        db.AccountingRepository.Get(
                            a => a.TypeID == TypeID
                        )
                    );
                }

                // فیلتر تاریخ شروع
                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);

                    startDate =
                        DateConvertor.ToMiladi(startDate.Value);

                    result = result
                        .Where(r => r.DateTitle >= startDate.Value)
                        .ToList();
                }

                // فیلتر تاریخ پایان
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);

                    endDate =
                        DateConvertor.ToMiladi(endDate.Value);

                    result = result
                        .Where(r => r.DateTitle <= endDate.Value)
                        .ToList();
                }

                // پاک کردن اطلاعات قبلی جدول
                dgvReport.Rows.Clear();

                // اضافه کردن اطلاعات جدید
                foreach (var accounting in result)
                {
                    string customerName =
                        db.CustomerRepository.GetCustomerNameById(
                            accounting.CustumerID
                        );

                    dgvReport.Rows.Add(
                        accounting.ID,                    // Cells[0]
                        customerName,                     // Cells[1]
                        accounting.Amount,                // Cells[2]
                        accounting.DateTitle.ToShamsi(), // Cells[3]
                        accounting.Description           // Cells[4]
                    );
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int id = int.Parse(
                    dgvReport.CurrentRow.Cells[0].Value.ToString()
                );

                if (RtlMessageBox.Show(
                    "آیا از حذف مطمئن هستید ؟",
                    "هشدار",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) == DialogResult.Yes)
                {
                    try
                    {
                        using (UnitOfWork db = new UnitOfWork())
                        {
                            db.AccountingRepository.Delete(id);
                            db.Save();
                        }

                        Filter();

                        RtlMessageBox.Show(
                            "اطلاعات با موفقیت حذف شد.",
                            "پیام",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    catch (Exception)
                    {
                        RtlMessageBox.Show(
                            "حذف اطلاعات انجام نشد.",
                            "خطا",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
            else
            {
                RtlMessageBox.Show(
                    "لطفاً یک مورد را انتخاب کنید",
                    "توجه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int id = int.Parse(
                    dgvReport.CurrentRow.Cells[0].Value.ToString()
                );

                frmNewAccounting frmNewAccounting =
                    new frmNewAccounting();

                frmNewAccounting.AccountID = id;

                if (frmNewAccounting.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dbPrint = new DataTable();

            // نام ستون‌ها باید دقیقاً با Report.mrt یکی باشد
            dbPrint.Columns.Add("Customer");
            dbPrint.Columns.Add("Amount");
            dbPrint.Columns.Add("Data");
            dbPrint.Columns.Add("Description");

            foreach (DataGridViewRow item in dgvReport.Rows)
            {
                // ستون 0 = ID است و نباید وارد گزارش شود
                // بنابراین از ستون‌های 1 تا 4 استفاده می‌کنیم

                dbPrint.Rows.Add(
                    item.Cells[1].Value?.ToString() ?? "", // Customer
                    item.Cells[2].Value?.ToString() ?? "", // Amount
                    item.Cells[3].Value?.ToString() ?? "", // Data
                    item.Cells[4].Value?.ToString() ?? ""  // Description
                );
            }

            // بارگذاری فایل گزارش
            stiPrint.Load(
                Application.StartupPath + "/Report.mrt"
            );

            // معرفی DataTable به Stimulsoft
            stiPrint.RegData("DT", dbPrint);

            // نمایش گزارش
            stiPrint.Show();

            // اگر خواستی مستقیم چاپ شود:
            // stiPrint.Print();
        }

        private void dgvReport_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
        }
    }
}