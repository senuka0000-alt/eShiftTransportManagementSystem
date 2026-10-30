
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace eShiftTransportManagementSystem
{
    public partial class AdminDashboardForm : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private bool IsLoggingOut = false;

        // ADDED: Allows child forms to check logout status.
        public bool IsLoggingOutRequested
        {
            get { return IsLoggingOut; }
        }

        public AdminDashboardForm()
        {
            InitializeComponent();

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

            this.Load -= AdminDashboardForm_Load;
            this.Load += AdminDashboardForm_Load;
        }

        // Allows child forms to request logout.
        public void RequestLogout()
        {
            IsLoggingOut = true;
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    string countQuery = @"
SELECT
    (SELECT COUNT(*)
     FROM dbo.Customers
     WHERE IsActive = 1) AS TotalCustomers,

    (SELECT COUNT(*)
     FROM dbo.TransportJobs) AS TotalJobs,

    (SELECT COUNT(*)
     FROM dbo.TransportJobs
     WHERE Status = 'Pending') AS PendingRequests,

    (SELECT COUNT(*)
     FROM dbo.TransportJobs
     WHERE Status = 'Accepted') AS ApprovedJobs,

    (SELECT COUNT(*)
     FROM dbo.TransportJobs
     WHERE Status = 'Completed') AS CompletedJobs;";

                    using (SqlCommand cmd =
                        new SqlCommand(countQuery, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTotalCustomers.Text =
                                reader["TotalCustomers"].ToString();

                            lblTotalJobs.Text =
                                reader["TotalJobs"].ToString();

                            lblPendingRequests.Text =
                                reader["PendingRequests"].ToString();

                            lblApprovedJobs.Text =
                                reader["ApprovedJobs"].ToString();

                            lblCompletedJobs.Text =
                                reader["CompletedJobs"].ToString();
                        }
                    }

                    string requestsQuery = @"
SELECT TOP 10
    j.JobID,
    j.JobNumber,
    c.FullName AS Customer,
    j.RequestDate,
    j.RequestedTransportDate,
    j.PickupLocation,
    j.DestinationLocation,
    j.Status
FROM dbo.TransportJobs AS j
LEFT JOIN dbo.Customers AS c
    ON j.CustomerID = c.CustomerID
ORDER BY j.RequestDate DESC, j.JobID DESC;";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(requestsQuery, con))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvRecentRequests.DataSource = table;
                        dgvRecentRequests.ReadOnly = true;
                        dgvRecentRequests.AllowUserToAddRows = false;
                        dgvRecentRequests.AllowUserToDeleteRows = false;
                        dgvRecentRequests.AutoSizeColumnsMode =
                            DataGridViewAutoSizeColumnsMode.Fill;
                        dgvRecentRequests.SelectionMode =
                            DataGridViewSelectionMode.FullRowSelect;
                        dgvRecentRequests.MultiSelect = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Admin Dashboard data.\n\n" +
                    ex.Message,
                    "Dashboard Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadDashboardData();

            MessageBox.Show(
                "Dashboard data refreshed successfully.",
                "Dashboard",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
            OpenChildForm(new FrmJobManagement());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmProductManagement());
        }

        private void btnTransportUnits_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmTransportUnitManagement());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmReports());
        }

        private void btnManageJobAssignments_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "The Job Assignments page has not been connected yet.",
                "Job Assignments",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // Open a child form and safely return to this dashboard.
        private void OpenChildForm(Form child)
        {
            if (child == null || this.IsDisposed || this.Disposing)
                return;

            this.Hide();

            try
            {
                child.StartPosition = FormStartPosition.CenterScreen;

                using (child)
                {
                    child.ShowDialog();
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
            finally
            {
                if (IsLoggingOut)
                {
                    ReturnToLogin();
                }
                else if (!this.IsDisposed && !this.Disposing)
                {
                    this.Show();
                    this.Activate();
                    LoadDashboardData();
                }
            }
        }

        // Return to the original Login Form if it is still open.
        private void ReturnToLogin()
        {
            LoginForm login = null;

            foreach (Form form in Application.OpenForms)
            {
                if (form is LoginForm && !form.IsDisposed)
                {
                    login = (LoginForm)form;
                    break;
                }
            }

            // Create a Login Form only if the original is unavailable.
            if (login == null)
            {
                login = new LoginForm();
            }

            login.StartPosition = FormStartPosition.CenterScreen;
            login.WindowState = FormWindowState.Normal;

            // Display Login before closing the dashboard.
            login.Show();
            login.BringToFront();
            login.Activate();

            this.Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            IsLoggingOut = true;

            ReturnToLogin();
        }

        private void PnlDashboard_Paint(
            object sender, PaintEventArgs e)
        {
            // Keep this event for the existing Designer connection.
        }
    }
}


