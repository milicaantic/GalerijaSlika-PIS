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
    /// Interaction logic for frmKustos.xaml
    /// </summary>
    public partial class frmKustos : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? kustosID;
        public frmKustos(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            kustosID = id;
            if (kustosID.HasValue)
            {
                UcitajPodatkeKustosa();
            }
            else
            {
                txtIme.Focus();
            }

        }
        private void UcitajPodatkeKustosa()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Kustos WHERE kustosID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", kustosID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtIme.Text = reader["ime"].ToString();
                    txtPrezime.Text = reader["prezime"].ToString();
                    txtKontaktInformacije.Text = reader["kontaktInformacije"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka kustosa: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        }
        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text) || string.IsNullOrWhiteSpace(txtPrezime.Text) || string.IsNullOrWhiteSpace(txtKontaktInformacije.Text))
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
                if (kustosID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Kustos 
                                        SET ime = @ime, prezime = @prezime, kontaktInformacije = @kontaktInformacije 
                                        WHERE kustosID = @id";
                    cmd.Parameters.AddWithValue("@id", kustosID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Kustos(ime,prezime,kontaktInformacije)
                                values(@ime,@prezime,@kontaktInformacije)";
                }
                cmd.Parameters.Add("@ime", SqlDbType.NChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@kontaktInformacije", SqlDbType.NChar).Value = txtKontaktInformacije.Text;
                
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
    }
}
