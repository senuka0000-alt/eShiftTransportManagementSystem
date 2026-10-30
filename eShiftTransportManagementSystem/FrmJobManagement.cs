
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftTransportManagementSystem
{
    public partial class FrmJobManagement : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int selectedJobID = 0;
        private bool isLoggingOut = false;

        public FrmJobManagement()
        {
            InitializeComponent();

            this.Load -= FrmJobManagement_Load;
            this.Load += FrmJobManagement_Load;

            btnSearchJobs.Click -= btnSearchJobs_Click;
            btnSearchJobs.Click += btnSearchJobs_Click;

            btnClearJobSearch.Click -= btnClearJobSearch_Click;
            btnClearJobSearch.Click += btnClearJobSearch_Click;

            dgvTransportJobs.CellClick -= dgvTransportJobs_CellClick;
            dgvTransportJobs.CellClick += dgvTransportJobs_CellClick;

            btnAcceptJob.Click -= btnAcceptJob_Click;
            btnAcceptJob.Click += btnAcceptJob_Click;

            btnDeclineJob.Click -= btnDeclineJob_Click;
            btnDeclineJob.Click += btnDeclineJob_Click;

            btnUpdateJob.Click -= btnUpdateJob_Click;
            btnUpdateJob.Click += btnUpdateJob_Click;

            btnAssignTransportUnit.Click -= btnAssignTransportUnit_Click;
            btnAssignTransportUnit.Click += btnAssignTransportUnit_Click;

            btnDashboard.Click -= btnDashboard_Click;
            btnDashboard.Click += btnDashboard_Click;

            btnCustomers.Click -= btnCustomers_Click;
            btnCustomers.Click += btnCustomers_Click;

            btnAdmins.Click -= btnAdmins_Click;
            btnAdmins.Click += btnAdmins_Click;

            btnJobs.Click -= btnJobs_Click;
            btnJobs.Click += btnJobs_Click;

            btnProducts.Click -= btnProducts_Click;
            btnProducts.Click += btnProducts_Click;

            btnTransportUnits.Click -= btnTransportUnits_Click;
            btnTransportUnits.Click += btnTransportUnits_Click;

            btnManageJobAssignments.Click -= btnManageJobAssignments_Click;
            btnManageJobAssignments.Click += btnManageJobAssignments_Click;

            btnReports.Click -= btnReports_Click;
            btnReports.Click += btnReports_Click;

            btnLogout.Click -= btnLogout_Click;
            btnLogout.Click += btnLogout_Click;
        }

        private void FrmJobManagement_Load(object sender, EventArgs e)
        {
            cmbFilterJobStatus.Items.Clear();
            cmbFilterJobStatus.Items.Add("All");
            cmbFilterJobStatus.Items.Add("Pending");
            cmbFilterJobStatus.Items.Add("Accepted");
            cmbFilterJobStatus.Items.Add("Declined");
            cmbFilterJobStatus.Items.Add("Completed");

            cmbFilterJobStatus.DropDownStyle =
                ComboBoxStyle.DropDownList;
            cmbFilterJobStatus.SelectedIndex = 0;

            dgvTransportJobs.ReadOnly = true;
            dgvTransportJobs.AllowUserToAddRows = false;
            dgvTransportJobs.AllowUserToDeleteRows = false;
            dgvTransportJobs.MultiSelect = false;
            dgvTransportJobs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvTransportJobs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            ClearSelectedJob();
            LoadTransportJobs();
        }

        private void LoadTransportJobs()
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string sql = @"
SELECT
    j.JobID,
    j.JobNumber,
    j.CustomerID,
    c.FullName AS Customer,
    j.RequestDate,
    j.RequestedTransportDate,
    j.PickupLocation,
    j.DestinationLocation,
    j.SpecialInstructions AS LoadDetails,
    j.Status,
    j.AdminRemarks
FROM dbo.TransportJobs AS j
LEFT JOIN dbo.Customers AS c
    ON j.CustomerID = c.CustomerID
WHERE
    (@Search = ''
     OR CONVERT(NVARCHAR(50), j.JobNumber) LIKE @Pattern
     OR CONVERT(NVARCHAR(50), j.CustomerID) LIKE @Pattern
     OR c.FullName LIKE @Pattern
     OR j.PickupLocation LIKE @Pattern
     OR j.DestinationLocation LIKE @Pattern)
    AND (@Status = 'All' OR j.Status = @Status)
ORDER BY j.RequestDate DESC, j.JobID DESC;";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        string search = txtSearchJobNo.Text.Trim();

                        string status =
                            cmbFilterJobStatus.SelectedItem == null
                            ? "All"
                            : cmbFilterJobStatus.SelectedItem.ToString();

                        cmd.Parameters.Add("@Search",
                            SqlDbType.NVarChar, 200).Value = search;

                        cmd.Parameters.Add("@Pattern",
                            SqlDbType.NVarChar, 202).Value =
                            "%" + search + "%";

                        cmd.Parameters.Add("@Status",
                            SqlDbType.NVarChar, 50).Value = status;

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            dgvTransportJobs.DataSource = table;
                        }
                    }
                }

                if (dgvTransportJobs.Columns.Contains("JobID"))
                    dgvTransportJobs.Columns["JobID"].Visible = false;

                if (dgvTransportJobs.Columns.Contains("CustomerID"))
                    dgvTransportJobs.Columns["CustomerID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load transport jobs.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSearchJobs_Click(object sender, EventArgs e)
        {
            LoadTransportJobs();

            MessageBox.Show(
                "Job search completed.",
                "Search",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnClearJobSearch_Click(object sender, EventArgs e)
        {
            txtSearchJobNo.Clear();

            if (cmbFilterJobStatus.Items.Count > 0)
                cmbFilterJobStatus.SelectedIndex = 0;

            ClearSelectedJob();
            LoadTransportJobs();

            MessageBox.Show(
                "Search and status filter cleared.",
                "Clear Search",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void dgvTransportJobs_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvTransportJobs.Rows[e.RowIndex];

            if (row.Cells["JobID"].Value == null)
                return;

            selectedJobID =
                Convert.ToInt32(row.Cells["JobID"].Value);

            lblValueJobNo.Text = CellText(row, "JobNumber");
            lblValueCustomer.Text = CellText(row, "Customer");
            lblValueRequestDate.Text = CellText(row, "RequestDate");
            lblValueRequestedTransportDate.Text =
                CellText(row, "RequestedTransportDate");
            lblValuePickupLocation.Text = CellText(row, "PickupLocation");
            lblValueDestination.Text =
                CellText(row, "DestinationLocation");
            lblValueLoadDetails.Text = CellText(row, "LoadDetails");
            lblValueJobStatus.Text = CellText(row, "Status");

            txtAdminRemarks.Text = CellText(row, "AdminRemarks");

            LoadAssignedTransportUnit();
        }

        private string CellText(DataGridViewRow row, string columnName)
        {
            if (!dgvTransportJobs.Columns.Contains(columnName))
                return "";

            object value = row.Cells[columnName].Value;

            return value == null || value == DBNull.Value
                ? ""
                : value.ToString();
        }

        private void ClearSelectedJob()
        {
            selectedJobID = 0;

            lblValueJobNo.Text = "-";
            lblValueCustomer.Text = "-";
            lblValueRequestDate.Text = "-";
            lblValueRequestedTransportDate.Text = "-";
            lblValuePickupLocation.Text = "-";
            lblValueDestination.Text = "-";
            lblValueLoadDetails.Text = "-";
            lblValueAssignedTransportUnit.Text = "Not assigned";
            lblValueJobStatus.Text = "-";

            txtAdminRemarks.Clear();
            dgvTransportJobs.ClearSelection();
        }

        private void LoadAssignedTransportUnit()
        {
            lblValueAssignedTransportUnit.Text = "Not assigned";

            if (selectedJobID == 0)
                return;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1 TransportUnitID
FROM dbo.JobAssignments
WHERE JobID = @JobID
ORDER BY JobAssignmentID DESC;", con))
                    {
                        cmd.Parameters.Add("@JobID", SqlDbType.Int)
                            .Value = selectedJobID;

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            lblValueAssignedTransportUnit.Text =
                                result.ToString();
                        }
                    }
                }
            }
            catch (SqlException)
            {
                lblValueAssignedTransportUnit.Text =
                    "Assignment not available";
            }
        }

        private void UpdateJobStatus(string newStatus)
        {
            if (selectedJobID == 0)
            {
                MessageBox.Show(
                    "Please select a job from the table first.",
                    "Select Job",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(@"
UPDATE dbo.TransportJobs
SET Status = @Status,
    AdminRemarks = @AdminRemarks
WHERE JobID = @JobID;", con))
                    {
                        cmd.Parameters.Add("@Status",
                            SqlDbType.NVarChar, 50).Value = newStatus;

                        cmd.Parameters.Add("@AdminRemarks",
                            SqlDbType.NVarChar).Value =
                            txtAdminRemarks.Text.Trim();

                        cmd.Parameters.Add("@JobID", SqlDbType.Int)
                            .Value = selectedJobID;

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            MessageBox.Show(
                                "The selected job could not be found.",
                                "Update Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Job status updated to " + newStatus + ".",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RefreshSelectedJob();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update the job.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAcceptJob_Click(object sender, EventArgs e)
        {
            UpdateJobStatus("Accepted");
        }

        private void btnDeclineJob_Click(object sender, EventArgs e)
        {
            if (selectedJobID == 0)
            {
                MessageBox.Show(
                    "Please select a job first.",
                    "Select Job",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "Are you sure you want to decline this transport request?",
                "Confirm Decline",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            UpdateJobStatus("Declined");
        }

        private void btnUpdateJob_Click(object sender, EventArgs e)
        {
            if (selectedJobID == 0)
            {
                MessageBox.Show(
                    "Please select a job first.",
                    "Select Job",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(@"
UPDATE dbo.TransportJobs
SET AdminRemarks = @AdminRemarks
WHERE JobID = @JobID;", con))
                    {
                        cmd.Parameters.Add("@AdminRemarks",
                            SqlDbType.NVarChar).Value =
                            txtAdminRemarks.Text.Trim();

                        cmd.Parameters.Add("@JobID", SqlDbType.Int)
                            .Value = selectedJobID;

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Admin remarks updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RefreshSelectedJob();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update admin remarks.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RefreshSelectedJob()
        {
            int jobID = selectedJobID;
            LoadTransportJobs();

            foreach (DataGridViewRow row in dgvTransportJobs.Rows)
            {
                if (row.Cells["JobID"].Value != null &&
                    Convert.ToInt32(row.Cells["JobID"].Value) == jobID)
                {
                    dgvTransportJobs_CellClick(
                        dgvTransportJobs,
                        new DataGridViewCellEventArgs(0, row.Index));
                    return;
                }
            }

            ClearSelectedJob();
        }

        // Standard Windows Forms dialog; no Microsoft.VisualBasic required.
        private string AskForTransportUnitID()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Assign Transport Unit";
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ClientSize = new Size(360, 130);

                Label prompt = new Label();
                prompt.Text =
                    "Enter the existing TransportUnitID:";
                prompt.AutoSize = true;
                prompt.Location = new Point(15, 15);

                TextBox input = new TextBox();
                input.Location = new Point(18, 42);
                input.Width = 320;

                Button okButton = new Button();
                okButton.Text = "OK";
                okButton.Location = new Point(175, 82);
                okButton.DialogResult = DialogResult.OK;

                Button cancelButton = new Button();
                cancelButton.Text = "Cancel";
                cancelButton.Location = new Point(260, 82);
                cancelButton.DialogResult = DialogResult.Cancel;

                dialog.Controls.Add(prompt);
                dialog.Controls.Add(input);
                dialog.Controls.Add(okButton);
                dialog.Controls.Add(cancelButton);

                dialog.AcceptButton = okButton;
                dialog.CancelButton = cancelButton;

                return dialog.ShowDialog(this) == DialogResult.OK
                    ? input.Text.Trim()
                    : "";
            }
        }

        private void btnAssignTransportUnit_Click(
            object sender, EventArgs e)
        {
            if (selectedJobID == 0)
            {
                MessageBox.Show(
                    "Please select a job first.",
                    "Select Job",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string unitIDText = AskForTransportUnitID();

            if (string.IsNullOrWhiteSpace(unitIDText))
                return;

            int transportUnitID;

            if (!int.TryParse(unitIDText, out transportUnitID))
            {
                MessageBox.Show(
                    "Please enter a valid numeric TransportUnitID.",
                    "Invalid Unit",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand check = new SqlCommand(@"
SELECT COUNT(*)
FROM dbo.TransportUnits
WHERE TransportUnitID = @UnitID;", con))
                    {
                        check.Parameters.Add("@UnitID", SqlDbType.Int)
                            .Value = transportUnitID;

                        if (Convert.ToInt32(check.ExecuteScalar()) == 0)
                        {
                            MessageBox.Show(
                                "That transport unit does not exist.",
                                "Unit Not Found",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    using (SqlCommand insert = new SqlCommand(@"
INSERT INTO dbo.JobAssignments (JobID, TransportUnitID)
VALUES (@JobID, @UnitID);", con))
                    {
                        insert.Parameters.Add("@JobID", SqlDbType.Int)
                            .Value = selectedJobID;

                        insert.Parameters.Add("@UnitID", SqlDbType.Int)
                            .Value = transportUnitID;

                        insert.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Transport unit assigned successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadAssignedTransportUnit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to assign the transport unit.\n\n" +
                    "Please verify the JobAssignments table columns.\n\n" +
                    ex.Message,
                    "Assignment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void OpenChildForm(Form child)
        {
            if (isLoggingOut || IsDisposed || Disposing)
            {
                child.Dispose();
                return;
            }

            Hide();

            try
            {
                using (child)
                {
                    child.StartPosition = FormStartPosition.CenterScreen;
                    child.ShowDialog();
                }
            }
            finally
            {
                if (!IsDisposed && !Disposing && !isLoggingOut)
                {
                    Show();
                    Activate();
                    LoadTransportJobs();
                }
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AdminDashboardForm());
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmCustomerManagement());
        }

        private void btnAdmins_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmAdminManagement());
        }

        private void btnJobs_Click(object sender, EventArgs e)
        {
            LoadTransportJobs();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmProductManagement());
        }

        private void btnTransportUnits_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmTransportUnitManagement());
        }

        private void btnManageJobAssignments_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "The Job Assignments management form has not been created yet.",
                "Job Assignments",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmReports());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            isLoggingOut = true;

            LoginForm login = new LoginForm();
            login.StartPosition = FormStartPosition.CenterScreen;
            login.Show();
            login.Activate();

            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                Form form = Application.OpenForms[i];

                if (form is AdminDashboardForm)
                    form.Close();
            }

            Close();
        }

        // Preserve existing Designer event handlers.
        private void lblDetailTransportDate_Click(object sender, EventArgs e)
        {
        }

        private void lblAssignedTransportUnit_Click(object sender, EventArgs e)
        {
        }

        private void lblJobNo_Click(object sender, EventArgs e)
        {
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

