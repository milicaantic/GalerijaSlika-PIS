using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for frmKorisnik.xaml
    /// </summary>
    public partial class frmKorisnik : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? korisnikID;
        public frmKorisnik(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            korisnikID = id;
            if(korisnikID.HasValue)
            {
                UcitajPodatkeKorisnika();
            }
            else
            {
                txtIme.Focus();
            }
            
        }
        private void UcitajPodatkeKorisnika()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Korisnik WHERE korisnikID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", korisnikID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtIme.Text = reader["ime"].ToString();
                    txtPrezime.Text = reader["prezime"].ToString();
                    txtKorisnickoIme.Text = reader["korisnickoIme"].ToString();
                    txtLozinka.Text = reader["lozinka"].ToString();
                    txtTipKorisnika.Text = reader["tipKorisnika"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka korisnika: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        }
        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text) || string.IsNullOrWhiteSpace(txtPrezime.Text) || string.IsNullOrWhiteSpace(txtKorisnickoIme.Text)
                || string.IsNullOrWhiteSpace(txtLozinka.Text) || string.IsNullOrWhiteSpace(txtTipKorisnika.Text))
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
                if (korisnikID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Korisnik 
                                        SET ime = @ime, prezime = @prezime, korisnickoIme = @korisnickoIme, 
                                            lozinka = @lozinka, tipKorisnika = @tipKorisnika 
                                        WHERE korisnikID = @id";
                    cmd.Parameters.AddWithValue("@id", korisnikID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Korisnik(ime,prezime,korisnickoIme,lozinka,tipKorisnika)
                                values(@ime,@prezime,@korisnickoIme,@lozinka,@tipKorisnika)";
                }
                cmd.Parameters.Add("@ime", SqlDbType.NChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@korisnickoIme", SqlDbType.NChar).Value = txtKorisnickoIme.Text;
                cmd.Parameters.Add("@lozinka", SqlDbType.NChar).Value = txtLozinka.Text;
                cmd.Parameters.Add("@tipKorisnika", SqlDbType.NChar).Value = txtTipKorisnika.Text;
                
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
