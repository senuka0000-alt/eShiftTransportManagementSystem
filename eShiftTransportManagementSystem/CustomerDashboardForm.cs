
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;
using System.Linq;

namespace eShiftTransportManagementSystem
{
    public partial class CustomerDashboardForm : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int loggedInCustomerID;

        public CustomerDashboardForm(int customerID)
        {
            InitializeComponent();

            loggedInCustomerID = customerID;

            // Connect sidebar buttons
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

            Load -= CustomerDashboardForm_Load;
            Load += CustomerDashboardForm_Load;
        }

        // ==========================================
        // FORM LOAD
        // ==========================================

        private void CustomerDashboardForm_Load(object sender, EventArgs e)
        {
            LoadCustomerDashboard();
        }

        // ==========================================
        // LOAD CUSTOMER DASHBOARD
        // ==========================================

        private void LoadCustomerDashboard()
        {
            LoadCustomerSummary();
            LoadRecentTransportJobs();
        }

        // ==========================================
        // LOAD CUSTOMER SUMMARY CARDS
        // ==========================================

        private void LoadCustomerSummary()
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
SELECT
    COUNT(*) AS TotalJobs,

    SUM(CASE
        WHEN Status = 'Pending' THEN 1
        ELSE 0
    END) AS PendingRequests,

    SUM(CASE
        WHEN Status = 'Accepted' THEN 1
        ELSE 0
    END) AS ApprovedJobs,

    SUM(CASE
        WHEN Status = 'Completed' THEN 1
        ELSE 0
    END) AS CompletedJobs

FROM dbo.TransportJobs
WHERE CustomerID = @CustomerID;";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@CustomerID", loggedInCustomerID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load customer summary.\n\n" + ex.Message,
                    "Dashboard Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // LOAD RECENT TRANSPORT JOBS
        // ==========================================

        private void LoadRecentTransportJobs()
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query = @"
SELECT TOP 10
    JobID,
    JobNumber,
    RequestDate,
    RequestedTransportDate,
    PickupLocation,
    DestinationLocation,
    Status
FROM dbo.TransportJobs
WHERE CustomerID = @CustomerID
ORDER BY RequestDate DESC, JobID DESC;";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@CustomerID", loggedInCustomerID);

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();

                            adapter.Fill(table);

                            dgvRecentTransportJobs.DataSource = table;

                            FormatTransportJobsGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load recent transport jobs.\n\n" +
                    ex.Message,
                    "Dashboard Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // FORMAT TRANSPORT JOBS TABLE
        // ==========================================

        private void FormatTransportJobsGrid()
        {
            dgvRecentTransportJobs.ReadOnly = true;

            dgvRecentTransportJobs.AllowUserToAddRows = false;

            dgvRecentTransportJobs.AllowUserToDeleteRows = false;

            dgvRecentTransportJobs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvRecentTransportJobs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvRecentTransportJobs.MultiSelect = false;

            dgvRecentTransportJobs.RowHeadersVisible = false;
        }

        // ==========================================
        // FIND REQUEST TRANSPORT FORM
        // ==========================================

        private Type FindTransportRequestForm()
        {
            Assembly assembly =
                typeof(CustomerDashboardForm).Assembly;

            Type[] formTypes = assembly.GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    typeof(Form).IsAssignableFrom(t))
                .ToArray();

            string[] possibleNames =
            {
                "FrmTransportRequest",
                "RequestTransportForm",
                "FrmRequestTransport",
                "FrmCreateTransportRequest",
                "CreateTransportRequestForm"
            };

            foreach (string name in possibleNames)
            {
                Type exactMatch = formTypes.FirstOrDefault(
                    t => t.Name.Equals(
                        name,
                        StringComparison.OrdinalIgnoreCase));

                if (exactMatch != null)
                    return exactMatch;
            }

            Type matchingForm = formTypes.FirstOrDefault(t =>
            {
                string name = t.Name.ToLowerInvariant();

                return name.Contains("transportrequest") ||
                       name.Contains("requesttransport");
            });

            return matchingForm;
        }

        // ==========================================
        // OPEN CUSTOMER PAGE
        // ==========================================

        private void OpenCustomerPage(Type formType)
        {
            if (formType == null || IsDisposed || Disposing)
                return;

            try
            {
                Form child = null;

                ConstructorInfo customerConstructor =
                    formType.GetConstructor(
                        new Type[] { typeof(int) });

                if (customerConstructor != null)
                {
                    child = (Form)customerConstructor.Invoke(
                        new object[] { loggedInCustomerID });
                }
                else
                {
                    ConstructorInfo defaultConstructor =
                        formType.GetConstructor(Type.EmptyTypes);

                    if (defaultConstructor != null)
                    {
                        child = (Form)defaultConstructor.Invoke(null);
                    }
                    else
                    {
                        throw new Exception(
                            "The selected page does not have a supported constructor.");
                    }
                }

                child.StartPosition =
                    FormStartPosition.CenterScreen;

                Hide();

                try
                {
                    using (child)
                    {
                        child.ShowDialog(this);
                    }
                }
                finally
                {
                    if (!IsDisposed && !Disposing)
                    {
                        Show();
                        Activate();
                        LoadCustomerDashboard();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to open the selected page.\n\n" +
                    ex.Message,
                    "Navigation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // DASHBOARD BUTTON
        // ==========================================

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadCustomerDashboard();
        }

        // ==========================================
        // CREATE TRANSPORT REQUEST
        // ==========================================

        private void btnCreateRequest_Click(object sender, EventArgs e)
        {
            Type requestForm = FindTransportRequestForm();

            if (requestForm == null)
            {
                MessageBox.Show(
                    "Unable to find the Request Transport form.\n\n" +
                    "Please check the actual class name of your form.",
                    "Page Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            OpenCustomerPage(requestForm);
        }

        // ==========================================
        // MY JOBS BUTTON
        // ==========================================

        private void btnMyJobs_Click(object sender, EventArgs e)
        {
            OpenCustomerPage(typeof(FrmMyTransportJobs));
        }

        // ==========================================
        // CUSTOMER PROFILE BUTTON
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
        // CUSTOMER PAYMENTS BUTTON
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

            this.Close();
        }

        // ==========================================
        // DASHBOARD PANEL PAINT
        // ==========================================

        private void PnlDashboard_Paint(
            object sender, PaintEventArgs e)
        {
            // Keep this event for the existing Designer connection.
        }
    }
}

