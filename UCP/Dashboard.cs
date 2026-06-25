using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UCP
{
    public partial class Dashboard : Form
    {
        DAL dbLogic = new DAL();
        DataTable dt;
        bool isInitializing = true;
        DataTable dtPetani;

        private string hakAksesAktif = "";
        private string namaPetaniAktif = "";
        public Dashboard()
        {
            InitializeComponent();
            hakAksesAktif = "Admin";
        }
        public Dashboard(string namaPetani)
        {
            InitializeComponent();
            hakAksesAktif = "Petani";
            namaPetaniAktif = namaPetani;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            dtpTahun.Format = DateTimePickerFormat.Custom;
            dtpTahun.CustomFormat = "yyyy";
            dtpTahun.ShowUpDown = true;
            dtpTahun.MaxDate = DateTime.Now;

            cmbNama.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipe.DropDownStyle = ComboBoxStyle.DropDownList;

            var itemsChart = new List<KeyValuePair<string, SeriesChartType>>();
            itemsChart.Add(new KeyValuePair<string, SeriesChartType>("Kolom", SeriesChartType.Column));
            itemsChart.Add(new KeyValuePair<string, SeriesChartType>("Pie", SeriesChartType.Pie));

            cmbTipe.DataSource = itemsChart;
            cmbTipe.DisplayMember = "Key";
            cmbTipe.ValueMember = "Value";
            cmbTipe.SelectedIndex = 0;

            try
            {
                dtPetani = dbLogic.GetNamaPetani();

                if (hakAksesAktif == "Petani")
                {
                    cmbNama.DataSource = dtPetani;
                    cmbNama.DisplayMember = "nama_petani";
                    cmbNama.ValueMember = "nama_petani";
                    cmbNama.SelectedValue = namaPetaniAktif;
                    cmbNama.Enabled = false;
                }
                else if (hakAksesAktif == "Admin")
                {
                    DataRow row = dtPetani.NewRow();
                    row["nama_petani"] = "Semua";
                    dtPetani.Rows.InsertAt(row, 0);

                    cmbNama.DataSource = dtPetani;
                    cmbNama.DisplayMember = "nama_petani";
                    cmbNama.ValueMember = "nama_petani";
                    cmbNama.SelectedIndex = 0;
                    cmbNama.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat filter nama petani: " + ex.Message);
            }
            isInitializing = false;
            LoadDataChart();
        }
        public void LoadDataChart()
        {
            if (isInitializing)
                return;

            if (cmbNama.SelectedValue == null || cmbTipe.SelectedValue == null)
                return;

            chartPanen.Series.Clear();
            chartPanen.Titles.Clear();
            chartPanen.Legends.Clear();
            chartPanen.ChartAreas.Clear();

            ChartArea ca = new ChartArea("MainArea");
            ca.AxisX.Title = (cmbNama.SelectedValue.ToString() == "Semua") ? "Nama Petani" : "Jenis Tanaman";
            ca.AxisY.Title = "Total Kuantitas Panen (Kg)";
            ca.BackColor = Color.Transparent;
            chartPanen.ChartAreas.Add(ca);

            try
            {
                dt = dbLogic.GetDataRekap(cmbNama.SelectedValue.ToString(), dtpTahun.Value.Year.ToString());

                SeriesChartType tipeTerpilih = (SeriesChartType)cmbTipe.SelectedValue;
                Series s = new Series("HasilPanen");
                s.ChartType = tipeTerpilih;

                if (tipeTerpilih == SeriesChartType.Pie)
                {
                    s.IsValueShownAsLabel = true;
                    s.Label = "#VAL Kg";
                    s.LegendText = "#VALX";
                }

                foreach (DataRow row in dt.Rows)
                {
                    string sumbuX = "";
                    if (cmbNama.SelectedValue != null && cmbNama.SelectedValue.ToString() == "Semua")
                    {
                        sumbuX = row.Table.Columns.Contains("NamaPetani") ? row["NamaPetani"].ToString() : row[1].ToString();
                    }
                    else
                    {
                        // Jika kolom "NamaTanaman" tidak terbaca, ia otomatis mengambil data dari indeks kolom ke-2 (Nama Tanaman)
                        sumbuX = row.Table.Columns.Contains("NamaTanaman") ? row["NamaTanaman"].ToString() : row[2].ToString();
                    }

                    // Mengamankan pembacaan kuantitas dari indeks kolom ke-4 (JumlahHasil / jumlah_hasil)
                    int kuantitas = 0;
                    if (row.Table.Columns.Contains("JumlahHasil"))
                    {
                        kuantitas = Convert.ToInt32(row["JumlahHasil"]);
                    }
                    else
                    {
                        kuantitas = Convert.ToInt32(row[4]); // Ambil berdasarkan urutan kolom ke-4 di select SP
                    }

                    s.Points.AddXY(sumbuX, kuantitas);
                }

                chartPanen.Series.Add(s);

                Title title = new Title("Grafik Analisis Hasil Panen Tahun " + dtpTahun.Value.Year, Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.DarkBlue);
                chartPanen.Titles.Add(title);

                Legend legend = new Legend("MainLegend");
                legend.Docking = Docking.Right;
                chartPanen.Legends.Add(legend);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data grafik: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadDataChart();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNama.DataSource != null)
            {
                LoadDataChart();
            }
        }

        private void cmbTipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipe.SelectedValue is SeriesChartType)
            {
                LoadDataChart();
            }
        }
    }
}
