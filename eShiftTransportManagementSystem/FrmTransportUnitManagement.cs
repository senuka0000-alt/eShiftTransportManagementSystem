
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace eShiftTransportManagementSystem
{
    public partial class FrmTransportUnitManagement : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private int selectedUnitID = 0;

        public FrmTransportUnitManagement()
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

            txtSearchTransportUnits.TextChanged -= txtSearchTransportUnits_TextChanged;
            txtSearchTransportUnits.TextChanged += txtSearchTransportUnits_TextChanged;

            dgvTransportUnitList.CellClick -= dgvTransportUnitList_CellClick;
            dgvTransportUnitList.CellClick += dgvTransportUnitList_CellClick;

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

            this.Load -= FrmTransportUnitManagement_Load;
            this.Load += FrmTransportUnitManagement_Load;
        }

        // FORM LOAD
        private void FrmTransportUnitManagement_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDrivers();
                LoadAssistants();
                LoadContainerTypes();
                LoadAvailabilityStatuses();
                LoadTransportUnits();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Transport Unit Management.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // LOAD DRIVERS
        private void LoadDrivers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
SELECT DriverID, FullName
FROM dbo.Drivers
WHERE IsActive = 1
ORDER BY FullName;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    DataRow emptyRow = table.NewRow();
                    emptyRow["DriverID"] = DBNull.Value;
                    emptyRow["FullName"] = "-- Select Driver --";
                    table.Rows.InsertAt(emptyRow, 0);

                    cmbDriverDetails.DataSource = null;
                    cmbDriverDetails.Items.Clear();
                    cmbDriverDetails.DisplayMember = "FullName";
                    cmbDriverDetails.ValueMember = "DriverID";
                    cmbDriverDetails.DataSource = table;
                    cmbDriverDetails.SelectedIndex = 0;
                }
            }
        }

        // LOAD ASSISTANTS
        private void LoadAssistants()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
SELECT AssistantID, FullName
FROM dbo.Assistants
WHERE IsActive = 1
ORDER BY FullName;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    DataRow emptyRow = table.NewRow();
                    emptyRow["AssistantID"] = DBNull.Value;
                    emptyRow["FullName"] = "-- Select Assistant --";
                    table.Rows.InsertAt(emptyRow, 0);

                    cmbAssistantDetails.DataSource = null;
                    cmbAssistantDetails.Items.Clear();
                    cmbAssistantDetails.DisplayMember = "FullName";
                    cmbAssistantDetails.ValueMember = "AssistantID";
                    cmbAssistantDetails.DataSource = table;
                    cmbAssistantDetails.SelectedIndex = 0;
                }
            }
        }

        // LOAD CONTAINER TYPES
        private void LoadContainerTypes()
        {
            cmbContainerType.DataSource = null;
            cmbContainerType.Items.Clear();

            cmbContainerType.Items.Add("-- Select Container Type --");
            cmbContainerType.Items.Add("Standard");
            cmbContainerType.Items.Add("Refrigerated");
            cmbContainerType.Items.Add("Open Top");
            cmbContainerType.Items.Add("Flat Rack");
            cmbContainerType.Items.Add("Tank Container");

            cmbContainerType.SelectedIndex = 0;
        }

        // LOAD AVAILABILITY STATUS
        private void LoadAvailabilityStatuses()
        {
            cmbAvailabilityStatus.Items.Clear();

            cmbAvailabilityStatus.Items.Add("Available");
            cmbAvailabilityStatus.Items.Add("Unavailable");
            cmbAvailabilityStatus.Items.Add("Assigned");
            cmbAvailabilityStatus.Items.Add("Under Maintenance");

            cmbAvailabilityStatus.SelectedIndex = 0;
        }

        // LOAD TRANSPORT UNITS
        private void LoadTransportUnits()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"
SELECT
    tu.TransportUnitID,
    tu.UnitNumber,
    tu.UnitName,
    l.LorryID,
    l.RegistrationNumber AS LorryRegistration,
    l.UnitType AS LorryDetails,
    c.ContainerID,
    c.ContainerNumber,
    c.ContainerType,
    l.LoadCapacityKg AS LoadCapacity,
    tu.AvailabilityStatus,
    tu.Remarks,
    tu.DriverID,
    d.FullName AS DriverName,
    tu.AssistantID,
    a.FullName AS AssistantName
FROM dbo.TransportUnits tu
INNER JOIN dbo.Lorries l ON tu.LorryID = l.LorryID
INNER JOIN dbo.Containers c ON tu.ContainerID = c.ContainerID
LEFT JOIN dbo.Drivers d ON tu.DriverID = d.DriverID
LEFT JOIN dbo.Assistants a ON tu.AssistantID = a.AssistantID
WHERE tu.IsActive = 1
ORDER BY tu.TransportUnitID DESC;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvTransportUnitList.DataSource = table;
                        dgvTransportUnitList.ReadOnly = true;
                        dgvTransportUnitList.AllowUserToAddRows = false;
                        dgvTransportUnitList.AllowUserToDeleteRows = false;
                        dgvTransportUnitList.SelectionMode =
                            DataGridViewSelectionMode.FullRowSelect;
                        dgvTransportUnitList.MultiSelect = false;
                        dgvTransportUnitList.AutoSizeColumnsMode =
                            DataGridViewAutoSizeColumnsMode.Fill;

                        HideIDColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Transport Units.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ADD TRANSPORT UNIT
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            decimal capacity;

            if (!decimal.TryParse(txtLoadCapacity.Text.Trim(), out capacity) ||
                capacity <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive Load Capacity.",
                    "Invalid Capacity",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // INSERT LORRY
                    string lorryQuery = @"
INSERT INTO dbo.Lorries
(
    RegistrationNumber,
    UnitType,
    LoadCapacityKg,
    AvailabilityStatus,
    Remarks,
    IsActive,
    CreatedAt
)
VALUES
(
    @Registration,
    @UnitType,
    @Capacity,
    @Status,
    @Remarks,
    1,
    SYSDATETIME()
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int lorryID;

                    using (SqlCommand cmd =
                        new SqlCommand(lorryQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@Registration", txtLorryRegistration.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@UnitType", txtLorryDetails.Text.Trim());

                        cmd.Parameters.AddWithValue("@Capacity", capacity);
                        cmd.Parameters.AddWithValue("@Status", cmbAvailabilityStatus.Text);
                        cmd.Parameters.AddWithValue(
                            "@Remarks", txtTransportUnitRemarks.Text.Trim());

                        lorryID = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // INSERT CONTAINER
                    // ContainerNumber is a computed column.
                    // Do not insert ContainerNumber manually.
                    string containerQuery = @"
INSERT INTO dbo.Containers
(
    RegistrationNumber,
    ContainerType,
    LoadCapacityKg,
    AvailabilityStatus,
    Remarks,
    IsActive,
    CreatedAt
)
VALUES
(
    @ContainerNo,
    @ContainerType,
    @Capacity,
    @Status,
    @Remarks,
    1,
    SYSDATETIME()
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int containerID;

                    using (SqlCommand cmd =
                        new SqlCommand(containerQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@ContainerNo", txtContainerNo.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@ContainerType", cmbContainerType.Text.Trim());

                        cmd.Parameters.AddWithValue("@Capacity", capacity);
                        cmd.Parameters.AddWithValue("@Status", cmbAvailabilityStatus.Text);
                        cmd.Parameters.AddWithValue(
                            "@Remarks", txtTransportUnitRemarks.Text.Trim());

                        containerID = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // INSERT TRANSPORT UNIT
                    // UnitNumber is a computed column.
                    // Do not insert UnitNumber manually.
                    string unitQuery = @"
INSERT INTO dbo.TransportUnits
(
    LorryID,
    ContainerID,
    DriverID,
    AssistantID,
    UnitName,
    AvailabilityStatus,
    Remarks,
    IsActive,
    CreatedAt
)
VALUES
(
    @LorryID,
    @ContainerID,
    @DriverID,
    @AssistantID,
    @UnitName,
    @Status,
    @Remarks,
    1,
    SYSDATETIME()
);";

                    using (SqlCommand cmd =
                        new SqlCommand(unitQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@LorryID", lorryID);
                        cmd.Parameters.AddWithValue("@ContainerID", containerID);

                        cmd.Parameters.Add("@DriverID", SqlDbType.Int).Value =
                            GetSelectedID(cmbDriverDetails);

                        cmd.Parameters.Add("@AssistantID", SqlDbType.Int).Value =
                            GetSelectedID(cmbAssistantDetails);

                        cmd.Parameters.AddWithValue(
                            "@UnitName", txtLorryRegistration.Text.Trim());

                        cmd.Parameters.AddWithValue("@Status", cmbAvailabilityStatus.Text);

                        cmd.Parameters.AddWithValue(
                            "@Remarks", txtTransportUnitRemarks.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show(
                        "Transport Unit added successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadTransportUnits();
                    LoadDrivers();
                    LoadAssistants();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // Transaction may already be completed.
                    }

                    MessageBox.Show(
                        "Unable to add Transport Unit.\n\n" + ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // UPDATE TRANSPORT UNIT
        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (selectedUnitID == 0)
            {
                MessageBox.Show(
                    "Please select a Transport Unit first.",
                    "Select Transport Unit",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateFields())
                return;

            decimal capacity;

            if (!decimal.TryParse(txtLoadCapacity.Text.Trim(), out capacity) ||
                capacity <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive Load Capacity.",
                    "Invalid Capacity",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    int lorryID;
                    int containerID;

                    string getIDs = @"
SELECT LorryID, ContainerID
FROM dbo.TransportUnits
WHERE TransportUnitID = @ID AND IsActive = 1;";

                    using (SqlCommand cmd =
                        new SqlCommand(getIDs, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ID", selectedUnitID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new Exception("Transport Unit was not found.");

                            lorryID = Convert.ToInt32(reader["LorryID"]);
                            containerID = Convert.ToInt32(reader["ContainerID"]);
                        }
                    }

                    // UPDATE LORRY
                    string lorryQuery = @"
UPDATE dbo.Lorries
SET
    RegistrationNumber = @Registration,
    UnitType = @UnitType,
    LoadCapacityKg = @Capacity,
    AvailabilityStatus = @Status,
    Remarks = @Remarks
WHERE LorryID = @LorryID;";

                    using (SqlCommand cmd =
                        new SqlCommand(lorryQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@Registration", txtLorryRegistration.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@UnitType", txtLorryDetails.Text.Trim());

                        cmd.Parameters.AddWithValue("@Capacity", capacity);
                        cmd.Parameters.AddWithValue("@Status", cmbAvailabilityStatus.Text);

                        cmd.Parameters.AddWithValue(
                            "@Remarks", txtTransportUnitRemarks.Text.Trim());

                        cmd.Parameters.AddWithValue("@LorryID", lorryID);

                        cmd.ExecuteNonQuery();
                    }

                    // UPDATE CONTAINER
                    // ContainerNumber is computed.
                    // Update RegistrationNumber instead.
                    string containerQuery = @"
UPDATE dbo.Containers
SET
    RegistrationNumber = @ContainerNo,
    ContainerType = @ContainerType,
    LoadCapacityKg = @Capacity,
    AvailabilityStatus = @Status,
    Remarks = @Remarks
WHERE ContainerID = @ContainerID;";

                    using (SqlCommand cmd =
                        new SqlCommand(containerQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@ContainerNo", txtContainerNo.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@ContainerType", cmbContainerType.Text.Trim());

                        cmd.Parameters.AddWithValue("@Capacity", capacity);
                        cmd.Parameters.AddWithValue("@Status", cmbAvailabilityStatus.Text);

                        cmd.Parameters.AddWithValue(
                            "@Remarks", txtTransportUnitRemarks.Text.Trim());

                        cmd.Parameters.AddWithValue("@ContainerID", containerID);

                        cmd.ExecuteNonQuery();
                    }

                    // UPDATE TRANSPORT UNIT
                    // UnitNumber is computed.
                    // Do not update UnitNumber manually.
                    string unitQuery = @"
UPDATE dbo.TransportUnits
SET
    UnitName = @UnitName,
    DriverID = @DriverID,
    AssistantID = @AssistantID,
    AvailabilityStatus = @Status,
    Remarks = @Remarks
WHERE TransportUnitID = @ID;";

                    using (SqlCommand cmd =
                        new SqlCommand(unitQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@UnitName", txtLorryRegistration.Text.Trim());

                        cmd.Parameters.Add("@DriverID", SqlDbType.Int).Value =
                            GetSelectedID(cmbDriverDetails);

                        cmd.Parameters.Add("@AssistantID", SqlDbType.Int).Value =
                            GetSelectedID(cmbAssistantDetails);

                        cmd.Parameters.AddWithValue("@Status", cmbAvailabilityStatus.Text);

                        cmd.Parameters.AddWithValue(
                            "@Remarks", txtTransportUnitRemarks.Text.Trim());

                        cmd.Parameters.AddWithValue("@ID", selectedUnitID);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show(
                        "Transport Unit updated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadTransportUnits();
                    LoadDrivers();
                    LoadAssistants();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // Transaction may already be completed.
                    }

                    MessageBox.Show(
                        "Unable to update Transport Unit.\n\n" + ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // DELETE TRANSPORT UNIT
        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (selectedUnitID == 0)
            {
                MessageBox.Show(
                    "Please select a Transport Unit first.",
                    "Select Transport Unit",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this Transport Unit?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
UPDATE dbo.TransportUnits
SET IsActive = 0
WHERE TransportUnitID = @ID;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", selectedUnitID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Transport Unit deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTransportUnits();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete Transport Unit.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // SEARCH TRANSPORT UNITS
        private void txtSearchTransportUnits_TextChanged(
            object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"
SELECT
    tu.TransportUnitID,
    tu.UnitNumber,
    tu.UnitName,
    l.LorryID,
    l.RegistrationNumber AS LorryRegistration,
    l.UnitType AS LorryDetails,
    c.ContainerID,
    c.ContainerNumber,
    c.ContainerType,
    l.LoadCapacityKg AS LoadCapacity,
    tu.AvailabilityStatus,
    tu.Remarks,
    tu.DriverID,
    d.FullName AS DriverName,
    tu.AssistantID,
    a.FullName AS AssistantName
FROM dbo.TransportUnits tu
INNER JOIN dbo.Lorries l ON tu.LorryID = l.LorryID
INNER JOIN dbo.Containers c ON tu.ContainerID = c.ContainerID
LEFT JOIN dbo.Drivers d ON tu.DriverID = d.DriverID
LEFT JOIN dbo.Assistants a ON tu.AssistantID = a.AssistantID
WHERE tu.IsActive = 1
AND
(
    tu.UnitNumber LIKE @Search
    OR l.RegistrationNumber LIKE @Search
    OR l.UnitType LIKE @Search
    OR c.ContainerNumber LIKE @Search
)
ORDER BY tu.TransportUnitID DESC;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue(
                            "@Search",
                            "%" + txtSearchTransportUnits.Text.Trim() + "%");

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvTransportUnitList.DataSource = table;
                        HideIDColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Search failed.\n\n" + ex.Message,
                    "Search Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // HIDE ID COLUMNS
        private void HideIDColumns()
        {
            string[] hiddenColumns =
            {
                "LorryID",
                "ContainerID",
                "DriverID",
                "AssistantID"
            };

            foreach (string column in hiddenColumns)
            {
                if (dgvTransportUnitList.Columns.Contains(column))
                {
                    dgvTransportUnitList.Columns[column].Visible = false;
                }
            }
        }

        // SELECT TRANSPORT UNIT FROM GRID
        private void dgvTransportUnitList_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvTransportUnitList.Rows[e.RowIndex];

            selectedUnitID = Convert.ToInt32(
                row.Cells["TransportUnitID"].Value);

            txtUnitID.Text = selectedUnitID.ToString();

            txtLorryRegistration.Text =
                GetCellValue(row, "LorryRegistration");

            txtLorryDetails.Text =
                GetCellValue(row, "LorryDetails");

            txtContainerNo.Text =
                GetCellValue(row, "ContainerNumber");

            txtLoadCapacity.Text =
                GetCellValue(row, "LoadCapacity");

            txtTransportUnitRemarks.Text =
                GetCellValue(row, "Remarks");

            cmbContainerType.Text =
                GetCellValue(row, "ContainerType");

            cmbAvailabilityStatus.Text =
                GetCellValue(row, "AvailabilityStatus");

            SelectComboValue(cmbDriverDetails, "DriverID", row);
            SelectComboValue(cmbAssistantDetails, "AssistantID", row);
        }

        private string GetCellValue(
            DataGridViewRow row, string columnName)
        {
            object value = row.Cells[columnName].Value;

            return value == null || value == DBNull.Value
                ? ""
                : value.ToString();
        }

        private void SelectComboValue(
            ComboBox combo, string columnName, DataGridViewRow row)
        {
            object value = row.Cells[columnName].Value;

            if (value == null || value == DBNull.Value)
            {
                combo.SelectedIndex = 0;
            }
            else
            {
                combo.SelectedValue = Convert.ToInt32(value);
            }
        }

        // VALIDATE FIELDS
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtLorryRegistration.Text))
            {
                MessageBox.Show(
                    "Please enter Lorry Registration Number.",
                    "Required Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLorryRegistration.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLorryDetails.Text))
            {
                MessageBox.Show(
                    "Please enter Lorry Details.",
                    "Required Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLorryDetails.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContainerNo.Text))
            {
                MessageBox.Show(
                    "Please enter Container Number.",
                    "Required Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtContainerNo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbContainerType.Text) ||
                cmbContainerType.SelectedIndex <= 0)
            {
                MessageBox.Show(
                    "Please select Container Type.",
                    "Required Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbContainerType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLoadCapacity.Text))
            {
                MessageBox.Show(
                    "Please enter Load Capacity.",
                    "Required Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLoadCapacity.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbAvailabilityStatus.Text))
            {
                MessageBox.Show(
                    "Please select Availability Status.",
                    "Required Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbAvailabilityStatus.Focus();
                return false;
            }

            return true;
        }

        // GET SELECTED DRIVER OR ASSISTANT ID
        private object GetSelectedID(ComboBox combo)
        {
            if (combo.SelectedValue == null ||
                combo.SelectedValue == DBNull.Value ||
                combo.SelectedIndex <= 0)
            {
                return DBNull.Value;
            }

            return Convert.ToInt32(combo.SelectedValue);
        }

        // RESET BUTTON
        private void btnResetAdmin_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // CLEAR BUTTON
        private void btnClearCustomer_Click(object sender, EventArgs e)
        {
            ClearFields();
            txtSearchTransportUnits.Clear();
        }

        // CLEAR ALL FIELDS
        private void ClearFields()
        {
            selectedUnitID = 0;

            txtUnitID.Clear();
            txtLorryRegistration.Clear();
            txtLorryDetails.Clear();
            txtContainerNo.Clear();
            txtLoadCapacity.Clear();
            txtTransportUnitRemarks.Clear();

            cmbDriverDetails.SelectedIndex =
                cmbDriverDetails.Items.Count > 0 ? 0 : -1;

            cmbAssistantDetails.SelectedIndex =
                cmbAssistantDetails.Items.Count > 0 ? 0 : -1;

            cmbContainerType.SelectedIndex = 0;
            cmbAvailabilityStatus.SelectedIndex = 0;

            dgvTransportUnitList.ClearSelection();

            txtLorryRegistration.Focus();
        }

        // SIDEBAR NAVIGATION
        private void OpenChildForm(Form child)
        {
            if (child == null || IsDisposed || Disposing)
                return;

            Hide();

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
                if (!IsDisposed && !Disposing)
                {
                    Show();
                    Activate();
                    LoadTransportUnits();
                }
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Close();
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
            LoadTransportUnits();
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

        private void btnReports_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmReports());
        }

        // LOGOUT
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            AdminDashboardForm dashboard = null;

            foreach (Form form in Application.OpenForms)
            {
                dashboard = form as AdminDashboardForm;

                if (dashboard != null)
                    break;
            }

            if (dashboard != null)
            {
                dashboard.RequestLogout();
                Close();
            }
            else
            {
                LoginForm login = new LoginForm();

                login.StartPosition = FormStartPosition.CenterScreen;
                login.Show();

                Close();
            }
        }

        private void pnlTransportUnitDetails_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

