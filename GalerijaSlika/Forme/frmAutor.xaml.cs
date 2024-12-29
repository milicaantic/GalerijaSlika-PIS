using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GalerijaSlika.Forme
{
    /// <summary>
    /// Interaction logic for frmAutor.xaml
    /// </summary>
    public partial class frmAutor : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? autorID; 
        public frmAutor(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            autorID = id;
            if (autorID.HasValue)
            {
                UcitajPodatkeAutora();
            }
            else
            {
                txtIme.Focus();
            }
        }
        private void UcitajPodatkeAutora()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Autor WHERE autorID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", autorID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtIme.Text = reader["ime"].ToString();
                    txtPrezime.Text = reader["prezime"].ToString();
                    txtBiografija.Text = reader["biografija"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka autora: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        }
        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text) ||string.IsNullOrWhiteSpace(txtPrezime.Text) || string.IsNullOrWhiteSpace(txtBiografija.Text))
            {
                MessageBox.Show("Sva polja moraju biti popunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                if (autorID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Autor 
                                        SET ime = @ime, prezime = @prezime, biografija = @biografija 
                                        WHERE autorID = @id";
                    cmd.Parameters.AddWithValue("@id", autorID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Autor(ime,prezime,biografija)
                                values(@ime,@prezime,@biografija)";
                }
                cmd.Parameters.Add("@ime", SqlDbType.NChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@biografija", SqlDbType.NChar).Value = txtBiografija.Text;
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
           
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
