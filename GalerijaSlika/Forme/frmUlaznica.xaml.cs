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
    /// Interaction logic for frmUlaznica.xaml
    /// </summary>
    public partial class frmUlaznica : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? ulaznicaID;
        public frmUlaznica(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            ulaznicaID = id;
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
            if (ulaznicaID.HasValue)
            {
                UcitajPodatkeUlaznice();
            }
            else
            {
                txtCena.Focus();
            }
        }
        private void UcitajPodatkeUlaznice()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Ulaznica WHERE ulaznicaID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", ulaznicaID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    
                    txtCena.Text = reader["cena"].ToString();
                    txtTipUlaznice.Text = reader["tipUlaznice"].ToString();
                    dpDatumKupovine.SelectedDate = (DateTime?)reader["datumKupovine"];
                    cbKorisnik.SelectedValue = reader["korisnikID"];
                    cbIzlozba.SelectedValue = reader["izlozbaID"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka ulaznice: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (string.IsNullOrWhiteSpace(txtCena.Text) || string.IsNullOrWhiteSpace(txtTipUlaznice.Text) || dpDatumKupovine.SelectedDate == null ||
    cbIzlozba.SelectedValue == null || cbKorisnik.SelectedValue == null)
            {
                MessageBox.Show("Sva polja moraju biti popunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtCena.Text, out _))
            {
                MessageBox.Show("Cena mora biti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
                {
                    konekcija.Open();
                    DateTime date = (DateTime)dpDatumKupovine.SelectedDate;
                    string datumKupovine = date.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);

                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };

                    cmd.Parameters.Add("@datumKupovine", SqlDbType.Date).Value = datumKupovine;
                    cmd.Parameters.Add("@cena", SqlDbType.Int).Value = txtCena.Text;
                    cmd.Parameters.Add("@tipUlaznice", SqlDbType.NChar).Value = txtTipUlaznice.Text;
                    cmd.Parameters.Add("@korisnikID", SqlDbType.Int).Value = cbKorisnik.SelectedValue;
                    cmd.Parameters.Add("@izlozbaID", SqlDbType.Int).Value = cbIzlozba.SelectedValue;
                if (ulaznicaID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Ulaznica 
                                        SET datumKupovine = @datumKupovine, cena = @cena, tipUlaznice = @tipUlaznice, 
                                        korisnikID = @korisnikID, izlozbaID = @izlozbaID 
                                        WHERE ulaznicaID = @id";
                    cmd.Parameters.AddWithValue("@id", ulaznicaID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Ulaznica(datumKupovine,cena,tipUlaznice,korisnikID,izlozbaID)
                                         values(@datumKupovine,@cena,@tipUlaznice,@korisnikID,@izlozbaID)";
                }
               
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    this.Close();
                }
                catch (SqlException ex)
                {
                MessageBox.Show("Unos odredjenih vrednosti nije validan." +ex.Message, "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
