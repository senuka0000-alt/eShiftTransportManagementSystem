
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace eShiftTransportManagementSystem
{
    public partial class FrmReports : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=eShiftTransportDB;Integrated Security=True;TrustServerCertificate=True;";

        private DataTable reportTable = new DataTable();
        private PrintDocument printDocument = new PrintDocument();
        private int printRowIndex = 0;

        public FrmReports()
        {
            InitializeComponent();

            btnGenerateReport.Click -= btnGenerateReport_Click;
            btnGenerateReport.Click += btnGenerateReport_Click;

            btnPrintReport.Click -= btnPrintReport_Click;
            btnPrintReport.Click += btnPrintReport_Click;

            btnExportReport.Click -= btnExportReport_Click;
            btnExportReport.Click += btnExportReport_Click;

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

            this.Load -= FrmReports_Load;
            this.Load += FrmReports_Load;

            printDocument.PrintPage += printDocument_PrintPage;
        }

        // ==========================================
        // FORM LOAD
        // ==========================================

        private void FrmReports_Load(object sender, EventArgs e)
        {
            try
            {
                cmbReportType.Items.Clear();

                cmbReportType.Items.Add("All Transport Jobs");
                cmbReportType.Items.Add("All Customers");
                cmbReportType.Items.Add("Pending Requests");
                cmbReportType.Items.Add("Approved Jobs");
                cmbReportType.Items.Add("Completed Jobs");

                cmbReportType.SelectedIndex = 0;

                cmbReportStatus.Items.Clear();

                cmbReportStatus.Items.Add("All");
                cmbReportStatus.Items.Add("Pending");
                cmbReportStatus.Items.Add("Accepted");
                cmbReportStatus.Items.Add("Completed");
                cmbReportStatus.Items.Add("Declined");

                cmbReportStatus.SelectedIndex = 0;

                dtpStartDate.Value = DateTime.Today.AddMonths(-1);
                dtpEndDate.Value = DateTime.Today;

                LoadSummaryCards();
                LoadJobsChart();

                // Automatically load the report without a success message.
                GenerateReport(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Reports page.\n\n" + ex.Message,
                    "Reports Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // LOAD SUMMARY CARDS
        // ==========================================

        private void LoadSummaryCards()
        {
            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
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
     WHERE Status = 'Completed') AS CompletedJobs;";

                using (SqlCommand cmd = new SqlCommand(query, con))
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

                        lblCompletedJobs.Text =
                            reader["CompletedJobs"].ToString();
                    }
                }
            }
        }

        // ==========================================
        // LOAD JOBS OVERVIEW CHART
        // ==========================================

        private void LoadJobsChart()
        {
            chartJobsOverview.Series.Clear();
            chartJobsOverview.ChartAreas.Clear();
            chartJobsOverview.Legends.Clear();

            ChartArea area = new ChartArea("JobsArea");
            chartJobsOverview.ChartAreas.Add(area);

            Series series = new Series("Jobs");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;

            chartJobsOverview.Series.Add(series);

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
SELECT Status, COUNT(*) AS Total
FROM dbo.TransportJobs
GROUP BY Status;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string status = reader["Status"].ToString();
                        int total = Convert.ToInt32(reader["Total"]);

                        series.Points.AddXY(status, total);
                    }
                }
            }

            chartJobsOverview.Titles.Clear();
            chartJobsOverview.Titles.Add("Transport Jobs Overview");
        }

        // ==========================================
        // GENERATE REPORT BUTTON
        // ==========================================

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport(bool showSuccessMessage = true)
        {
            try
            {
                if (dtpStartDate.Value.Date > dtpEndDate.Value.Date)
                {
                    MessageBox.Show(
                        "Start Date cannot be later than End Date.",
                        "Invalid Date Range",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                string reportType = cmbReportType.Text;
                string status = cmbReportStatus.Text;

                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    string query;

                    if (reportType == "All Customers")
                    {
                        query = @"
SELECT
    CustomerID,
    FullName,
    Address,
    Telephone,
    Email
FROM dbo.Customers
WHERE IsActive = 1
ORDER BY CustomerID DESC;";
                    }
                    else
                    {
                        query = @"
SELECT
    j.JobID,
    j.JobNumber,
    c.FullName AS Customer,
    j.RequestDate,
    j.RequestedTransportDate,
    j.PickupLocation,
    j.DestinationLocation,
    j.Status
FROM dbo.TransportJobs j
LEFT JOIN dbo.Customers c
    ON j.CustomerID = c.CustomerID
WHERE j.RequestDate >= @StartDate
AND j.RequestDate < @EndDate";

                        if (reportType == "Pending Requests")
                        {
                            query += " AND j.Status = 'Pending'";
                        }
                        else if (reportType == "Approved Jobs")
                        {
                            query += " AND j.Status = 'Accepted'";
                        }
                        else if (reportType == "Completed Jobs")
                        {
                            query += " AND j.Status = 'Completed'";
                        }
                        else if (status != "All")
                        {
                            query += " AND j.Status = @Status";
                        }

                        query += " ORDER BY j.RequestDate DESC;";
                    }

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        if (reportType != "All Customers")
                        {
                            cmd.Parameters.AddWithValue(
                                "@StartDate",
                                dtpStartDate.Value.Date);

                            cmd.Parameters.AddWithValue(
                                "@EndDate",
                                dtpEndDate.Value.Date.AddDays(1));

                            if (reportType == "All Transport Jobs" &&
                                status != "All")
                            {
                                cmd.Parameters.AddWithValue(
                                    "@Status", status);
                            }
                        }

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            reportTable = new DataTable();
                            adapter.Fill(reportTable);

                            dgvGeneratedReports.DataSource = reportTable;
                        }
                    }
                }

                FormatReportGrid();

                // Show success message only when the user
                // manually clicks Generate Report.
                if (showSuccessMessage)
                {
                    MessageBox.Show(
                        "Report generated successfully!",
                        "Reports",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to generate report.\n\n" + ex.Message,
                    "Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // FORMAT REPORT GRID
        // ==========================================

        private void FormatReportGrid()
        {
            dgvGeneratedReports.ReadOnly = true;
            dgvGeneratedReports.AllowUserToAddRows = false;
            dgvGeneratedReports.AllowUserToDeleteRows = false;

            dgvGeneratedReports.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvGeneratedReports.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvGeneratedReports.MultiSelect = false;
        }

        // ==========================================
        // PRINT REPORT BUTTON
        // ==========================================

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            if (reportTable == null || reportTable.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Please generate a report before printing.",
                    "No Report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (PrintDialog dialog = new PrintDialog())
                {
                    dialog.Document = printDocument;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        printRowIndex = 0;
                        printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to print report.\n\n" + ex.Message,
                    "Print Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // PRINT PAGE CONTENT
        // ==========================================

        private void printDocument_PrintPage(
            object sender, PrintPageEventArgs e)
        {
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font headerFont = new Font("Arial", 9, FontStyle.Bold);
            Font rowFont = new Font("Arial", 8);

            float x = 40;
            float y = 40;

            e.Graphics.DrawString(
                "eShift Transport Management System",
                titleFont,
                Brushes.Black,
                x,
                y);

            y += 35;

            e.Graphics.DrawString(
                "Generated Report - " +
                DateTime.Now.ToString("dd/MM/yyyy"),
                rowFont,
                Brushes.Black,
                x,
                y);

            y += 35;

            foreach (DataColumn column in reportTable.Columns)
            {
                e.Graphics.DrawString(
                    column.ColumnName,
                    headerFont,
                    Brushes.Black,
                    x,
                    y);

                x += 110;
            }

            x = 40;
            y += 25;

            while (printRowIndex < reportTable.Rows.Count)
            {
                if (y > e.MarginBounds.Bottom - 30)
                {
                    e.HasMorePages = true;
                    return;
                }

                DataRow row = reportTable.Rows[printRowIndex];

                foreach (DataColumn column in reportTable.Columns)
                {
                    string value = row[column].ToString();

                    e.Graphics.DrawString(
                        value.Length > 17
                            ? value.Substring(0, 17)
                            : value,
                        rowFont,
                        Brushes.Black,
                        x,
                        y);

                    x += 110;
                }

                x = 40;
                y += 25;
                printRowIndex++;
            }

            e.HasMorePages = false;
        }

        // ==========================================
        // EXPORT REPORT BUTTON
        // ==========================================

        private void btnExportReport_Click(object sender, EventArgs e)
        {
            if (reportTable == null || reportTable.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Please generate a report before exporting.",
                    "No Report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "CSV Files (*.csv)|*.csv";
                    dialog.FileName = "eShift_Report_" +
                        DateTime.Now.ToString("yyyyMMdd");

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    using (StreamWriter writer =
                        new StreamWriter(
                            dialog.FileName,
                            false,
                            System.Text.Encoding.UTF8))
                    {
                        for (int i = 0; i < reportTable.Columns.Count; i++)
                        {
                            writer.Write(
                                "\"" +
                                reportTable.Columns[i].ColumnName
                                .Replace("\"", "\"\"") + "\"");

                            if (i < reportTable.Columns.Count - 1)
                                writer.Write(",");
                        }

                        writer.WriteLine();

                        foreach (DataRow row in reportTable.Rows)
                        {
                            for (int i = 0; i < reportTable.Columns.Count; i++)
                            {
                                string value = row[i].ToString()
                                    .Replace("\"", "\"\"");

                                writer.Write("\"" + value + "\"");

                                if (i < reportTable.Columns.Count - 1)
                                    writer.Write(",");
                            }

                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show(
                        "Report exported successfully!",
                        "Export Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to export report.\n\n" + ex.Message,
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // SIDEBAR NAVIGATION
        // ==========================================

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
            OpenChildForm(new FrmTransportUnitManagement());
        }

        private void btnManageJobAssignments_Click(
            object sender, EventArgs e)
        {
            MessageBox.Show(
                "Job Assignments page has not been connected.",
                "Job Assignments",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            // Already on the Reports page.
            // Do not generate another report or show a message.
        }

        // ==========================================
        // LOGOUT
        // ==========================================

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

        // ==========================================
        // EXISTING DESIGNER EVENT
        // ==========================================

        private void lblReportsSubtitle_Click(object sender, EventArgs e)
        {
        }

        private void PnlDashboard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

