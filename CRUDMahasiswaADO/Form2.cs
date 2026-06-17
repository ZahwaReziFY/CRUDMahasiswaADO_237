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

namespace CRUDMahasiswaADO
{
    public partial class FormRekapData : Form
    {
        DAL dbLogic = new DAL();

        static string connectionString = "Data Source=WAWAAA\\ZAHWA;Initial Catalog=DBAkademikADO;User ID=sa;Password=Zahwarzfy04";

        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public FormRekapData()
        {
            InitializeComponent();
        }

        private void FormRekapData_Load(object sender, EventArgs e)
        {
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;
            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.MaxDate = DateTime.Now;

            cmbProdi.DropDownStyle = ComboBoxStyle.DropDownList;

            btnCetak.Enabled = false;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT namaprodi FROM programstudi", conn);
                dtProdi = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtProdi);

                cmbProdi.DataSource = dtProdi;
                cmbProdi.DisplayMember = "namaprodi";
                cmbProdi.ValueMember = "namaprodi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load daftar prodi: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            string prodiDipilih = cmbProdi.SelectedValue.ToString();
            DateTime tanggalDipilih = dtpTanggalMasuk.Value;

            Form3 frmCetak = new Form3(prodiDipilih, tanggalDipilih);
            frmCetak.Show();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (cmbProdi.SelectedValue == null)
            {
                MessageBox.Show("Silakan pilih Program Studi terlebih dahulu!");
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = cmbProdi.SelectedValue.ToString();
                cmd.Parameters.Add("@inTglMsuk", SqlDbType.VarChar, 4).Value = dtpTanggalMasuk.Value.Year.ToString();

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                dataGridView1.DataSource = dtMahasiswa;

                if (dtMahasiswa.Rows.Count > 0)
                {
                    btnCetak.Enabled = true;
                }
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}