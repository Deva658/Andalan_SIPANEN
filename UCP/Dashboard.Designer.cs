namespace UCP
{
    partial class Dashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dtpTahun = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbNama = new System.Windows.Forms.ComboBox();
            this.chartPanen = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.cmbTipe = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartPanen)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpTahun
            // 
            this.dtpTahun.CustomFormat = "yyyy";
            this.dtpTahun.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTahun.Location = new System.Drawing.Point(218, 81);
            this.dtpTahun.Name = "dtpTahun";
            this.dtpTahun.ShowUpDown = true;
            this.dtpTahun.Size = new System.Drawing.Size(67, 22);
            this.dtpTahun.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(264, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "REKAP HASIL PANEN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmbNama
            // 
            this.cmbNama.FormattingEnabled = true;
            this.cmbNama.Location = new System.Drawing.Point(82, 79);
            this.cmbNama.Name = "cmbNama";
            this.cmbNama.Size = new System.Drawing.Size(121, 24);
            this.cmbNama.TabIndex = 2;
            this.cmbNama.SelectedIndexChanged += new System.EventHandler(this.cmbNama_SelectedIndexChanged);
            // 
            // chartPanen
            // 
            chartArea5.Name = "ChartArea1";
            this.chartPanen.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartPanen.Legends.Add(legend5);
            this.chartPanen.Location = new System.Drawing.Point(82, 121);
            this.chartPanen.Name = "chartPanen";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chartPanen.Series.Add(series5);
            this.chartPanen.Size = new System.Drawing.Size(636, 317);
            this.chartPanen.TabIndex = 3;
            this.chartPanen.Text = "chart1";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(463, 77);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 26);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(544, 77);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 26);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // cmbTipe
            // 
            this.cmbTipe.FormattingEnabled = true;
            this.cmbTipe.Location = new System.Drawing.Point(638, 77);
            this.cmbTipe.Name = "cmbTipe";
            this.cmbTipe.Size = new System.Drawing.Size(80, 24);
            this.cmbTipe.TabIndex = 6;
            this.cmbTipe.SelectedIndexChanged += new System.EventHandler(this.cmbTipe_SelectedIndexChanged);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cmbTipe);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.chartPanen);
            this.Controls.Add(this.cmbNama);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpTahun);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartPanen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpTahun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbNama;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPanen;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ComboBox cmbTipe;
    }
}