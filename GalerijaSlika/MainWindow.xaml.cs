using GalerijaSlika.Forme;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GalerijaSlika
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private string aktivnaTabela = "tbl_Autor";
        private string nazivID = "autorID";
        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke();

        }
        private void UcitajPodatke()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM {aktivnaTabela}", konekcija);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridCentar.ItemsSource = dt.DefaultView;
            }
            catch
            {
                MessageBox.Show("Greška pri učitavanju podataka: ", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        
        }

        private void btnAutor_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Autor";
            nazivID = "autorID";
            UcitajPodatke();
        }

        private void btnIzlozba_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Izlozba";
            nazivID = "izlozbaID";
            UcitajPodatke();
        }

        private void btnKategorije_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Kategorije";
            nazivID = "kategorijaID";
            UcitajPodatke();
        }

        private void btnKorisnik_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Korisnik";
            nazivID = "korisnikID";
            UcitajPodatke();
        }

        private void btnKustos_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Kustos";
            nazivID = "kustosID";
            UcitajPodatke();

        }

        private void btnRecenzija_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Recenzija";
            nazivID = "recenzijaID";
            UcitajPodatke();
        }

        private void btnSlika_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Slika";
            nazivID = "slikaID";
            UcitajPodatke();
        }

        private void btnUlaznica_Click(object sender, RoutedEventArgs e)
        {
            aktivnaTabela = "tbl_Ulaznica";
            nazivID = "ulaznicaID";
            UcitajPodatke();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            OpenForm(null);
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCentar.SelectedItem == null)
            {
                MessageBox.Show("Odaberite stavku za izmenu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRowView selectedRow = (DataRowView)dataGridCentar.SelectedItem;
            int id = Convert.ToInt32(selectedRow[nazivID]);
            OpenForm(id);
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCentar.SelectedItem == null)
            {
                MessageBox.Show("Odaberite stavku za brisanje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRowView selectedRow = (DataRowView)dataGridCentar.SelectedItem;
            int id = Convert.ToInt32(selectedRow[nazivID]);

            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete stavku?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    konekcija.Open();
                    SqlCommand cmd = new SqlCommand($"DELETE FROM {aktivnaTabela} WHERE {nazivID} = @id", konekcija);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Greška pri brisanju: "+ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    konekcija.Close();
                    UcitajPodatke();
                }
            }
        }
            private void OpenForm(int? id)
            {
                Window forma = null;

                switch (aktivnaTabela)
                {
                    case "tbl_Autor":
                        forma = new GalerijaSlika.Forme.frmAutor(id);
                        break;
                    case "tbl_Izlozba":
                        forma = new GalerijaSlika.Forme.frmIzlozba(id);
                        break;
                    case "tbl_Kategorije":
                        forma = new GalerijaSlika.Forme.frmKategorije(id);
                        break;
                    case "tbl_Korisnik":
                        forma = new GalerijaSlika.Forme.frmKorisnik(id);
                        break;
                    case "tbl_Kustos":
                        forma = new GalerijaSlika.Forme.frmKustos(id);
                        break;
                    case "tbl_Recenzija":
                        forma = new GalerijaSlika.Forme.frmRecenzija(id);
                        break;
                    case "tbl_Slika":
                        forma = new GalerijaSlika.Forme.frmSlika(id);
                        break;
                    case "tbl_Ulaznica":
                        forma = new GalerijaSlika.Forme.frmUlaznica(id);
                        break;
                    default:
                        MessageBox.Show("Nepoznata tabela.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }

                forma?.ShowDialog();
                UcitajPodatke();
            }
        }
    }

