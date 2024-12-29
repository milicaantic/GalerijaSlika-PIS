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
    /// Interaction logic for frmIzlozba.xaml
    /// </summary>
    public partial class frmIzlozba : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? izlozbaID;
        public frmIzlozba(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            izlozbaID = id;
            UcitajPadajuceListe();
            if (izlozbaID.HasValue)
            {
                UcitajPodatkeIzlozbe();
            }
            else
            {
                txtNazivIzlozbe.Focus();
            }
          
        }
        private void UcitajPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiKustose = @"select kustosID,ime from tbl_Kustos";
                SqlDataAdapter daKustosa = new SqlDataAdapter(vratiKustose, konekcija);
                DataTable dtKustosa = new DataTable();
                daKustosa.Fill(dtKustosa);
                cbKustos.ItemsSource = dtKustosa.DefaultView;
                daKustosa.Dispose();
                dtKustosa.Dispose();
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
         private void UcitajPodatkeIzlozbe()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Izlozba WHERE izlozbaID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", izlozbaID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    
                    txtNazivIzlozbe.Text = reader["nazivIzlozbe"].ToString();
                    dpDatumPocetka.SelectedDate = DateTime.Parse(reader["datumPocetka"].ToString());
                    dpDatumZavrsetka.SelectedDate = DateTime.Parse(reader["datumZavrsetka"].ToString());
                    txtOpis.Text = reader["opis"].ToString();
                    int kustosID = Convert.ToInt32(reader["kustosID"]);
                    cbKustos.SelectedValue = kustosID;
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
        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNazivIzlozbe.Text) || string.IsNullOrWhiteSpace(txtOpis.Text) || dpDatumPocetka.SelectedDate == null ||
    dpDatumZavrsetka.SelectedDate == null || cbKustos.SelectedValue == null )
            {
                MessageBox.Show("Sva polja moraju biti popunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                konekcija.Open();
                DateTime date1 = (DateTime)dpDatumPocetka.SelectedDate;
                string datumPocetka = date1.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
                DateTime date2 = (DateTime)dpDatumZavrsetka.SelectedDate;
                string datumZavrsetka = date2.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                if(izlozbaID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Izlozba 
                                        SET nazivIzlozbe = @nazivIzlozbe, datumPocetka = @datumPocetka, 
                                            datumZavrsetka = @datumZavrsetka, opis = @opis, kustosID = @kustosID 
                                        WHERE izlozbaID = @id";
                    cmd.Parameters.AddWithValue("@id", izlozbaID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Izlozba(nazivIzlozbe,datumPocetka,datumZavrsetka,opis,kustosID)
                                values(@nazivIzlozbe,@datumPocetka,@datumZavrsetka,@opis,@kustosID)";
                }
                cmd.Parameters.Add("@nazivIzlozbe", SqlDbType.NChar).Value = txtNazivIzlozbe.Text;
                cmd.Parameters.Add("@datumPocetka", SqlDbType.Date).Value = datumPocetka;
                cmd.Parameters.Add("@datumZavrsetka", SqlDbType.Date).Value = datumZavrsetka;
                cmd.Parameters.Add("@opis", SqlDbType.NChar).Value = txtOpis.Text;
                cmd.Parameters.Add("@kustosID", SqlDbType.Int).Value = cbKustos.SelectedValue;
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(InvalidOperationException)
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
