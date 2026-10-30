using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace eShiftTransportManagementSystem
{
    public partial class FrmProductManagement : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int selectedProductID = 0;
        private bool isLoggingOut = false;

        public FrmProductManagement()
        {
            InitializeComponent();

            this.Load -= FrmProductManagement_Load;
            this.Load += FrmProductManagement_Load;

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

            txtSearchProducts.TextChanged -= txtSearchProducts_TextChanged;
            txtSearchProducts.TextChanged += txtSearchProducts_TextChanged;

            dgvProductList.CellClick -= dgvProductList_CellClick;
            dgvProductList.CellClick += dgvProductList_CellClick;

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

        private void FrmProductManagement_Load(object sender, EventArgs e)
        {
            txtProductID.ReadOnly = true;

            cmbProductCategory.Items.Clear();
            cmbProductCategory.Items.AddRange(new object[]
            {
                "Furniture",
                "Electronics",
                "Kitchenware",
                "Clothing",
                "Appliances",
                "Other"
            });
            cmbProductCategory.DropDownStyle = ComboBoxStyle.DropDown;

            cmbProductUnit.Items.Clear();
            cmbProductUnit.Items.AddRange(new object[]
            {
                "Piece",
                "Box",
                "Carton",
                "Bag",
                "Set",
                "Item"
            });
            cmbProductUnit.DropDownStyle = ComboBoxStyle.DropDown;

            dgvProductList.ReadOnly = true;
            dgvProductList.AllowUserToAddRows = false;
            dgvProductList.AllowUserToDeleteRows = false;
            dgvProductList.MultiSelect = false;
            dgvProductList.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvProductList.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LoadProducts();
            ResetProductFields();
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string sql = @"
SELECT
    p.ProductID,
    p.ProductNumber,
    p.ProductName,
    p.Category,
    p.Unit,
    p.Description,
    p.AverageSize,
    p.AverageWeight,
    p.IsActive
FROM dbo.Products AS p
WHERE
    (
        @Search = ''
        OR CONVERT(NVARCHAR(50), p.ProductID) LIKE @Pattern
        OR ISNULL(p.ProductNumber, '') LIKE @Pattern
        OR p.ProductName LIKE @Pattern
        OR p.Category LIKE @Pattern
        OR p.Unit LIKE @Pattern
        OR ISNULL(p.Description, '') LIKE @Pattern
    )
ORDER BY p.ProductID DESC;";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        string search = txtSearchProducts.Text.Trim();

                        cmd.Parameters.Add("@Search",
                            SqlDbType.NVarChar, 200).Value = search;

                        cmd.Parameters.Add("@Pattern",
                            SqlDbType.NVarChar, 202).Value =
                            "%" + search + "%";

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            dgvProductList.DataSource = table;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load products.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void txtSearchProducts_TextChanged(
            object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void dgvProductList_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvProductList.Rows[e.RowIndex];

            if (row.Cells["ProductID"].Value == null)
                return;

            selectedProductID =
                Convert.ToInt32(row.Cells["ProductID"].Value);

            txtProductID.Text = selectedProductID.ToString();
            txtProductName.Text = CellText(row, "ProductName");
            txtProductDescription.Text = CellText(row, "Description");
            txtAverageSize.Text = CellText(row, "AverageSize");
            txtAverageWeight.Text = CellText(row, "AverageWeight");

            cmbProductCategory.Text = CellText(row, "Category");
            cmbProductUnit.Text = CellText(row, "Unit");

            txtLoadNumber.Clear();
            txtProductQuantity.Clear();
            txtAssociatedJobNo.Clear();

            LoadProductLoadDetails();
        }

        private string CellText(DataGridViewRow row, string column)
        {
            if (!dgvProductList.Columns.Contains(column))
                return "";

            object value = row.Cells[column].Value;

            return value == null || value == DBNull.Value
                ? ""
                : value.ToString();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (!ValidateProduct())
                return;

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        string sql = @"
INSERT INTO dbo.Products
(
    ProductName,
    Description,
    Category,
    Unit,
    AverageSize,
    AverageWeight,
    CreatedAt,
    IsActive
)
VALUES
(
    @Name,
    @Description,
    @Category,
    @Unit,
    @AverageSize,
    @AverageWeight,
    SYSDATETIME(),
    1
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int newProductID;

                        using (SqlCommand cmd =
                            new SqlCommand(sql, con, transaction))
                        {
                            AddProductParameters(cmd);
                            newProductID = Convert.ToInt32(
                                cmd.ExecuteScalar());
                        }

                        if (HasLoadDetails())
                        {
                            SaveLoadDetails(
                                con, transaction, newProductID);
                        }

                        transaction.Commit();

                        MessageBox.Show(
                            "Product added successfully.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadProducts();
                        ResetProductFields();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        MessageBox.Show(
                            "Unable to add product.\n\n" + ex.Message,
                            "Database Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (selectedProductID == 0)
            {
                MessageBox.Show(
                    "Select a product from the table first.",
                    "Select Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateProduct())
                return;

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        string sql = @"
UPDATE dbo.Products
SET ProductName = @Name,
    Description = @Description,
    Category = @Category,
    Unit = @Unit,
    AverageSize = @AverageSize,
    AverageWeight = @AverageWeight
WHERE ProductID = @ProductID;";

                        using (SqlCommand cmd =
                            new SqlCommand(sql, con, transaction))
                        {
                            AddProductParameters(cmd);

                            cmd.Parameters.Add("@ProductID", SqlDbType.Int)
                                .Value = selectedProductID;

                            if (cmd.ExecuteNonQuery() == 0)
                            {
                                throw new Exception(
                                    "The selected product no longer exists.");
                            }
                        }

                        if (HasLoadDetails())
                        {
                            SaveLoadDetails(
                                con, transaction, selectedProductID);
                        }

                        transaction.Commit();

                        MessageBox.Show(
                            "Product updated successfully.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadProducts();
                        ResetProductFields();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        MessageBox.Show(
                            "Unable to update product.\n\n" + ex.Message,
                            "Database Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (selectedProductID == 0)
            {
                MessageBox.Show(
                    "Select a product from the table first.",
                    "Select Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "Delete this product? Products used in existing loads may not be deletable.",
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
DELETE FROM dbo.Products
WHERE ProductID = @ProductID
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.Loads
      WHERE Loads.ProductID = Products.ProductID
  );", con))
                    {
                        cmd.Parameters.Add("@ProductID", SqlDbType.Int)
                            .Value = selectedProductID;

                        int affected = cmd.ExecuteNonQuery();

                        if (affected == 0)
                        {
                            MessageBox.Show(
                                "The product was not deleted. It may be used in existing loads.",
                                "Cannot Delete Product",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Product deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadProducts();
                ResetProductFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete product.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ValidateProduct()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                string.IsNullOrWhiteSpace(cmbProductCategory.Text) ||
                string.IsNullOrWhiteSpace(cmbProductUnit.Text))
            {
                MessageBox.Show(
                    "Enter Product Name, Category, and Unit.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            decimal weight;

            if (!string.IsNullOrWhiteSpace(txtAverageWeight.Text) &&
                (!decimal.TryParse(txtAverageWeight.Text.Trim(), out weight)
                 || weight < 0))
            {
                MessageBox.Show(
                    "Average Weight must be a valid non-negative number.",
                    "Invalid Weight",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            int quantity;

            if (!string.IsNullOrWhiteSpace(txtProductQuantity.Text) &&
                (!int.TryParse(txtProductQuantity.Text.Trim(), out quantity)
                 || quantity <= 0))
            {
                MessageBox.Show(
                    "Product Quantity must be a positive whole number.",
                    "Invalid Quantity",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void AddProductParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar)
                .Value = txtProductName.Text.Trim();

            cmd.Parameters.Add("@Description", SqlDbType.NVarChar)
                .Value = string.IsNullOrWhiteSpace(txtProductDescription.Text)
                    ? (object)DBNull.Value
                    : txtProductDescription.Text.Trim();

            cmd.Parameters.Add("@Category", SqlDbType.NVarChar)
                .Value = cmbProductCategory.Text.Trim();

            cmd.Parameters.Add("@Unit", SqlDbType.NVarChar)
                .Value = cmbProductUnit.Text.Trim();

            cmd.Parameters.Add("@AverageSize", SqlDbType.NVarChar)
                .Value = string.IsNullOrWhiteSpace(txtAverageSize.Text)
                    ? (object)DBNull.Value
                    : txtAverageSize.Text.Trim();

            SqlParameter weightParameter =
                cmd.Parameters.Add("@AverageWeight", SqlDbType.Decimal);

            weightParameter.Precision = 18;
            weightParameter.Scale = 2;

            decimal weight;

            weightParameter.Value =
                decimal.TryParse(txtAverageWeight.Text.Trim(), out weight)
                ? (object)weight
                : DBNull.Value;
        }

        private bool HasLoadDetails()
        {
            return !string.IsNullOrWhiteSpace(txtAssociatedJobNo.Text) ||
                   !string.IsNullOrWhiteSpace(txtLoadNumber.Text) ||
                   !string.IsNullOrWhiteSpace(txtProductQuantity.Text);
        }

        private void SaveLoadDetails(
            SqlConnection con,
            SqlTransaction transaction,
            int productID)
        {
            if (string.IsNullOrWhiteSpace(txtAssociatedJobNo.Text))
                throw new Exception(
                    "Enter an Associated Job Number when adding load details.");

            int quantity;

            if (!int.TryParse(txtProductQuantity.Text.Trim(), out quantity)
                || quantity <= 0)
            {
                throw new Exception(
                    "Enter a valid positive load quantity.");
            }

            int jobID;

            using (SqlCommand findJob = new SqlCommand(@"
SELECT JobID
FROM dbo.TransportJobs
WHERE JobNumber = @JobNumber;", con, transaction))
            {
                findJob.Parameters.Add("@JobNumber", SqlDbType.VarChar, 50)
                    .Value = txtAssociatedJobNo.Text.Trim();

                object result = findJob.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    throw new Exception(
                        "The associated Job Number was not found.");

                jobID = Convert.ToInt32(result);
            }

            string loadNumber = txtLoadNumber.Text.Trim();

            int existingLoadID = 0;

            using (SqlCommand findLoad = new SqlCommand(@"
SELECT TOP 1 LoadID
FROM dbo.Loads
WHERE JobID = @JobID AND ProductID = @ProductID
ORDER BY LoadID DESC;", con, transaction))
            {
                findLoad.Parameters.Add("@JobID", SqlDbType.Int).Value = jobID;
                findLoad.Parameters.Add("@ProductID", SqlDbType.Int)
                    .Value = productID;

                object result = findLoad.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                    existingLoadID = Convert.ToInt32(result);
            }

            string sql;

            if (existingLoadID > 0)
            {
                sql = @"
UPDATE dbo.Loads
SET LoadNumber = @LoadNumber,
    Quantity = @Quantity
WHERE LoadID = @LoadID;";
            }
            else
            {
                sql = @"
INSERT INTO dbo.Loads
    (LoadNumber, JobID, ProductID, Quantity, LoadRemarks, CreatedAt)
VALUES
    (@LoadNumber, @JobID, @ProductID, @Quantity, NULL, SYSDATETIME());";
            }

            using (SqlCommand cmd =
                new SqlCommand(sql, con, transaction))
            {
                cmd.Parameters.Add("@LoadNumber", SqlDbType.VarChar, 50)
                    .Value = string.IsNullOrWhiteSpace(loadNumber)
                        ? (object)DBNull.Value
                        : loadNumber;

                cmd.Parameters.Add("@JobID", SqlDbType.Int).Value = jobID;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int)
                    .Value = productID;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                    .Value = quantity;

                if (existingLoadID > 0)
                {
                    cmd.Parameters.Add("@LoadID", SqlDbType.Int)
                        .Value = existingLoadID;
                }

                cmd.ExecuteNonQuery();
            }
        }

        private void LoadProductLoadDetails()
        {
            if (selectedProductID == 0)
                return;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1
    l.LoadNumber,
    l.Quantity,
    j.JobNumber
FROM dbo.Loads AS l
INNER JOIN dbo.TransportJobs AS j
    ON l.JobID = j.JobID
WHERE l.ProductID = @ProductID
ORDER BY l.LoadID DESC;", con))
                    {
                        cmd.Parameters.Add("@ProductID", SqlDbType.Int)
                            .Value = selectedProductID;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtLoadNumber.Text =
                                    reader["LoadNumber"] == DBNull.Value
                                    ? ""
                                    : reader["LoadNumber"].ToString();

                                txtProductQuantity.Text =
                                    reader["Quantity"].ToString();

                                txtAssociatedJobNo.Text =
                                    reader["JobNumber"] == DBNull.Value
                                    ? ""
                                    : reader["JobNumber"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load associated load details.\n\n" +
                    ex.Message,
                    "Load Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnResetAdmin_Click(object sender, EventArgs e)
        {
            ResetProductFields();

            MessageBox.Show(
                "Product form reset.",
                "Reset",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnClearCustomer_Click(object sender, EventArgs e)
        {
            txtSearchProducts.Clear();
            ResetProductFields();
            LoadProducts();

            MessageBox.Show(
                "Search cleared.",
                "Clear Search",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ResetProductFields()
        {
            selectedProductID = 0;

            txtProductID.Clear();
            txtProductName.Clear();
            txtProductDescription.Clear();
            txtAverageSize.Clear();
            txtAverageWeight.Clear();

            txtLoadNumber.Clear();
            txtProductQuantity.Clear();
            txtAssociatedJobNo.Clear();

            cmbProductCategory.SelectedIndex = -1;
            cmbProductCategory.Text = "";

            cmbProductUnit.SelectedIndex = -1;
            cmbProductUnit.Text = "";

            dgvProductList.ClearSelection();
            txtProductName.Focus();
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
                    LoadProducts();
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
            OpenChildForm(new FrmJobManagement());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btnTransportUnits_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmTransportUnitManagement());
        }

        private void btnManageJobAssignments_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "The Job Assignments management form has not been connected yet.",
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

            // Ask the parent Admin Dashboard to complete logout.
            // The dashboard will show LoginForm after this dialog closes.
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminDashboardForm)
                {
                    ((AdminDashboardForm)form).RequestLogout();
                    break;
                }
            }

            // Close this Product Management child form.
            Close();
        }

        private void picMovingTruck_Click(object sender, EventArgs e)
        {
        }
    }
}
