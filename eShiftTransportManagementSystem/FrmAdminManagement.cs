
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace eShiftTransportManagementSystem
{
    public partial class FrmAdminManagement : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int selectedAdminID = 0;
        private bool isLoggingOut = false;

        public FrmAdminManagement()
        {
            InitializeComponent();

            btnAddCustomer.Click -= btnAddCustomer_Click;
            btnAddCustomer.Click += btnAddCustomer_Click;

            btnUpdateCustomer.Click -= btnUpdateCustomer_Click;
            btnUpdateCustomer.Click += btnUpdateCustomer_Click;

            btnDeleteCustomer.Click -= btnDeleteCustomer_Click;
            btnDeleteCustomer.Click += btnDeleteCustomer_Click;

            btnResetAdmin.Click -= btnResetAdmin_Click;
            btnResetAdmin.Click += btnResetAdmin_Click;

            btnClearCustomer.Click -= btnClearCustomer_Click;
            btnClearCustomer.Click += btnClearCustomer_Click;

            txtSearchAdmin.KeyDown -= txtSearchAdmin_KeyDown;
            txtSearchAdmin.KeyDown += txtSearchAdmin_KeyDown;

            dgvAdminList.CellClick -= dgvAdminList_CellClick;
            dgvAdminList.CellClick += dgvAdminList_CellClick;

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

            this.Load -= FrmAdminManagement_Load;
            this.Load += FrmAdminManagement_Load;
        }

        private void FrmAdminManagement_Load(object sender, EventArgs e)
        {
            txtAdminID.ReadOnly = true;
            txtAdminPassword.UseSystemPasswordChar = true;

            cmbAdminRole.DropDownStyle = ComboBoxStyle.DropDownList;

            if (cmbAdminRole.Items.Count == 0)
            {
                cmbAdminRole.Items.Add("Admin");
                cmbAdminRole.Items.Add("SuperAdmin");
            }

            dgvAdminList.ReadOnly = true;
            dgvAdminList.AllowUserToAddRows = false;
            dgvAdminList.AllowUserToDeleteRows = false;
            dgvAdminList.MultiSelect = false;
            dgvAdminList.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvAdminList.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LoadAdmins();
        }

        private void LoadAdmins(string search = "")
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string sql = @"
SELECT
    AdminID,
    FullName,
    Username,
    ContactNumber,
    Email,
    Role,
    IsActive
FROM dbo.Admins
WHERE
    @Search = ''
    OR CONVERT(NVARCHAR(50), AdminID) LIKE @Pattern
    OR FullName LIKE @Pattern
    OR Username LIKE @Pattern
    OR ContactNumber LIKE @Pattern
    OR Email LIKE @Pattern
    OR Role LIKE @Pattern
ORDER BY AdminID DESC;";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 200)
                            .Value = search;

                        cmd.Parameters.Add("@Pattern", SqlDbType.NVarChar, 202)
                            .Value = "%" + search + "%";

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            dgvAdminList.DataSource = table;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load admin records.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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

        private bool ValidateAdmin()
        {
            if (string.IsNullOrWhiteSpace(txtAdminFullName.Text) ||
                string.IsNullOrWhiteSpace(txtAdminUsername.Text) ||
                string.IsNullOrWhiteSpace(txtAdminContact.Text) ||
                string.IsNullOrWhiteSpace(txtAdminEmail.Text) ||
                cmbAdminRole.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please complete all admin details and select a role.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (!txtAdminEmail.Text.Contains("@"))
            {
                MessageBox.Show(
                    "Please enter a valid email address.",
                    "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (!ValidateAdmin())
                return;

            string password = txtAdminPassword.Text;

            if (string.IsNullOrWhiteSpace(password) ||
                password.Length < 8)
            {
                MessageBox.Show(
                    "Enter an admin password with at least 8 characters.",
                    "Password Required",
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
FROM dbo.Admins
WHERE Username = @Username;", con))
                    {
                        check.Parameters.AddWithValue(
                            "@Username", txtAdminUsername.Text.Trim());

                        if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show(
                                "This admin username already exists.",
                                "Duplicate Username",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }

                    string sql = @"
INSERT INTO dbo.Admins
(
    FullName,
    Username,
    PasswordHash,
    ContactNumber,
    Email,
    Role,
    CreatedAt,
    IsActive
)
VALUES
(
    @FullName,
    @Username,
    @PasswordHash,
    @ContactNumber,
    @Email,
    @Role,
    SYSDATETIME(),
    1
);";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        AddAdminParameters(cmd);

                        cmd.Parameters.Add(
                            "@PasswordHash", SqlDbType.NVarChar, 300)
                            .Value = HashPassword(password);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Admin added successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearAdminFields();
                LoadAdmins();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to add admin.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AddAdminParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue(
                "@FullName", txtAdminFullName.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@Username", txtAdminUsername.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@ContactNumber", txtAdminContact.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@Email", txtAdminEmail.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@Role", cmbAdminRole.SelectedItem.ToString());
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (selectedAdminID == 0)
            {
                MessageBox.Show(
                    "Select an admin from the table first.",
                    "Select Admin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!ValidateAdmin())
                return;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql;

                    if (string.IsNullOrWhiteSpace(txtAdminPassword.Text))
                    {
                        sql = @"
UPDATE dbo.Admins
SET FullName = @FullName,
    Username = @Username,
    ContactNumber = @ContactNumber,
    Email = @Email,
    Role = @Role
WHERE AdminID = @AdminID;";
                    }
                    else
                    {
                        if (txtAdminPassword.Text.Length < 8)
                        {
                            MessageBox.Show(
                                "The new password must contain at least 8 characters.",
                                "Invalid Password",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }

                        sql = @"
UPDATE dbo.Admins
SET FullName = @FullName,
    Username = @Username,
    ContactNumber = @ContactNumber,
    Email = @Email,
    Role = @Role,
    PasswordHash = @PasswordHash
WHERE AdminID = @AdminID;";
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        AddAdminParameters(cmd);

                        cmd.Parameters.Add("@AdminID", SqlDbType.Int)
                            .Value = selectedAdminID;

                        if (!string.IsNullOrWhiteSpace(txtAdminPassword.Text))
                        {
                            cmd.Parameters.Add(
                                "@PasswordHash", SqlDbType.NVarChar, 300)
                                .Value = HashPassword(txtAdminPassword.Text);
                        }

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            MessageBox.Show(
                                "Admin record not found.",
                                "Update",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Admin updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearAdminFields();
                LoadAdmins();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update admin.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (selectedAdminID == 0)
            {
                MessageBox.Show(
                    "Select an admin from the table first.",
                    "Select Admin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (MessageBox.Show(
                "Are you sure you want to delete this admin?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(@"
DELETE FROM dbo.Admins
WHERE AdminID = @AdminID;", con))
                    {
                        cmd.Parameters.Add("@AdminID", SqlDbType.Int)
                            .Value = selectedAdminID;

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Admin deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearAdminFields();
                LoadAdmins();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete admin.\n\n" + ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvAdminList_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvAdminList.Rows[e.RowIndex];

            if (row.Cells["AdminID"].Value == null)
                return;

            selectedAdminID =
                Convert.ToInt32(row.Cells["AdminID"].Value);

            txtAdminID.Text = selectedAdminID.ToString();
            txtAdminFullName.Text = CellText(row, "FullName");
            txtAdminUsername.Text = CellText(row, "Username");
            txtAdminContact.Text = CellText(row, "ContactNumber");
            txtAdminEmail.Text = CellText(row, "Email");

            string role = CellText(row, "Role");

            if (!cmbAdminRole.Items.Contains(role))
                cmbAdminRole.Items.Add(role);

            cmbAdminRole.SelectedItem = role;
            txtAdminPassword.Clear();
        }

        private string CellText(DataGridViewRow row, string column)
        {
            object value = row.Cells[column].Value;

            return value == null || value == DBNull.Value
                ? string.Empty
                : value.ToString();
        }

        private void btnResetAdmin_Click(object sender, EventArgs e)
        {
            ClearAdminFields();

            MessageBox.Show(
                "Admin details have been reset.",
                "Reset",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnClearCustomer_Click(object sender, EventArgs e)
        {
            txtSearchAdmin.Clear();
            LoadAdmins();

            MessageBox.Show(
                "Search cleared.",
                "Clear Search",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ClearAdminFields()
        {
            selectedAdminID = 0;

            txtAdminID.Clear();
            txtAdminFullName.Clear();
            txtAdminUsername.Clear();
            txtAdminPassword.Clear();
            txtAdminContact.Clear();
            txtAdminEmail.Clear();

            if (cmbAdminRole.Items.Count > 0)
                cmbAdminRole.SelectedIndex = -1;

            dgvAdminList.ClearSelection();
            txtAdminFullName.Focus();
        }

        private void txtSearchAdmin_KeyDown(
            object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadAdmins(txtSearchAdmin.Text.Trim());
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
                    LoadAdmins(txtSearchAdmin.Text.Trim());
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
            LoadAdmins();
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
            OpenChildForm(new FrmReports());
        }

        // Corrected logout: close the hidden Admin Dashboard too,
        // so it cannot reappear when this form closes.
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            isLoggingOut = true;

            // Create and show the login form first.
            LoginForm login = new LoginForm();
            login.StartPosition = FormStartPosition.CenterScreen;
            login.Show();
            login.Activate();

            // Close any hidden Admin Dashboard forms.
            // Iterate backwards because the collection changes as forms close.
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                Form form = Application.OpenForms[i];

                if (form is AdminDashboardForm)
                {
                    form.Close();
                }
            }

            // Close this Admin Management form.
            // OpenChildForm will not reopen it because isLoggingOut is true.
            Close();
        }
    }
}

