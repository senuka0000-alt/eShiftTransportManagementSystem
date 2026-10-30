
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace eShiftTransportManagementSystem
{
    public partial class FrmCustomerManagement : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int selectedCustomerId = 0;

        public FrmCustomerManagement()
        {
            InitializeComponent();

            // Prevent duplicate event subscriptions.
            this.Load -= FrmCustomerManagement_Load;
            this.Load += FrmCustomerManagement_Load;

            btnSearchCustomer.Click -= btnSearchCustomer_Click;
            btnSearchCustomer.Click += btnSearchCustomer_Click;

            btnClearSearch.Click -= btnClearSearch_Click;
            btnClearSearch.Click += btnClearSearch_Click;

            btnAddCustomer.Click -= btnAddCustomer_Click;
            btnAddCustomer.Click += btnAddCustomer_Click;

            btnUpdateCustomer.Click -= btnUpdateCustomer_Click;
            btnUpdateCustomer.Click += btnUpdateCustomer_Click;

            btnDeleteCustomer.Click -= btnDeleteCustomer_Click;
            btnDeleteCustomer.Click += btnDeleteCustomer_Click;

            btnClearCustomer.Click -= btnClearCustomer_Click;
            btnClearCustomer.Click += btnClearCustomer_Click;

            dgvCustomers.CellClick -= dgvCustomers_CellClick;
            dgvCustomers.CellClick += dgvCustomers_CellClick;

            // Sidebar buttons.
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

        private void FrmCustomerManagement_Load(object sender, EventArgs e)
        {
            txtCustomerNumber.ReadOnly = true;

            dgvCustomers.ReadOnly = true;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LoadCustomers();
        }

        // Display real customer records.
        private void LoadCustomers(string searchText = "")
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string sql = @"
SELECT
    CustomerID,
    CustomerNumber,
    FullName,
    Address,
    Telephone,
    Email,
    Username,
    IsActive
FROM dbo.Customers
WHERE
    @Search = ''
    OR CustomerNumber LIKE @Pattern
    OR FullName LIKE @Pattern
    OR Telephone LIKE @Pattern
    OR Email LIKE @Pattern
    OR Username LIKE @Pattern
ORDER BY CustomerID DESC;";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 200)
                            .Value = searchText;

                        cmd.Parameters.Add("@Pattern", SqlDbType.NVarChar, 202)
                            .Value = "%" + searchText + "%";

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            dgvCustomers.DataSource = table;
                        }
                    }
                }

                if (dgvCustomers.Columns.Contains("CustomerID"))
                    dgvCustomers.Columns["CustomerID"].Visible = false;

                if (dgvCustomers.Columns.Contains("IsActive"))
                    dgvCustomers.Columns["IsActive"].HeaderText = "Active";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load customer records.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            LoadCustomers(txtSearchJobNo.Text.Trim());

            MessageBox.Show(
                dgvCustomers.Rows.Count + " matching customer record(s) found.",
                "Search Results",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchJobNo.Clear();
            LoadCustomers();

            MessageBox.Show(
                "Search cleared.",
                "Search",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // Generate a temporary password and store it using the
        // same PBKDF2 format used by LoginForm.
        private string GenerateTemporaryPassword()
        {
            byte[] bytes = new byte[12];

            using (RandomNumberGenerator rng =
                RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return "Temp@" + Convert.ToBase64String(bytes)
                .Replace("+", "A")
                .Replace("/", "B")
                .Replace("=", "C");
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];

            using (RandomNumberGenerator rng =
                RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            const int iterations = 100000;
            byte[] hash;

            using (var pbkdf2 =
                new Rfc2898DeriveBytes(password, salt, iterations))
            {
                hash = pbkdf2.GetBytes(32);
            }

            return "PBKDF2$" + iterations + "$" +
                   Convert.ToBase64String(salt) + "$" +
                   Convert.ToBase64String(hash);
        }

        private bool ValidateCustomerDetails()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerFullName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerAddress.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerTelephone.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerUsername.Text))
            {
                MessageBox.Show(
                    "Please complete all customer details.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (!ValidateCustomerDetails())
                return;

            string temporaryPassword = GenerateTemporaryPassword();

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    // CustomerNumber is computed by SQL Server,
                    // so it is deliberately excluded from INSERT.
                    string sql = @"
INSERT INTO dbo.Customers
(
    FullName,
    Address,
    Telephone,
    Email,
    Username,
    PasswordHash,
    CreatedAt,
    IsActive
)
VALUES
(
    @FullName,
    @Address,
    @Telephone,
    @Email,
    @Username,
    @PasswordHash,
    SYSDATETIME(),
    1
);";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        AddCustomerParameters(cmd);

                        cmd.Parameters.Add("@PasswordHash",
                            SqlDbType.NVarChar, -1).Value =
                            HashPassword(temporaryPassword);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Customer added successfully.\n\n" +
                    "Temporary password: " + temporaryPassword +
                    "\n\nSave this password and share it securely " +
                    "with the customer. It will not be shown again.",
                    "Customer Added",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearCustomerFields();
                LoadCustomers();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Unable to add customer.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AddCustomerParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar)
                .Value = txtCustomerFullName.Text.Trim();

            cmd.Parameters.Add("@Address", SqlDbType.NVarChar)
                .Value = txtCustomerAddress.Text.Trim();

            cmd.Parameters.Add("@Telephone", SqlDbType.NVarChar)
                .Value = txtCustomerTelephone.Text.Trim();

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar)
                .Value = txtCustomerEmail.Text.Trim();

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar)
                .Value = txtCustomerUsername.Text.Trim();
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0)
            {
                MessageBox.Show(
                    "Select a customer from the table first.",
                    "Select Customer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateCustomerDetails())
                return;

            DialogResult answer = MessageBox.Show(
                "Update the selected customer?",
                "Confirm Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = @"
UPDATE dbo.Customers
SET
    FullName = @FullName,
    Address = @Address,
    Telephone = @Telephone,
    Email = @Email,
    Username = @Username
WHERE CustomerID = @CustomerID;";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        AddCustomerParameters(cmd);

                        cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                            .Value = selectedCustomerId;

                        int affected = cmd.ExecuteNonQuery();

                        if (affected == 0)
                        {
                            MessageBox.Show(
                                "The selected customer was not found.",
                                "Update",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Customer updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearCustomerFields();
                LoadCustomers();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Unable to update customer.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0)
            {
                MessageBox.Show(
                    "Select a customer from the table first.",
                    "Select Customer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult answer = MessageBox.Show(
                "Permanently delete this customer?\n\n" +
                "This may fail if transport jobs reference this customer.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (answer != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = @"
DELETE FROM dbo.Customers
WHERE CustomerID = @CustomerID;";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                            .Value = selectedCustomerId;

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Customer deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearCustomerFields();
                LoadCustomers();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Unable to delete this customer. Existing transport " +
                    "jobs may reference the customer.\n\n" + ex.Message,
                    "Delete Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvCustomers_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];

            if (row.Cells["CustomerID"].Value == null)
                return;

            selectedCustomerId =
                Convert.ToInt32(row.Cells["CustomerID"].Value);

            txtCustomerNumber.Text =
                GetCellText(row, "CustomerNumber");

            txtCustomerFullName.Text =
                GetCellText(row, "FullName");

            txtCustomerAddress.Text =
                GetCellText(row, "Address");

            txtCustomerTelephone.Text =
                GetCellText(row, "Telephone");

            txtCustomerEmail.Text =
                GetCellText(row, "Email");

            txtCustomerUsername.Text =
                GetCellText(row, "Username");
        }

        private string GetCellText(DataGridViewRow row, string columnName)
        {
            object value = row.Cells[columnName].Value;

            return value == null || value == DBNull.Value
                ? string.Empty
                : value.ToString();
        }

        private void btnClearCustomer_Click(object sender, EventArgs e)
        {
            ClearCustomerFields();

            MessageBox.Show(
                "Customer details cleared.",
                "Clear",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ClearCustomerFields()
        {
            selectedCustomerId = 0;

            txtCustomerNumber.Clear();
            txtCustomerFullName.Clear();
            txtCustomerAddress.Clear();
            txtCustomerTelephone.Clear();
            txtCustomerEmail.Clear();
            txtCustomerUsername.Clear();

            dgvCustomers.ClearSelection();
            txtCustomerFullName.Focus();
        }

        // Sidebar navigation.
        private void OpenPage(Form child)
        {
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
                if (!IsDisposed)
                {
                    Show();
                    Activate();
                    LoadCustomers(txtSearchJobNo.Text.Trim());
                }
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            OpenPage(new AdminDashboardForm());
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            MessageBox.Show(
                "Customer list refreshed.",
                "Customers",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnAdmins_Click(object sender, EventArgs e)
        {
            OpenPage(new FrmAdminManagement());
        }

        private void btnJobs_Click(object sender, EventArgs e)
        {
            OpenPage(new FrmJobManagement());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            OpenPage(new FrmProductManagement());
        }

        private void btnTransportUnits_Click(object sender, EventArgs e)
        {
            OpenPage(new FrmTransportUnitManagement());
        }

        private void btnManageJobAssignments_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "The Job Assignments form has not been connected yet.",
                "Job Assignments",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            OpenPage(new FrmReports());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            LoginForm login = new LoginForm();
            login.StartPosition = FormStartPosition.CenterScreen;
            login.Show();

            // Close the dashboard as well so it cannot reappear.
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminDashboardForm)
                {
                    form.Close();
                    break;
                }
            }

            Close();
        }

        // Keep existing Designer event handlers.
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblWelcomeDescription_Click(object sender, EventArgs e)
        {
        }
    }
}
