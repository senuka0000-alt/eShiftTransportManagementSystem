
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace eShiftTransportManagementSystem
{
    public partial class RequestTransportForm : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int customerID;

        private List<PendingLoad> pendingLoads = new List<PendingLoad>();

        private class PendingLoad
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public string WeightSize { get; set; }
        }

        public RequestTransportForm() : this(0)
        {
        }

        public RequestTransportForm(int customerID)
        {
            InitializeComponent();

            this.customerID = customerID;

            btnDashboard.Click += btnDashboard_Click;
            btnCreateRequest.Click += btnCreateRequest_Click;
            btnMyJobs.Click += btnMyJobs_Click;
            btnCustomerLogout.Click += btnCustomerLogout_Click;

            btnAddLoad.Click += btnAddLoad_Click;
            btnSubmitRequest.Click += btnSubmitRequest_Click;
            btnClearRequest.Click += btnClearRequest_Click;

            this.Load += RequestTransportForm_Load;
        }

        // ==========================================
        // FORM LOAD
        // ==========================================

        private void RequestTransportForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCustomers();
                LoadPreferredTimes();
                GenerateJobNumber();

                dgvAddedLoads.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load the transport request form.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // LOAD CUSTOMERS
        // ==========================================

        private void LoadCustomers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT
                        CustomerID,
                        ISNULL(NULLIF(CustomerNumber, ''),
                        'C-' + CAST(CustomerID AS NVARCHAR(20)))
                        + ' - ' + FullName AS CustomerDisplay
                    FROM dbo.Customers
                    WHERE IsActive = 1
                    AND (@CustomerID = 0 OR CustomerID = @CustomerID)
                    ORDER BY FullName";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);

                    DataTable dt = new DataTable();

                    con.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    cmbCustomerNumber.DataSource = dt;
                    cmbCustomerNumber.DisplayMember = "CustomerDisplay";
                    cmbCustomerNumber.ValueMember = "CustomerID";

                    if (dt.Rows.Count > 0)
                    {
                        if (customerID > 0)
                            cmbCustomerNumber.SelectedValue = customerID;
                        else
                            cmbCustomerNumber.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbCustomerNumber.SelectedIndex = -1;

                        MessageBox.Show(
                            "No active customer account was found.",
                            "Customer Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // ==========================================
        // PREFERRED TIME
        // ==========================================

        private void LoadPreferredTimes()
        {
            cmbPreferredTime.Items.Clear();

            cmbPreferredTime.Items.Add("8:00 AM - 10:00 AM");
            cmbPreferredTime.Items.Add("10:00 AM - 12:00 PM");
            cmbPreferredTime.Items.Add("12:00 PM - 2:00 PM");
            cmbPreferredTime.Items.Add("2:00 PM - 4:00 PM");
            cmbPreferredTime.Items.Add("4:00 PM - 6:00 PM");

            cmbPreferredTime.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbPreferredTime.SelectedIndex = 0;
        }

        // ==========================================
        // GENERATE JOB NUMBER PREVIEW
        // ==========================================

        private void GenerateJobNumber()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT ISNULL(
                        MAX(TRY_CONVERT(INT, SUBSTRING(JobNumber, 3, 20))),
                        0
                    ) + 1
                    FROM dbo.TransportJobs
                    WHERE JobNumber LIKE 'J-%'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    int nextNumber = Convert.ToInt32(cmd.ExecuteScalar());

                    txtJobNumber.Text = "J-" + nextNumber.ToString("D4");
                }
            }
        }

        // ==========================================
        // ADD LOAD BUTTON
        // ==========================================

        private void btnAddLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string productName = txtCustomerNumber.Text.Trim();

                if (string.IsNullOrWhiteSpace(productName))
                {
                    MessageBox.Show(
                        "Please enter a Product / Load Name.",
                        "Required Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtCustomerNumber.Focus();
                    return;
                }

                int quantity;

                if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) ||
                    quantity <= 0)
                {
                    MessageBox.Show(
                        "Please enter a valid quantity.",
                        "Invalid Quantity",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtQuantity.Focus();
                    return;
                }

                int productID;
                string actualProductName;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT TOP 1 ProductID, ProductName
                        FROM dbo.Products
                        WHERE ProductName = @ProductName
                        AND IsActive = 1";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100)
                            .Value = productName;

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show(
                                    "The entered product was not found in the Products table.\n\n" +
                                    "Please enter an existing product name.",
                                    "Product Not Found",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                txtCustomerNumber.Focus();
                                return;
                            }

                            productID = Convert.ToInt32(reader["ProductID"]);
                            actualProductName = reader["ProductName"].ToString();
                        }
                    }
                }

                string weightSize = txtEstimatedWeightSize.Text.Trim();

                PendingLoad newLoad = new PendingLoad
                {
                    ProductID = productID,
                    ProductName = actualProductName,
                    Quantity = quantity,
                    WeightSize = weightSize
                };

                pendingLoads.Add(newLoad);

                dgvAddedLoads.Rows.Add(
                    actualProductName,
                    quantity,
                    weightSize,
                    "Remove");

                txtCustomerNumber.Clear();
                txtQuantity.Clear();
                txtEstimatedWeightSize.Clear();

                MessageBox.Show(
                    "Load added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtCustomerNumber.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to add load.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // REMOVE LOAD FROM GRID
        // ==========================================

        private void dgvAddedLoads_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex == 3)
            {
                if (e.RowIndex < pendingLoads.Count)
                {
                    pendingLoads.RemoveAt(e.RowIndex);
                    dgvAddedLoads.Rows.RemoveAt(e.RowIndex);

                    MessageBox.Show(
                        "Load removed successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        // ==========================================
        // SUBMIT TRANSPORT REQUEST
        // ==========================================

        private void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            if (cmbCustomerNumber.SelectedValue == null)
            {
                MessageBox.Show(
                    "Please select a customer.",
                    "Required Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtPickupLocation.Text))
            {
                MessageBox.Show(
                    "Please enter the Pickup Location.",
                    "Required Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPickupLocation.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDestinationLocation.Text))
            {
                MessageBox.Show(
                    "Please enter the Destination Location.",
                    "Required Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDestinationLocation.Focus();
                return;
            }

            if (dtpRequestedDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "Please select today or a future transport date.",
                    "Invalid Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (pendingLoads.Count == 0)
            {
                MessageBox.Show(
                    "Please add at least one load before submitting.",
                    "No Loads Added",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int selectedCustomerID =
                Convert.ToInt32(cmbCustomerNumber.SelectedValue);

            string pickup = txtPickupLocation.Text.Trim();
            string destination = txtDestinationLocation.Text.Trim();

            string specialInstructions =
                txtSpecialInstructions.Text.Trim();

            // Save preferred time together with special instructions.

            if (cmbPreferredTime.SelectedItem != null)
            {
                string preferredTime =
                    cmbPreferredTime.SelectedItem.ToString();

                specialInstructions =
                    "Preferred Time: " + preferredTime +
                    (string.IsNullOrWhiteSpace(specialInstructions)
                        ? ""
                        : Environment.NewLine + specialInstructions);
            }

            if (specialInstructions.Length > 1000)
            {
                MessageBox.Show(
                    "Special Instructions and Preferred Time must not exceed 1000 characters.",
                    "Information Too Long",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlTransaction transaction =
                        con.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            // IMPORTANT:
                            // JobNumber is a COMPUTED COLUMN.
                            // SQL Server generates it automatically.
                            // Do not insert or update JobNumber manually.

                            string insertJobQuery = @"
                                INSERT INTO dbo.TransportJobs
                                (
                                    CustomerID,
                                    PickupLocation,
                                    DestinationLocation,
                                    RequestedTransportDate,
                                    SpecialInstructions,
                                    RequestDate,
                                    Status
                                )
                                OUTPUT
                                    INSERTED.JobID,
                                    INSERTED.JobNumber
                                VALUES
                                (
                                    @CustomerID,
                                    @PickupLocation,
                                    @DestinationLocation,
                                    @RequestedTransportDate,
                                    @SpecialInstructions,
                                    GETDATE(),
                                    'Pending'
                                )";

                            int jobID;
                            string jobNumber;

                            using (SqlCommand cmd =
                                new SqlCommand(insertJobQuery, con, transaction))
                            {
                                cmd.Parameters.Add("@CustomerID",
                                    SqlDbType.Int).Value = selectedCustomerID;

                                cmd.Parameters.Add("@PickupLocation",
                                    SqlDbType.NVarChar, 250).Value = pickup;

                                cmd.Parameters.Add("@DestinationLocation",
                                    SqlDbType.NVarChar, 250).Value = destination;

                                cmd.Parameters.Add("@RequestedTransportDate",
                                    SqlDbType.Date).Value =
                                    dtpRequestedDate.Value.Date;

                                cmd.Parameters.Add("@SpecialInstructions",
                                    SqlDbType.NVarChar, 1000).Value =
                                    string.IsNullOrWhiteSpace(specialInstructions)
                                    ? (object)DBNull.Value
                                    : specialInstructions;

                                // Retrieve the automatically generated values.

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        throw new Exception(
                                            "Unable to retrieve the generated Job Number.");
                                    }

                                    jobID = Convert.ToInt32(reader["JobID"]);
                                    jobNumber = reader["JobNumber"].ToString();
                                }
                            }

                            // ==========================================
                            // INSERT ALL LOADS
                            // ==========================================

                            string insertLoadQuery = @"
                                INSERT INTO dbo.Loads
                                (
                                    JobID,
                                    ProductID,
                                    Quantity,
                                    LoadRemarks
                                )
                                VALUES
                                (
                                    @JobID,
                                    @ProductID,
                                    @Quantity,
                                    @LoadRemarks
                                )";

                            foreach (PendingLoad load in pendingLoads)
                            {
                                using (SqlCommand cmd =
                                    new SqlCommand(insertLoadQuery, con, transaction))
                                {
                                    cmd.Parameters.Add("@JobID",
                                        SqlDbType.Int).Value = jobID;

                                    cmd.Parameters.Add("@ProductID",
                                        SqlDbType.Int).Value = load.ProductID;

                                    cmd.Parameters.Add("@Quantity",
                                        SqlDbType.Int).Value = load.Quantity;

                                    cmd.Parameters.Add("@LoadRemarks",
                                        SqlDbType.NVarChar, 500).Value =
                                        string.IsNullOrWhiteSpace(load.WeightSize)
                                        ? (object)DBNull.Value
                                        : load.WeightSize;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // ==========================================
                            // COMMIT TRANSACTION
                            // ==========================================

                            transaction.Commit();

                            MessageBox.Show(
                                "Transport request submitted successfully!\n\n" +
                                "Job Number: " + jobNumber +
                                "\nTotal Loads: " + pendingLoads.Count +
                                "\nStatus: Pending",
                                "Request Submitted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            ClearRequestFields();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to submit transport request.\n\n" + ex.Message,
                    "Submission Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // CLEAR BUTTON
        // ==========================================

        private void btnClearRequest_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all entered information?",
                "Clear Request",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearRequestFields();

                MessageBox.Show(
                    "All entered information has been cleared.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void ClearRequestFields()
        {
            txtPickupLocation.Clear();
            txtDestinationLocation.Clear();
            txtSpecialInstructions.Clear();

            txtCustomerNumber.Clear();
            txtQuantity.Clear();
            txtEstimatedWeightSize.Clear();

            pendingLoads.Clear();
            dgvAddedLoads.Rows.Clear();

            dtpRequestedDate.Value = DateTime.Today;

            if (cmbPreferredTime.Items.Count > 0)
                cmbPreferredTime.SelectedIndex = 0;

            if (cmbCustomerNumber.Items.Count > 0)
            {
                if (customerID > 0)
                    cmbCustomerNumber.SelectedValue = customerID;
                else
                    cmbCustomerNumber.SelectedIndex = 0;
            }

            GenerateJobNumber();
        }

        // ==========================================
        // DASHBOARD NAVIGATION
        // ==========================================

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            NavigateToForm(
                new string[]
                {
                    "CustomerDashboardForm",
                    "FrmCustomerDashboard"
                });
        }

        // ==========================================
        // CREATE REQUEST NAVIGATION
        // ==========================================

        private void btnCreateRequest_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "You are already on the Create Transport Request page.",
                "Create Request",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==========================================
        // MY JOBS NAVIGATION
        // ==========================================

        private void btnMyJobs_Click(object sender, EventArgs e)
        {
            NavigateToForm(
                new string[]
                {
                    "FrmMyTransportJobs"
                });
        }

        // ==========================================
        // NAVIGATION HELPER
        // ==========================================

        private void NavigateToForm(string[] formNames)
        {
            try
            {
                Type targetType = null;

                Assembly assembly = Assembly.GetExecutingAssembly();

                foreach (Type type in assembly.GetTypes())
                {
                    if (formNames.Contains(type.Name))
                    {
                        targetType = type;
                        break;
                    }
                }

                if (targetType == null)
                {
                    MessageBox.Show(
                        "The selected page could not be found.\n\n" +
                        "Expected page: " + string.Join(" / ", formNames),
                        "Page Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Form targetForm = null;

                ConstructorInfo customerConstructor =
                    targetType.GetConstructor(new Type[] { typeof(int) });

                if (customerConstructor != null)
                {
                    targetForm = (Form)customerConstructor.Invoke(
                        new object[] { customerID });
                }
                else
                {
                    ConstructorInfo defaultConstructor =
                        targetType.GetConstructor(Type.EmptyTypes);

                    if (defaultConstructor != null)
                    {
                        targetForm = (Form)defaultConstructor.Invoke(null);
                    }
                }

                if (targetForm == null)
                {
                    MessageBox.Show(
                        "Unable to open the selected page. " +
                        "Please check its constructor.",
                        "Navigation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                PropertyInfo customerProperty =
                    targetType.GetProperty("CustomerID") ??
                    targetType.GetProperty("CurrentCustomerID");

                if (customerProperty != null &&
                    customerProperty.CanWrite &&
                    customerProperty.PropertyType == typeof(int))
                {
                    customerProperty.SetValue(targetForm, customerID);
                }

                targetForm.FormClosed += (s, args) =>
                {
                    if (!this.IsDisposed)
                        this.Show();
                };

                this.Hide();

                targetForm.StartPosition = FormStartPosition.CenterScreen;
                targetForm.Show();
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
        // LOGOUT
        // ==========================================

        private void btnCustomerLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Form loginForm = null;

                    foreach (Form form in Application.OpenForms)
                    {
                        if (form != this &&
                            (form.GetType().Name == "LoginForm" ||
                             form.GetType().Name == "FrmLogin"))
                        {
                            loginForm = form;
                            break;
                        }
                    }

                    if (loginForm == null)
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();

                        Type loginType = assembly.GetTypes().FirstOrDefault(
                            t => t.Name == "LoginForm" ||
                                 t.Name == "FrmLogin");

                        if (loginType != null)
                        {
                            loginForm = (Form)Activator.CreateInstance(loginType);
                        }
                    }

                    if (loginForm == null)
                    {
                        MessageBox.Show(
                            "Login form could not be found. " +
                            "Please check your login form class name.",
                            "Logout Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    MessageBox.Show(
                        "You have successfully logged out.",
                        "Logout Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    loginForm.Show();
                    loginForm.StartPosition = FormStartPosition.CenterScreen;
                    loginForm.BringToFront();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Logout failed.\n\n" + ex.Message,
                        "Logout Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // EXISTING DESIGNER EVENTS
        // ==========================================

        private void lblBetterTomorrow_Click(object sender, EventArgs e)
        {
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pnlLoads_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
