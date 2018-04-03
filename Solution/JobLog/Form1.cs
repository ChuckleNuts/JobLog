using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
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

            this.Text = "JSR Job Logs - v" + version;
        }

        private void Edit(bool value) {
            cmbOpenClosed.Enabled = value;
            txtName.Enabled = value;
            txtContact.Enabled = value;
            txtIMEI.Enabled = value;
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
                txtName.Focus();
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                cmbOpenClosed.Text = "Open";
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appData.jobInfo.RejectChanges();
            }
        }

        private void TsmNew(object sender, EventArgs e) {
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
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
            jobInfoBindingSource.EndEdit();
            jobInfoTableAdapter.Update(appData.jobInfo);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                jobInfoBindingSource.RemoveCurrent();
            jobInfoBindingSource.EndEdit();
            jobInfoTableAdapter.Update(appData.jobInfo);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete)
                if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    jobInfoBindingSource.RemoveCurrent();
            jobInfoBindingSource.EndEdit();
            jobInfoTableAdapter.Update(appData.jobInfo);
        }
        //
        // Search database 
        //
        private void txtSearch_TextChanged(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(txtSearch.Text)) {
                jobInfoBindingSource.Filter = string.Format("Name LIKE '{0}%' OR Contact LIKE '{1}%' OR IMEI LIKE '{2}%'", txtSearch.Text, txtSearch.Text, txtSearch.Text);
            } else {
                jobInfoBindingSource.Filter = string.Empty;
            }
        }
        //
        // Check for updates
        //
        private void checkForUpdatesToolStripMenuItem1_Click(object sender, EventArgs e) {
            UpdateCheckInfo info;

            if (ApplicationDeployment.IsNetworkDeployed) {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                try {
                    info = ad.CheckForDetailedUpdate();
                } catch (DeploymentDownloadException dde) {
                    MessageBox.Show("The new version of the application can't be downloaded at this time. \n\nPlease Check your network connection or try again later. Error: " + dde.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } catch (InvalidDeploymentException ide) {
                    MessageBox.Show("Can't check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } catch (InvalidOperationException ioe) {
                    MessageBox.Show("This application can't be updated. It's likely not a ClickOnce application. Error: " + ioe.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (info.UpdateAvailable) {
                    if (MessageBox.Show("A newer version is available. Would you like to update now?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        try {
                            ad.Update();
                            Application.Restart();
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                } else {
                    MessageBox.Show("You are running the latest version.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        // 
        // Shows version number on form
        //
        private static Version version = new Version(Application.ProductVersion);

        public static Version Version {
            get {
                return version;
            }
        }
        //
        // Exits Application
        //
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }
        //
        // Cut
        // 
        // TODO - Fix
        private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
            TextBox txtBox = this.ActiveControl as TextBox;
            if (txtBox.SelectedText != string.Empty)
                Clipboard.SetData(DataFormats.Text, txtBox.SelectedText);
            txtBox.SelectedText = string.Empty;
        }
        //
        // Copy
        //
        // TODO - Fix
        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            TextBox txtBox = this.ActiveControl as TextBox;
            if (txtBox.SelectedText != string.Empty)
                Clipboard.SetData(DataFormats.Text, txtBox.SelectedText);

        }
        //
        // Paste
        //
        // TODO - Fix
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
            int position = ((TextBox)this.ActiveControl).SelectionStart;
            this.ActiveControl.Text = this.ActiveControl.Text.Insert(position, Clipboard.GetText());
        }
    }
}
