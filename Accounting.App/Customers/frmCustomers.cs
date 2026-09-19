using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            BindGrid();


        }

        void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgCustomers.AutoGenerateColumns = false;
                dgCustomers.DataSource = db.CustomerRepository.GetAllCustomers();
            }
        }

        private void btnDeleteCustomers_Click(object sender, EventArgs e)
        {
            if (dgCustomers.CurrentRow != null)
            {
                string name = dgCustomers.CurrentRow.Cells[1].Value.ToString();

                if (RtlMessageBox.Show(
                    $"آیا از حذف {name} مطمئن هستید ؟",
                    "توجه",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) == DialogResult.Yes)
                {
                    try
                    {
                        using (UnitOfWork db = new UnitOfWork())
                        {
                            int customerId = int.Parse(
                                dgCustomers.CurrentRow.Cells[0].Value.ToString()
                            );

                            bool result =
                                db.CustomerRepository.DeleteCustomer(customerId);

                            if (result)
                            {
                                db.Save();
                                BindGrid();

                                RtlMessageBox.Show(
                                    "شخص با موفقیت حذف شد.",
                                    "پیام",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
                            }
                            else
                            {
                                RtlMessageBox.Show(
                                    "حذف شخص انجام نشد.",
                                    "خطا",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                            }
                        }
                    }
                    catch (Exception)
                    {
                        RtlMessageBox.Show(
                            "این شخص دارای تراکنش است و امکان حذف آن وجود ندارد.\n\n" +
                            "لطفاً ابتدا تراکنش‌های مربوط به این شخص را حذف کنید.",
                            "خطا در حذف",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
            else
            {
                RtlMessageBox.Show(
                    "لطفاً شخصی را انتخاب کنید",
                    "توجه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                dgCustomers.DataSource = unitOfWork.CustomerRepository.GetCustomersByFilter(txtFilter.Text);
            }
        }

        private void btnRefreshCustomers_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            BindGrid();
        }

        private void dgCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnNewAddCustomer_Click(object sender, EventArgs e)
        {
            frmAddOrEditCustomer frmAdd = new frmAddOrEditCustomer();
            if (frmAdd.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }



        }




        private void btnEditCustomers_Click(object sender, EventArgs e)
        {
            if (dgCustomers.CurrentRow != null)
            {
                int customerId = int.Parse(dgCustomers.CurrentRow.Cells[0].Value.ToString());
                frmAddOrEditCustomer frmAddOrEdit = new frmAddOrEditCustomer();
                frmAddOrEdit.customerId = customerId;
                if (frmAddOrEdit.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }

            }

        }
    }

}


