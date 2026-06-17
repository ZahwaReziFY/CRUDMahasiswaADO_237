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
using CrystalDecisions.CrystalReports.Engine;

namespace CRUDMahasiswaADO
{
    public partial class Form3 : Form
    {
        DAL dbLogic = new DAL();

        List<ListMahasiswa> listMahasiswa = new List<ListMahasiswa>();

        string prodi { get; set; }
        DateTime tglmasuk { get; set; }

        public Form3(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();

            prodi = Prodi;
            tglmasuk = TglMasuk;

            try
            {
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);

                foreach (DataRow row in dtMahasiswa.Rows)
                {
                    ListMahasiswa mhs = new ListMahasiswa();
                    mhs.Nama = row["Nama"].ToString();
                    mhs.JenisKelamin = row["JenisKelamin"].ToString();
                    mhs.Alamat = row["Alamat"].ToString();
                    mhs.NamaProdi = row["NamaProdi"].ToString();
                    mhs.TanggalDaftar = Convert.ToDateTime(row["TanggalDaftar"]);
                    listMahasiswa.Add(mhs);
                }

                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(@"D:\PABD\P6\CRUDMahasiswaADO\CRUDMahasiswaADO\CrystalReport1.rpt");
                cryRpt.SetDataSource(listMahasiswa);

                crystalReportViewer1.ReportSource = cryRpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }
    }
}