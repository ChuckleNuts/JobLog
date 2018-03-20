using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobLog {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // This line of code loads data into the 'appData.jobInfo' table. You can move, or remove it, as needed.
            this.jobInfoTableAdapter.Fill(this.appData.jobInfo);

            Edit(false);
        }

        private void Edit(bool value) {
            txtDate.Enabled = value;
            cmbOpenClosed.Enabled = value;
            txtName.Enabled = value;
            txtContact.Enabled = value;
            txtNote.Enabled = value;
        }
        //
        // Create new job 
        //
        private void btnNew_Click(object sender, EventArgs e) {
            try {
                Edit(true);
                appData.jobInfo.AddjobInfoRow(appData.jobInfo.NewjobInfoRow());
                jobInfoBindingSource.MoveLast();
                txtDate.Focus();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appData.jobInfo.RejectChanges();
            }
        }

        private void tsmNew_Click(object sender, EventArgs e) {
            try {
                Edit(true);
                appData.jobInfo.AddjobInfoRow(appData.jobInfo.NewjobInfoRow());
                jobInfoBindingSource.MoveLast();
                txtDate.Focus();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appData.jobInfo.RejectChanges();
            }
        }
        //
        // Save job to database
        //
        private void btnSave_Click(object sender, EventArgs e) {
            try {
                Edit(false);
                jobInfoBindingSource.EndEdit();
                jobInfoTableAdapter.Update(appData.jobInfo);
                dataGridView1.Refresh();
                txtDate.Focus();
                MessageBox.Show("Your data has been successfully saved.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appData.jobInfo.RejectChanges();
            }
        }

        private void tsnSave_Click(object sender, EventArgs e) {
            try {
                Edit(false);
                jobInfoBindingSource.EndEdit();
                jobInfoTableAdapter.Update(appData.jobInfo);
                dataGridView1.Refresh();
                txtDate.Focus();
                MessageBox.Show("Your data has been successfully saved.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appData.jobInfo.RejectChanges();
            }
        }
        //
        // Enable job editing
        //
        private void btnEdit_Click(object sender, EventArgs e) {
            Edit(true);
            txtDate.Focus();
        }

        private void tsmEdit_Click(object sender, EventArgs e) {
            Edit(true);
            txtDate.Focus();
        }
        //
        // Delete record
        //
        private void btnDelete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                jobInfoBindingSource.RemoveCurrent();
        }

        private void tsmDelete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                jobInfoBindingSource.RemoveCurrent();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete)
                if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    jobInfoBindingSource.RemoveCurrent();
        }
        //
        // Search database 
        //
        // TODO - make work
        private void button1_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(txtSearch.Text)) {
                (dataGridView1.DataSource as System.Data.DataTable).DefaultView.RowFilter = string.Empty;
            } else {
                (dataGridView1.DataSource as System.Data.DataTable).DefaultView.RowFilter = string.Format("Name = '{0}'", txtSearch.Text);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (!string.IsNullOrEmpty(txtSearch.Text)) {
                    (dataGridView1.DataSource as System.Data.DataTable).DefaultView.RowFilter = string.Empty;
                } else {
                    (dataGridView1.DataSource as System.Data.DataTable).DefaultView.RowFilter = string.Format("Name = '{0}'", txtSearch.Text);
                }
            }
        }
    }
}
