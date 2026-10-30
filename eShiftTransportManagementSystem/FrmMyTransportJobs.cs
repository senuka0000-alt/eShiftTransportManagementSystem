
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;
using System.Linq;

namespace eShiftTransportManagementSystem
{
    public partial class FrmMyTransportJobs : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int loggedInCustomerID;
        private int selectedJobID = 0;

        // ==========================================
        // CONSTRUCTORS
        // ==========================================

        public FrmMyTransportJobs()
        {
            InitializeComponent();
            ConnectButtons();
        }

        public FrmMyTransportJobs(int customerID)
        {
            InitializeComponent();
            loggedInCustomerID = customerID;
            ConnectButtons();
        }

        // ==========================================
        // CONNECT BUTTONS
        // ==========================================

        private void ConnectButtons()
        {
            btnRefreshJobs.Click -= btnRefreshJobs_Click;
            btnRefreshJobs.Click += btnRefreshJobs_Click;

            btnViewDetails.Click -= btnViewDetails_Click;
            btnViewDetails.Click += btnViewDetails_Click;

            btnCancelRequest.Click -= btnCancelRequest_Click;
            btnCancelRequest.Click += btnCancelRequest_Click;

            btnDashboard.Click -= btnDashboard_Click;
            btnDashboard.Click += btnDashboard_Click;

            btnCreateRequest.Click -= btnCreateRequest_Click;
            btnCreateRequest.Click += btnCreateRequest_Click;

            btnMyJobs.Click -= btnMyJobs_Click;
            btnMyJobs.Click += btnMyJobs_Click;

            btnProfile.Click -= btnProfile_Click;
            btnProfile.Click += btnProfile_Click;

            btnMyPayments.Click -= btnMyPayments_Click;
            btnMyPayments.Click += btnMyPayments_Click;

            btnCustomerLogout.Click -= btnCustomerLogout_Click;
            btnCustomerLogout.Click += btnCustomerLogout_Click;

            Load -= FrmMyTransportJobs_Load;
            Load += FrmMyTransportJobs_Load;

            dgvMyTransportJobs.SelectionChanged -=
                dgvMyTransportJobs_SelectionChanged;

            dgvMyTransportJobs.SelectionChanged +=
                dgvMyTransportJobs_SelectionChanged;

            txtSearchJobNo.TextChanged -= txtSearchJobNo_TextChanged;
            txtSearchJobNo.TextChanged += txtSearchJobNo_TextChanged;

            cmbFilterStatus.SelectedIndexChanged -=
                cmbFilterStatus_SelectedIndexChanged;

            cmbFilterStatus.SelectedIndexChanged +=
                cmbFilterStatus_SelectedIndexChanged;
        }

        // ==========================================
        // FORM LOAD
        // ==========================================

        private void FrmMyTransportJobs_Load(object sender, EventArgs e)
        {
            cmbFilterStatus.Items.Clear();

            cmbFilterStatus.Items.Add("All");
            cmbFilterStatus.Items.Add("Pending");
            cmbFilterStatus.Items.Add("Accepted");
            cmbFilterStatus.Items.Add("Declined");
            cmbFilterStatus.Items.Add("Completed");

            cmbFilterStatus.SelectedIndex = 0;

            LoadMyTransportJobs();
        }

        // ==========================================
        // LOAD MY TRANSPORT JOBS
        // ==========================================

        private void LoadMyTransportJobs()
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query = @"
SELECT
    j.JobID,
    j.JobNumber,
    c.CustomerNumber,
    j.CustomerID,
    j.PickupLocation,
    j.DestinationLocation,
    j.RequestDate,
    j.RequestedTransportDate,
    j.Status,
    j.AdminRemarks,
    (SELECT COUNT(*)
     FROM dbo.Loads l
     WHERE l.JobID = j.JobID) AS TotalLoads
FROM dbo.TransportJobs j
INNER JOIN dbo.Customers c
    ON j.CustomerID = c.CustomerID
WHERE j.CustomerID = @CustomerID
AND (@JobNumber = '' OR j.JobNumber LIKE '%' + @JobNumber + '%')
AND (@Status = 'All' OR j.Status = @Status)
ORDER BY j.RequestDate DESC, j.JobID DESC;";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@CustomerID", loggedInCustomerID);

                        cmd.Parameters.AddWithValue(
                            "@JobNumber", txtSearchJobNo.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Status",
                            cmbFilterStatus.SelectedItem == null
                                ? "All"
                                : cmbFilterStatus.SelectedItem.ToString());

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();

                            adapter.Fill(table);

                            dgvMyTransportJobs.DataSource = table;

                            FormatJobsGrid();

                            ClearSelectedJobDetails();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load your transport jobs.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // FORMAT DATA GRID VIEW
        // ==========================================

        private void FormatJobsGrid()
        {
            dgvMyTransportJobs.ReadOnly = true;

            dgvMyTransportJobs.AllowUserToAddRows = false;

            dgvMyTransportJobs.AllowUserToDeleteRows = false;

            dgvMyTransportJobs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvMyTransportJobs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvMyTransportJobs.MultiSelect = false;

            dgvMyTransportJobs.RowHeadersVisible = false;

            if (dgvMyTransportJobs.Columns.Contains("JobID"))
                dgvMyTransportJobs.Columns["JobID"].Visible = false;

            if (dgvMyTransportJobs.Columns.Contains("CustomerID"))
                dgvMyTransportJobs.Columns["CustomerID"].Visible = false;

            if (dgvMyTransportJobs.Columns.Contains("CustomerNumber"))
                dgvMyTransportJobs.Columns["CustomerNumber"].HeaderText =
                    "Customer No";

            if (dgvMyTransportJobs.Columns.Contains("JobNumber"))
                dgvMyTransportJobs.Columns["JobNumber"].HeaderText =
                    "Job No";

            if (dgvMyTransportJobs.Columns.Contains("TotalLoads"))
                dgvMyTransportJobs.Columns["TotalLoads"].HeaderText =
                    "Total Loads";
        }

        // ==========================================
        // SELECTED JOB DETAILS
        // ==========================================

        private void dgvMyTransportJobs_SelectionChanged(
            object sender, EventArgs e)
        {
            if (dgvMyTransportJobs.SelectedRows.Count == 0)
            {
                ClearSelectedJobDetails();
                return;
            }

            DataGridViewRow row =
                dgvMyTransportJobs.SelectedRows[0];

            if (row.Cells["JobID"].Value == null)
                return;

            selectedJobID =
                Convert.ToInt32(row.Cells["JobID"].Value);

            lblValueJobNo.Text =
                row.Cells["JobNumber"].Value.ToString();

            lblValueCustomerNo.Text =
                row.Cells["CustomerNumber"].Value.ToString();

            lblValuePickup.Text =
                row.Cells["PickupLocation"].Value.ToString();

            lblValueDestination.Text =
                row.Cells["DestinationLocation"].Value.ToString();

            lblValueRequestDate.Text =
                Convert.ToDateTime(
                    row.Cells["RequestDate"].Value).ToString("yyyy-MM-dd");

            lblValueTransportDate.Text =
                Convert.ToDateTime(
                    row.Cells["RequestedTransportDate"].Value)
                    .ToString("yyyy-MM-dd");

            lblValueStatus.Text =
                row.Cells["Status"].Value.ToString();

            lblValueLoads.Text =
                row.Cells["TotalLoads"].Value.ToString();

            lblValueAssignedUnit.Text =
                "Check assignment details";

            lblValueAdminRemarks.Text =
                row.Cells["AdminRemarks"].Value == DBNull.Value
                    ? "No remarks"
                    : row.Cells["AdminRemarks"].Value.ToString();
        }

        // ==========================================
        // CLEAR SELECTED JOB DETAILS
        // ==========================================

        private void ClearSelectedJobDetails()
        {
            selectedJobID = 0;

            lblValueJobNo.Text = "-";
            lblValueCustomerNo.Text = "-";
            lblValuePickup.Text = "-";
            lblValueDestination.Text = "-";
            lblValueRequestDate.Text = "-";
            lblValueTransportDate.Text = "-";
            lblValueStatus.Text = "-";
            lblValueLoads.Text = "-";
            lblValueAssignedUnit.Text = "-";
            lblValueAdminRemarks.Text = "-";
        }

        // ==========================================
        // SEARCH JOB NUMBER
        // ==========================================

        private void txtSearchJobNo_TextChanged(
            object sender, EventArgs e)
        {
            LoadMyTransportJobs();
        }

        // ==========================================
        // FILTER BY STATUS
        // ==========================================

        private void cmbFilterStatus_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            LoadMyTransportJobs();
        }

        // ==========================================
        // REFRESH JOBS
        // ==========================================

        private void btnRefreshJobs_Click(object sender, EventArgs e)
        {
            txtSearchJobNo.Clear();

            cmbFilterStatus.SelectedIndex = 0;

            LoadMyTransportJobs();

            MessageBox.Show(
                "Your transport jobs have been refreshed successfully.",
                "Refresh Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==========================================
        // VIEW JOB DETAILS
        // ==========================================

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (selectedJobID == 0)
            {
                MessageBox.Show(
                    "Please select a transport job first.",
                    "Select Job",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            MessageBox.Show(
                "Job Number: " + lblValueJobNo.Text +
                "\nCustomer Number: " + lblValueCustomerNo.Text +
                "\nPickup: " + lblValuePickup.Text +
                "\nDestination: " + lblValueDestination.Text +
                "\nRequest Date: " + lblValueRequestDate.Text +
                "\nTransport Date: " + lblValueTransportDate.Text +
                "\nStatus: " + lblValueStatus.Text +
                "\nTotal Loads: " + lblValueLoads.Text +
                "\nAssigned Unit: " + lblValueAssignedUnit.Text +
                "\nAdmin Remarks: " + lblValueAdminRemarks.Text,
                "Transport Job Details",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==========================================
        // CANCEL TRANSPORT REQUEST
        // ==========================================

        private void btnCancelRequest_Click(object sender, EventArgs e)
        {
            if (selectedJobID == 0)
            {
                MessageBox.Show(
                    "Please select a transport job first.",
                    "Select Job",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!lblValueStatus.Text.Equals(
                "Pending", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Only pending transport requests can be cancelled.",
                    "Cannot Cancel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel this transport request?",
                "Confirm Cancellation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
UPDATE dbo.TransportJobs
SET Status = 'Cancelled'
WHERE JobID = @JobID
AND CustomerID = @CustomerID
AND Status = 'Pending';";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@JobID", selectedJobID);

                        cmd.Parameters.AddWithValue(
                            "@CustomerID", loggedInCustomerID);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show(
                                "Your transport request has been cancelled successfully.",
                                "Cancellation Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            LoadMyTransportJobs();
                        }
                        else
                        {
                            MessageBox.Show(
                                "The request could not be cancelled. Please refresh and try again.",
                                "Cancellation Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to cancel transport request.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // DASHBOARD BUTTON
        // ==========================================

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Form dashboard = Owner;

            if (dashboard == null || dashboard.IsDisposed)
            {
                dashboard = Application.OpenForms
                    .Cast<Form>()
                    .FirstOrDefault(f =>
                        f.GetType().Name == "CustomerDashboardForm" ||
                        f.GetType().Name == "FrmCustomerDashboard");
            }

            if (dashboard != null && !dashboard.IsDisposed)
            {
                dashboard.Show();
                dashboard.Activate();

                Close();
            }
            else
            {
                MessageBox.Show(
                    "Customer Dashboard is not currently open.",
                    "Navigation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // ==========================================
        // CREATE REQUEST BUTTON
        // ==========================================

        private void btnCreateRequest_Click(object sender, EventArgs e)
        {
            OpenCustomerPage("RequestTransportForm");
        }

        // ==========================================
        // MY JOBS BUTTON
        // ==========================================

        private void btnMyJobs_Click(object sender, EventArgs e)
        {
            LoadMyTransportJobs();
        }

        // ==========================================
        // OPEN CUSTOMER PAGE
        // ==========================================

        private void OpenCustomerPage(string formName)
        {
            try
            {
                Type formType =
                    typeof(FrmMyTransportJobs).Assembly.GetTypes()
                    .FirstOrDefault(t =>
                        t.Name.Equals(
                            formName,
                            StringComparison.OrdinalIgnoreCase));

                if (formType == null)
                {
                    MessageBox.Show(
                        "The selected page was not found: " + formName,
                        "Page Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Form child = null;

                ConstructorInfo customerConstructor =
                    formType.GetConstructor(new Type[] { typeof(int) });

                if (customerConstructor != null)
                {
                    child = (Form)customerConstructor.Invoke(
                        new object[] { loggedInCustomerID });
                }
                else
                {
                    child = (Form)Activator.CreateInstance(formType);
                }

                child.StartPosition = FormStartPosition.CenterScreen;

                Hide();

                using (child)
                {
                    child.ShowDialog(this);
                }

                if (!IsDisposed && !Disposing)
                {
                    Show();
                    Activate();
                    LoadMyTransportJobs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to open the selected page.\n\n" + ex.Message,
                    "Navigation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // PROFILE BUTTON
        // ==========================================

        private void btnProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "The Customer Profile page will be connected next.",
                "My Profile",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==========================================
        // PAYMENTS BUTTON
        // ==========================================

        private void btnMyPayments_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "The My Payments page will be connected next.",
                "My Payments",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==========================================
        // CUSTOMER LOGOUT
        // ==========================================

        private void btnCustomerLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                Form dashboard = Owner;

                if (dashboard == null || dashboard.IsDisposed)
                {
                    dashboard = Application.OpenForms
                        .Cast<Form>()
                        .FirstOrDefault(f =>
                            f.GetType().Name == "CustomerDashboardForm" ||
                            f.GetType().Name == "FrmCustomerDashboard");
                }

                Form login = Application.OpenForms
                    .Cast<Form>()
                    .FirstOrDefault(f =>
                        f.GetType().Name == "LoginForm");

                // Close My Transport Jobs page.
                Close();

                // Close Customer Dashboard.
                if (dashboard != null && !dashboard.IsDisposed)
                {
                    dashboard.Close();
                }

                // Return to Login Form.
                if (login != null && !login.IsDisposed)
                {
                    login.Show();
                    login.Activate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to logout.\n\n" + ex.Message,
                    "Logout Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void pnlSelectedJobDetails_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

