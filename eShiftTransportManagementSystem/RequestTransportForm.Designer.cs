namespace eShiftTransportManagementSystem
{
    partial class RequestTransportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblBetterTomorrow = new System.Windows.Forms.Label();
            this.lblMoveToday = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.PictureBox();
            this.lblTagline = new System.Windows.Forms.Label();
            this.lblBrand = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlLoads = new System.Windows.Forms.Panel();
            this.dgvAddedLoads = new System.Windows.Forms.DataGridView();
            this.colLoadName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeightSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblAddedLoads = new System.Windows.Forms.Label();
            this.picAddedLoadsicon = new System.Windows.Forms.PictureBox();
            this.btnClearRequest = new System.Windows.Forms.Button();
            this.btnSubmitRequest = new System.Windows.Forms.Button();
            this.btnAddLoad = new System.Windows.Forms.Button();
            this.pnlRequestDetails = new System.Windows.Forms.Panel();
            this.txtEstimatedWeightSize = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtSpecialInstructions = new System.Windows.Forms.TextBox();
            this.lblSpecialInstructions = new System.Windows.Forms.Label();
            this.lblEstimatedWeightSize = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtCustomerNumber = new System.Windows.Forms.TextBox();
            this.lblProductLoadName = new System.Windows.Forms.Label();
            this.cmbPreferredTime = new System.Windows.Forms.ComboBox();
            this.lblPreferredTime = new System.Windows.Forms.Label();
            this.dtpRequestedDate = new System.Windows.Forms.DateTimePicker();
            this.lblRequestedDate = new System.Windows.Forms.Label();
            this.cmbCustomerNumber = new System.Windows.Forms.ComboBox();
            this.txtDestinationLocation = new System.Windows.Forms.TextBox();
            this.lblDestinationLocation = new System.Windows.Forms.Label();
            this.txtPickupLocation = new System.Windows.Forms.TextBox();
            this.lblPickupLocation = new System.Windows.Forms.Label();
            this.lblCustomerNumber = new System.Windows.Forms.Label();
            this.txtJobNumber = new System.Windows.Forms.TextBox();
            this.lblJobNumber = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblRequestTransportTitle = new System.Windows.Forms.Label();
            this.picleft = new System.Windows.Forms.PictureBox();
            this.btnCustomerLogout = new System.Windows.Forms.Button();
            this.btnMyRequests = new System.Windows.Forms.Button();
            this.btnMyPayments = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.btnMyJobs = new System.Windows.Forms.Button();
            this.btnCreateRequest = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.picMovingTruck = new System.Windows.Forms.PictureBox();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblLogo)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlLoads.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddedLoads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddedLoadsicon)).BeginInit();
            this.pnlRequestDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picleft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMovingTruck)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(57)))), ((int)(((byte)(99)))));
            this.pnlHeader.Controls.Add(this.lblBetterTomorrow);
            this.pnlHeader.Controls.Add(this.lblMoveToday);
            this.pnlHeader.Controls.Add(this.lblLogo);
            this.pnlHeader.Controls.Add(this.lblTagline);
            this.pnlHeader.Controls.Add(this.lblBrand);
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1112, 58);
            this.pnlHeader.TabIndex = 3;
            // 
            // lblBetterTomorrow
            // 
            this.lblBetterTomorrow.AutoSize = true;
            this.lblBetterTomorrow.BackColor = System.Drawing.Color.Transparent;
            this.lblBetterTomorrow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBetterTomorrow.ForeColor = System.Drawing.Color.White;
            this.lblBetterTomorrow.Location = new System.Drawing.Point(981, 22);
            this.lblBetterTomorrow.Name = "lblBetterTomorrow";
            this.lblBetterTomorrow.Size = new System.Drawing.Size(106, 15);
            this.lblBetterTomorrow.TabIndex = 4;
            this.lblBetterTomorrow.Text = "A Better Tomorrow";
            this.lblBetterTomorrow.Click += new System.EventHandler(this.lblBetterTomorrow_Click);
            // 
            // lblMoveToday
            // 
            this.lblMoveToday.AutoSize = true;
            this.lblMoveToday.BackColor = System.Drawing.Color.Transparent;
            this.lblMoveToday.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoveToday.ForeColor = System.Drawing.Color.White;
            this.lblMoveToday.Location = new System.Drawing.Point(886, 22);
            this.lblMoveToday.Name = "lblMoveToday";
            this.lblMoveToday.Size = new System.Drawing.Size(89, 15);
            this.lblMoveToday.TabIndex = 3;
            this.lblMoveToday.Text = "|  Move Today  |";
            // 
            // lblLogo
            // 
            this.lblLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblLogo.Image = global::eShiftTransportManagementSystem.Properties.Resources.logo_truck_png;
            this.lblLogo.Location = new System.Drawing.Point(12, 3);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(80, 48);
            this.lblLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.lblLogo.TabIndex = 0;
            this.lblLogo.TabStop = false;
            // 
            // lblTagline
            // 
            this.lblTagline.AutoSize = true;
            this.lblTagline.BackColor = System.Drawing.Color.Transparent;
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTagline.ForeColor = System.Drawing.Color.White;
            this.lblTagline.Location = new System.Drawing.Point(213, 18);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(230, 17);
            this.lblTagline.TabIndex = 2;
            this.lblTagline.Text = "Transportation Management System";
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.BackColor = System.Drawing.Color.Transparent;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Location = new System.Drawing.Point(87, 11);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(76, 28);
            this.lblBrand.TabIndex = 1;
            this.lblBrand.Text = "e-Shift";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlMain.Controls.Add(this.pnlLoads);
            this.pnlMain.Controls.Add(this.btnClearRequest);
            this.pnlMain.Controls.Add(this.btnSubmitRequest);
            this.pnlMain.Controls.Add(this.btnAddLoad);
            this.pnlMain.Controls.Add(this.pnlRequestDetails);
            this.pnlMain.Controls.Add(this.lblSubtitle);
            this.pnlMain.Controls.Add(this.lblRequestTransportTitle);
            this.pnlMain.Location = new System.Drawing.Point(234, 76);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(863, 591);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            // 
            // pnlLoads
            // 
            this.pnlLoads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoads.Controls.Add(this.dgvAddedLoads);
            this.pnlLoads.Controls.Add(this.lblAddedLoads);
            this.pnlLoads.Controls.Add(this.picAddedLoadsicon);
            this.pnlLoads.Location = new System.Drawing.Point(21, 381);
            this.pnlLoads.Name = "pnlLoads";
            this.pnlLoads.Size = new System.Drawing.Size(822, 194);
            this.pnlLoads.TabIndex = 49;
            this.pnlLoads.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLoads_Paint);
            // 
            // dgvAddedLoads
            // 
            this.dgvAddedLoads.AllowUserToAddRows = false;
            this.dgvAddedLoads.AllowUserToDeleteRows = false;
            this.dgvAddedLoads.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAddedLoads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAddedLoads.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLoadName,
            this.colQuantity,
            this.colWeightSize,
            this.colAction});
            this.dgvAddedLoads.EnableHeadersVisualStyles = false;
            this.dgvAddedLoads.Location = new System.Drawing.Point(13, 48);
            this.dgvAddedLoads.MultiSelect = false;
            this.dgvAddedLoads.Name = "dgvAddedLoads";
            this.dgvAddedLoads.ReadOnly = true;
            this.dgvAddedLoads.RowHeadersVisible = false;
            this.dgvAddedLoads.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAddedLoads.Size = new System.Drawing.Size(789, 131);
            this.dgvAddedLoads.TabIndex = 28;
            this.dgvAddedLoads.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAddedLoads_CellContentClick);
            // 
            // colLoadName
            // 
            this.colLoadName.HeaderText = "Load Name";
            this.colLoadName.Name = "colLoadName";
            this.colLoadName.ReadOnly = true;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            // 
            // colWeightSize
            // 
            this.colWeightSize.HeaderText = "Weight / Size";
            this.colWeightSize.Name = "colWeightSize";
            this.colWeightSize.ReadOnly = true;
            // 
            // colAction
            // 
            this.colAction.HeaderText = "Action";
            this.colAction.Name = "colAction";
            this.colAction.ReadOnly = true;
            // 
            // lblAddedLoads
            // 
            this.lblAddedLoads.AutoSize = true;
            this.lblAddedLoads.BackColor = System.Drawing.Color.Transparent;
            this.lblAddedLoads.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddedLoads.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblAddedLoads.Location = new System.Drawing.Point(47, 25);
            this.lblAddedLoads.Name = "lblAddedLoads";
            this.lblAddedLoads.Size = new System.Drawing.Size(88, 17);
            this.lblAddedLoads.TabIndex = 27;
            this.lblAddedLoads.Text = "Added Loads";
            // 
            // picAddedLoadsicon
            // 
            this.picAddedLoadsicon.BackColor = System.Drawing.Color.Transparent;
            this.picAddedLoadsicon.Image = global::eShiftTransportManagementSystem.Properties.Resources.T_Requesticon_png;
            this.picAddedLoadsicon.Location = new System.Drawing.Point(17, 20);
            this.picAddedLoadsicon.Name = "picAddedLoadsicon";
            this.picAddedLoadsicon.Size = new System.Drawing.Size(24, 22);
            this.picAddedLoadsicon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAddedLoadsicon.TabIndex = 26;
            this.picAddedLoadsicon.TabStop = false;
            // 
            // btnClearRequest
            // 
            this.btnClearRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(241)))), ((int)(((byte)(246)))));
            this.btnClearRequest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearRequest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearRequest.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearRequest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.btnClearRequest.Location = new System.Drawing.Point(609, 315);
            this.btnClearRequest.Name = "btnClearRequest";
            this.btnClearRequest.Size = new System.Drawing.Size(188, 35);
            this.btnClearRequest.TabIndex = 48;
            this.btnClearRequest.Text = "Clear";
            this.btnClearRequest.UseVisualStyleBackColor = false;
            // 
            // btnSubmitRequest
            // 
            this.btnSubmitRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(88)))), ((int)(((byte)(214)))));
            this.btnSubmitRequest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmitRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmitRequest.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmitRequest.ForeColor = System.Drawing.Color.White;
            this.btnSubmitRequest.Location = new System.Drawing.Point(400, 315);
            this.btnSubmitRequest.Name = "btnSubmitRequest";
            this.btnSubmitRequest.Size = new System.Drawing.Size(188, 35);
            this.btnSubmitRequest.TabIndex = 46;
            this.btnSubmitRequest.Text = "Submit Request";
            this.btnSubmitRequest.UseVisualStyleBackColor = false;
            // 
            // btnAddLoad
            // 
            this.btnAddLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.btnAddLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLoad.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddLoad.ForeColor = System.Drawing.Color.White;
            this.btnAddLoad.Image = global::eShiftTransportManagementSystem.Properties.Resources.ADDBUTTONICON_png;
            this.btnAddLoad.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnAddLoad.Location = new System.Drawing.Point(21, 315);
            this.btnAddLoad.Name = "btnAddLoad";
            this.btnAddLoad.Size = new System.Drawing.Size(188, 35);
            this.btnAddLoad.TabIndex = 45;
            this.btnAddLoad.Text = "Add Load";
            this.btnAddLoad.UseVisualStyleBackColor = false;
            // 
            // pnlRequestDetails
            // 
            this.pnlRequestDetails.Controls.Add(this.txtEstimatedWeightSize);
            this.pnlRequestDetails.Controls.Add(this.txtQuantity);
            this.pnlRequestDetails.Controls.Add(this.txtSpecialInstructions);
            this.pnlRequestDetails.Controls.Add(this.lblSpecialInstructions);
            this.pnlRequestDetails.Controls.Add(this.lblEstimatedWeightSize);
            this.pnlRequestDetails.Controls.Add(this.lblQuantity);
            this.pnlRequestDetails.Controls.Add(this.txtCustomerNumber);
            this.pnlRequestDetails.Controls.Add(this.lblProductLoadName);
            this.pnlRequestDetails.Controls.Add(this.cmbPreferredTime);
            this.pnlRequestDetails.Controls.Add(this.lblPreferredTime);
            this.pnlRequestDetails.Controls.Add(this.dtpRequestedDate);
            this.pnlRequestDetails.Controls.Add(this.lblRequestedDate);
            this.pnlRequestDetails.Controls.Add(this.cmbCustomerNumber);
            this.pnlRequestDetails.Controls.Add(this.txtDestinationLocation);
            this.pnlRequestDetails.Controls.Add(this.lblDestinationLocation);
            this.pnlRequestDetails.Controls.Add(this.txtPickupLocation);
            this.pnlRequestDetails.Controls.Add(this.lblPickupLocation);
            this.pnlRequestDetails.Controls.Add(this.lblCustomerNumber);
            this.pnlRequestDetails.Controls.Add(this.txtJobNumber);
            this.pnlRequestDetails.Controls.Add(this.lblJobNumber);
            this.pnlRequestDetails.Location = new System.Drawing.Point(21, 65);
            this.pnlRequestDetails.Name = "pnlRequestDetails";
            this.pnlRequestDetails.Size = new System.Drawing.Size(822, 234);
            this.pnlRequestDetails.TabIndex = 44;
            // 
            // txtEstimatedWeightSize
            // 
            this.txtEstimatedWeightSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEstimatedWeightSize.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEstimatedWeightSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtEstimatedWeightSize.Location = new System.Drawing.Point(576, 182);
            this.txtEstimatedWeightSize.Name = "txtEstimatedWeightSize";
            this.txtEstimatedWeightSize.Size = new System.Drawing.Size(200, 22);
            this.txtEstimatedWeightSize.TabIndex = 48;
            // 
            // txtQuantity
            // 
            this.txtQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantity.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtQuantity.Location = new System.Drawing.Point(576, 139);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(200, 22);
            this.txtQuantity.TabIndex = 47;
            // 
            // txtSpecialInstructions
            // 
            this.txtSpecialInstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSpecialInstructions.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpecialInstructions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtSpecialInstructions.Location = new System.Drawing.Point(146, 184);
            this.txtSpecialInstructions.Multiline = true;
            this.txtSpecialInstructions.Name = "txtSpecialInstructions";
            this.txtSpecialInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSpecialInstructions.Size = new System.Drawing.Size(200, 20);
            this.txtSpecialInstructions.TabIndex = 46;
            // 
            // lblSpecialInstructions
            // 
            this.lblSpecialInstructions.AutoSize = true;
            this.lblSpecialInstructions.BackColor = System.Drawing.Color.Transparent;
            this.lblSpecialInstructions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpecialInstructions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblSpecialInstructions.Location = new System.Drawing.Point(15, 191);
            this.lblSpecialInstructions.Name = "lblSpecialInstructions";
            this.lblSpecialInstructions.Size = new System.Drawing.Size(115, 15);
            this.lblSpecialInstructions.TabIndex = 45;
            this.lblSpecialInstructions.Text = "Special Instructions";
            // 
            // lblEstimatedWeightSize
            // 
            this.lblEstimatedWeightSize.AutoSize = true;
            this.lblEstimatedWeightSize.BackColor = System.Drawing.Color.Transparent;
            this.lblEstimatedWeightSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstimatedWeightSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblEstimatedWeightSize.Location = new System.Drawing.Point(427, 191);
            this.lblEstimatedWeightSize.Name = "lblEstimatedWeightSize";
            this.lblEstimatedWeightSize.Size = new System.Drawing.Size(140, 15);
            this.lblEstimatedWeightSize.TabIndex = 44;
            this.lblEstimatedWeightSize.Text = "Estimated Weight / Size";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.BackColor = System.Drawing.Color.Transparent;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblQuantity.Location = new System.Drawing.Point(427, 146);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(55, 15);
            this.lblQuantity.TabIndex = 43;
            this.lblQuantity.Text = "Quantity";
            // 
            // txtCustomerNumber
            // 
            this.txtCustomerNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomerNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtCustomerNumber.Location = new System.Drawing.Point(576, 99);
            this.txtCustomerNumber.Name = "txtCustomerNumber";
            this.txtCustomerNumber.Size = new System.Drawing.Size(200, 22);
            this.txtCustomerNumber.TabIndex = 42;
            // 
            // lblProductLoadName
            // 
            this.lblProductLoadName.AutoSize = true;
            this.lblProductLoadName.BackColor = System.Drawing.Color.Transparent;
            this.lblProductLoadName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductLoadName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblProductLoadName.Location = new System.Drawing.Point(427, 106);
            this.lblProductLoadName.Name = "lblProductLoadName";
            this.lblProductLoadName.Size = new System.Drawing.Size(124, 15);
            this.lblProductLoadName.TabIndex = 41;
            this.lblProductLoadName.Text = "Product / Load Name";
            // 
            // cmbPreferredTime
            // 
            this.cmbPreferredTime.FormattingEnabled = true;
            this.cmbPreferredTime.Location = new System.Drawing.Point(576, 53);
            this.cmbPreferredTime.Name = "cmbPreferredTime";
            this.cmbPreferredTime.Size = new System.Drawing.Size(200, 21);
            this.cmbPreferredTime.TabIndex = 40;
            // 
            // lblPreferredTime
            // 
            this.lblPreferredTime.AutoSize = true;
            this.lblPreferredTime.BackColor = System.Drawing.Color.Transparent;
            this.lblPreferredTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreferredTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblPreferredTime.Location = new System.Drawing.Point(427, 59);
            this.lblPreferredTime.Name = "lblPreferredTime";
            this.lblPreferredTime.Size = new System.Drawing.Size(93, 15);
            this.lblPreferredTime.TabIndex = 39;
            this.lblPreferredTime.Text = "Preferred Time";
            // 
            // dtpRequestedDate
            // 
            this.dtpRequestedDate.Location = new System.Drawing.Point(576, 11);
            this.dtpRequestedDate.Name = "dtpRequestedDate";
            this.dtpRequestedDate.Size = new System.Drawing.Size(200, 20);
            this.dtpRequestedDate.TabIndex = 38;
            // 
            // lblRequestedDate
            // 
            this.lblRequestedDate.AutoSize = true;
            this.lblRequestedDate.BackColor = System.Drawing.Color.Transparent;
            this.lblRequestedDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestedDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblRequestedDate.Location = new System.Drawing.Point(427, 16);
            this.lblRequestedDate.Name = "lblRequestedDate";
            this.lblRequestedDate.Size = new System.Drawing.Size(97, 15);
            this.lblRequestedDate.TabIndex = 37;
            this.lblRequestedDate.Text = "Requested Date";
            // 
            // cmbCustomerNumber
            // 
            this.cmbCustomerNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerNumber.FormattingEnabled = true;
            this.cmbCustomerNumber.Location = new System.Drawing.Point(146, 53);
            this.cmbCustomerNumber.Name = "cmbCustomerNumber";
            this.cmbCustomerNumber.Size = new System.Drawing.Size(210, 21);
            this.cmbCustomerNumber.TabIndex = 36;
            // 
            // txtDestinationLocation
            // 
            this.txtDestinationLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDestinationLocation.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDestinationLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtDestinationLocation.Location = new System.Drawing.Point(146, 139);
            this.txtDestinationLocation.Multiline = true;
            this.txtDestinationLocation.Name = "txtDestinationLocation";
            this.txtDestinationLocation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDestinationLocation.Size = new System.Drawing.Size(210, 20);
            this.txtDestinationLocation.TabIndex = 35;
            // 
            // lblDestinationLocation
            // 
            this.lblDestinationLocation.AutoSize = true;
            this.lblDestinationLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblDestinationLocation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestinationLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblDestinationLocation.Location = new System.Drawing.Point(15, 146);
            this.lblDestinationLocation.Name = "lblDestinationLocation";
            this.lblDestinationLocation.Size = new System.Drawing.Size(121, 15);
            this.lblDestinationLocation.TabIndex = 34;
            this.lblDestinationLocation.Text = "Destination Location";
            // 
            // txtPickupLocation
            // 
            this.txtPickupLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPickupLocation.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPickupLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtPickupLocation.Location = new System.Drawing.Point(146, 96);
            this.txtPickupLocation.Multiline = true;
            this.txtPickupLocation.Name = "txtPickupLocation";
            this.txtPickupLocation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPickupLocation.Size = new System.Drawing.Size(210, 20);
            this.txtPickupLocation.TabIndex = 33;
            // 
            // lblPickupLocation
            // 
            this.lblPickupLocation.AutoSize = true;
            this.lblPickupLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblPickupLocation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPickupLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblPickupLocation.Location = new System.Drawing.Point(15, 97);
            this.lblPickupLocation.Name = "lblPickupLocation";
            this.lblPickupLocation.Size = new System.Drawing.Size(94, 15);
            this.lblPickupLocation.TabIndex = 32;
            this.lblPickupLocation.Text = "Pickup Location";
            // 
            // lblCustomerNumber
            // 
            this.lblCustomerNumber.AutoSize = true;
            this.lblCustomerNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerNumber.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblCustomerNumber.Location = new System.Drawing.Point(15, 55);
            this.lblCustomerNumber.Name = "lblCustomerNumber";
            this.lblCustomerNumber.Size = new System.Drawing.Size(110, 15);
            this.lblCustomerNumber.TabIndex = 30;
            this.lblCustomerNumber.Text = "Customer Number";
            // 
            // txtJobNumber
            // 
            this.txtJobNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJobNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJobNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtJobNumber.Location = new System.Drawing.Point(146, 15);
            this.txtJobNumber.Name = "txtJobNumber";
            this.txtJobNumber.ReadOnly = true;
            this.txtJobNumber.Size = new System.Drawing.Size(210, 22);
            this.txtJobNumber.TabIndex = 29;
            // 
            // lblJobNumber
            // 
            this.lblJobNumber.AutoSize = true;
            this.lblJobNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblJobNumber.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJobNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblJobNumber.Location = new System.Drawing.Point(15, 16);
            this.lblJobNumber.Name = "lblJobNumber";
            this.lblJobNumber.Size = new System.Drawing.Size(75, 15);
            this.lblJobNumber.TabIndex = 28;
            this.lblJobNumber.Text = "Job Number";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblSubtitle.Location = new System.Drawing.Point(18, 44);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(296, 15);
            this.lblSubtitle.TabIndex = 10;
            this.lblSubtitle.Text = "Household Goods Transportation Management System";
            // 
            // lblRequestTransportTitle
            // 
            this.lblRequestTransportTitle.AutoSize = true;
            this.lblRequestTransportTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRequestTransportTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestTransportTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.lblRequestTransportTitle.Location = new System.Drawing.Point(16, 14);
            this.lblRequestTransportTitle.Name = "lblRequestTransportTitle";
            this.lblRequestTransportTitle.Size = new System.Drawing.Size(259, 30);
            this.lblRequestTransportTitle.TabIndex = 9;
            this.lblRequestTransportTitle.Text = "Create Transport Request";
            // 
            // picleft
            // 
            this.picleft.BackColor = System.Drawing.Color.Transparent;
            this.picleft.Image = global::eShiftTransportManagementSystem.Properties.Resources.pic_left_png;
            this.picleft.Location = new System.Drawing.Point(12, 457);
            this.picleft.Name = "picleft";
            this.picleft.Size = new System.Drawing.Size(198, 221);
            this.picleft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picleft.TabIndex = 43;
            this.picleft.TabStop = false;
            // 
            // btnCustomerLogout
            // 
            this.btnCustomerLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.btnCustomerLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCustomerLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.btnCustomerLogout.Image = global::eShiftTransportManagementSystem.Properties.Resources.Logout_icon_png;
            this.btnCustomerLogout.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnCustomerLogout.Location = new System.Drawing.Point(12, 341);
            this.btnCustomerLogout.Name = "btnCustomerLogout";
            this.btnCustomerLogout.Size = new System.Drawing.Size(183, 34);
            this.btnCustomerLogout.TabIndex = 42;
            this.btnCustomerLogout.Text = "Logout";
            this.btnCustomerLogout.UseVisualStyleBackColor = false;
            // 
            // btnMyRequests
            // 
            this.btnMyRequests.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(84)))), ((int)(((byte)(129)))));
            this.btnMyRequests.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMyRequests.FlatAppearance.BorderSize = 0;
            this.btnMyRequests.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyRequests.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMyRequests.ForeColor = System.Drawing.Color.White;
            this.btnMyRequests.Image = global::eShiftTransportManagementSystem.Properties.Resources.CREATEREQUESTICON_png;
            this.btnMyRequests.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnMyRequests.Location = new System.Drawing.Point(28, 287);
            this.btnMyRequests.Name = "btnMyRequests";
            this.btnMyRequests.Size = new System.Drawing.Size(153, 34);
            this.btnMyRequests.TabIndex = 41;
            this.btnMyRequests.Text = "My Requests";
            this.btnMyRequests.UseVisualStyleBackColor = false;
            // 
            // btnMyPayments
            // 
            this.btnMyPayments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(84)))), ((int)(((byte)(129)))));
            this.btnMyPayments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMyPayments.FlatAppearance.BorderSize = 0;
            this.btnMyPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyPayments.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMyPayments.ForeColor = System.Drawing.Color.White;
            this.btnMyPayments.Image = global::eShiftTransportManagementSystem.Properties.Resources.MYPAYMENTSICON_png;
            this.btnMyPayments.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnMyPayments.Location = new System.Drawing.Point(28, 247);
            this.btnMyPayments.Name = "btnMyPayments";
            this.btnMyPayments.Size = new System.Drawing.Size(153, 34);
            this.btnMyPayments.TabIndex = 40;
            this.btnMyPayments.Text = "My Payments";
            this.btnMyPayments.UseVisualStyleBackColor = false;
            // 
            // btnProfile
            // 
            this.btnProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(84)))), ((int)(((byte)(129)))));
            this.btnProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProfile.FlatAppearance.BorderSize = 0;
            this.btnProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfile.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfile.ForeColor = System.Drawing.Color.White;
            this.btnProfile.Image = global::eShiftTransportManagementSystem.Properties.Resources.PROFILEICON_png;
            this.btnProfile.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnProfile.Location = new System.Drawing.Point(28, 207);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(153, 34);
            this.btnProfile.TabIndex = 14;
            this.btnProfile.Text = "Profile";
            this.btnProfile.UseVisualStyleBackColor = false;
            // 
            // btnMyJobs
            // 
            this.btnMyJobs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(84)))), ((int)(((byte)(129)))));
            this.btnMyJobs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMyJobs.FlatAppearance.BorderSize = 0;
            this.btnMyJobs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyJobs.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMyJobs.ForeColor = System.Drawing.Color.White;
            this.btnMyJobs.Image = global::eShiftTransportManagementSystem.Properties.Resources.JOBICON_png;
            this.btnMyJobs.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnMyJobs.Location = new System.Drawing.Point(28, 167);
            this.btnMyJobs.Name = "btnMyJobs";
            this.btnMyJobs.Size = new System.Drawing.Size(153, 34);
            this.btnMyJobs.TabIndex = 13;
            this.btnMyJobs.Text = "My Jobs";
            this.btnMyJobs.UseVisualStyleBackColor = false;
            // 
            // btnCreateRequest
            // 
            this.btnCreateRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(84)))), ((int)(((byte)(129)))));
            this.btnCreateRequest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateRequest.FlatAppearance.BorderSize = 0;
            this.btnCreateRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateRequest.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateRequest.ForeColor = System.Drawing.Color.White;
            this.btnCreateRequest.Image = global::eShiftTransportManagementSystem.Properties.Resources.CREATEREQUESTNEWICON_png;
            this.btnCreateRequest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateRequest.Location = new System.Drawing.Point(28, 127);
            this.btnCreateRequest.Name = "btnCreateRequest";
            this.btnCreateRequest.Size = new System.Drawing.Size(153, 34);
            this.btnCreateRequest.TabIndex = 12;
            this.btnCreateRequest.Text = "Create Request";
            this.btnCreateRequest.UseVisualStyleBackColor = false;
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(84)))), ((int)(((byte)(129)))));
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Image = global::eShiftTransportManagementSystem.Properties.Resources.home_icon;
            this.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnDashboard.Location = new System.Drawing.Point(28, 87);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(153, 34);
            this.btnDashboard.TabIndex = 11;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = false;
            // 
            // picMovingTruck
            // 
            this.picMovingTruck.BackColor = System.Drawing.Color.Transparent;
            this.picMovingTruck.Image = global::eShiftTransportManagementSystem.Properties.Resources.AdminDasboardpannel_png;
            this.picMovingTruck.Location = new System.Drawing.Point(0, 57);
            this.picMovingTruck.Name = "picMovingTruck";
            this.picMovingTruck.Size = new System.Drawing.Size(214, 683);
            this.picMovingTruck.TabIndex = 9;
            this.picMovingTruck.TabStop = false;
            // 
            // RequestTransportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 679);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.picleft);
            this.Controls.Add(this.btnCustomerLogout);
            this.Controls.Add(this.btnMyRequests);
            this.Controls.Add(this.btnMyPayments);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.btnMyJobs);
            this.Controls.Add(this.btnCreateRequest);
            this.Controls.Add(this.btnDashboard);
            this.Controls.Add(this.picMovingTruck);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RequestTransportForm";
            this.Text = "e-Shift - Transportation Management System";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblLogo)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlLoads.ResumeLayout(false);
            this.pnlLoads.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddedLoads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddedLoadsicon)).EndInit();
            this.pnlRequestDetails.ResumeLayout(false);
            this.pnlRequestDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picleft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMovingTruck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel pnlHeader;
        internal System.Windows.Forms.Label lblBetterTomorrow;
        internal System.Windows.Forms.Label lblMoveToday;
        internal System.Windows.Forms.PictureBox lblLogo;
        internal System.Windows.Forms.Label lblTagline;
        internal System.Windows.Forms.Label lblBrand;
        internal System.Windows.Forms.PictureBox picMovingTruck;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnCreateRequest;
        private System.Windows.Forms.Button btnMyJobs;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Button btnMyPayments;
        private System.Windows.Forms.Button btnMyRequests;
        private System.Windows.Forms.Button btnCustomerLogout;
        internal System.Windows.Forms.PictureBox picleft;
        private System.Windows.Forms.Panel pnlMain;
        internal System.Windows.Forms.Label lblRequestTransportTitle;
        internal System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlRequestDetails;
        internal System.Windows.Forms.Label lblJobNumber;
        internal System.Windows.Forms.Label lblCustomerNumber;
        private System.Windows.Forms.TextBox txtJobNumber;
        private System.Windows.Forms.TextBox txtDestinationLocation;
        internal System.Windows.Forms.Label lblDestinationLocation;
        private System.Windows.Forms.TextBox txtPickupLocation;
        internal System.Windows.Forms.Label lblPickupLocation;
        private System.Windows.Forms.ComboBox cmbCustomerNumber;
        internal System.Windows.Forms.Label lblRequestedDate;
        internal System.Windows.Forms.Label lblPreferredTime;
        private System.Windows.Forms.DateTimePicker dtpRequestedDate;
        internal System.Windows.Forms.Label lblProductLoadName;
        private System.Windows.Forms.ComboBox cmbPreferredTime;
        private System.Windows.Forms.TextBox txtCustomerNumber;
        internal System.Windows.Forms.Label lblEstimatedWeightSize;
        internal System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtSpecialInstructions;
        internal System.Windows.Forms.Label lblSpecialInstructions;
        private System.Windows.Forms.TextBox txtEstimatedWeightSize;
        private System.Windows.Forms.Button btnSubmitRequest;
        private System.Windows.Forms.Button btnAddLoad;
        private System.Windows.Forms.Button btnClearRequest;
        private System.Windows.Forms.Panel pnlLoads;
        private System.Windows.Forms.PictureBox picAddedLoadsicon;
        internal System.Windows.Forms.Label lblAddedLoads;
        private System.Windows.Forms.DataGridView dgvAddedLoads;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeightSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
    }
}