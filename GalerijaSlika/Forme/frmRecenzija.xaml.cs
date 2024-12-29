using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Interaction logic for frmRecenzija.xaml
    /// </summary>
    public partial class frmRecenzija : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? recenzijaID;
        public frmRecenzija(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            recenzijaID = id;
              UcitajPadajuceListe();
            if (recenzijaID.HasValue)
            {
                UcitajPodatkeRecenzije();
            }
            else
            {
                txtRecenzija.Focus();
            }
           
        }
        private void UcitajPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiKorisnike = @"select korisnikID,ime from tbl_Korisnik";
                SqlDataAdapter daKorisnika = new SqlDataAdapter(vratiKorisnike, konekcija);
                DataTable dtKorisnika = new DataTable();
                daKorisnika.Fill(dtKorisnika);
                cbKorisnik.ItemsSource = dtKorisnika.DefaultView;
                daKorisnika.Dispose();
                dtKorisnika.Dispose();


                string vratiIzlozbe = @"select izlozbaID,nazivIzlozbe from tbl_Izlozba";
                SqlDataAdapter daIzlozba = new SqlDataAdapter(vratiIzlozbe, konekcija);
                DataTable dtIzlozba = new DataTable();
                daIzlozba.Fill(dtIzlozba);
                cbIzlozba.ItemsSource = dtIzlozba.DefaultView;
                daIzlozba.Dispose();
                dtIzlozba.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Nije popunjena padajuca lista", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }
        private void UcitajPodatkeRecenzije()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Recenzija WHERE recenzijaID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", recenzijaID);
                SqlDataReader reader = cmd.ExecuteReader();
                    
                      if (reader.Read())
                        {
                            txtRecenzija.Text = reader["textRecenzije"].ToString();
                            txtOcena.Text = reader["ocena"].ToString();
                            dpDatum.SelectedDate = Convert.ToDateTime(reader["datum"]);
                            cbKorisnik.SelectedValue = reader["korisnikID"];
                            cbIzlozba.SelectedValue = reader["izlozbaID"];
                        }
                    
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja recenzije: ", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (string.IsNullOrWhiteSpace(txtRecenzija.Text) || string.IsNullOrWhiteSpace(txtOcena.Text) || dpDatum.SelectedDate == null ||
    cbIzlozba.SelectedValue == null || cbKorisnik.SelectedValue == null)
            {
                MessageBox.Show("Sva polja moraju biti popunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(!int.TryParse(txtOcena.Text, out _))
            {
                MessageBox.Show("Ocena mora biti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                konekcija.Open();
                DateTime date = (DateTime)dpDatum.SelectedDate;
                string datum = date.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);

                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };

                
                cmd.Parameters.Add("@textRecenzije", SqlDbType.NChar).Value = txtRecenzija.Text;
                cmd.Parameters.Add("@ocena", SqlDbType.Int).Value = txtOcena.Text;
                cmd.Parameters.Add("@datum", SqlDbType.Date).Value = datum;
                cmd.Parameters.Add("@korisnikID", SqlDbType.Int).Value = cbKorisnik.SelectedValue;
                cmd.Parameters.Add("@izlozbaID", SqlDbType.Int).Value = cbIzlozba.SelectedValue;
                if (recenzijaID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Recenzija 
                                        SET textRecenzije = @textRecenzije, ocena = @ocena, datum = @datum, korisnikID = @korisnikID, izlozbaID = @izlozbaID
                                    WHERE recenzijaID = @id";
                    cmd.Parameters.AddWithValue("@id", recenzijaID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Recenzija(textRecenzije,ocena,datum,korisnikID,izlozbaID)
                                values(@textRecenzije,@ocena,@datum,@korisnikID,@izlozbaID)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan."+ex.Message, "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Odaberite vreme!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
