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
    /// Interaction logic for frmSlika.xaml
    /// </summary>
    public partial class frmSlika : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? slikaID;
        public frmSlika(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            slikaID = id;
            try
            {
                konekcija.Open();
                string vratiAutore = @"select autorID,ime from tbl_Autor";
                SqlDataAdapter daAutora = new SqlDataAdapter(vratiAutore, konekcija);
                DataTable dtAutora = new DataTable();
                daAutora.Fill(dtAutora);
                cbAutor.ItemsSource = dtAutora.DefaultView;
                daAutora.Dispose();
                dtAutora.Dispose();

                string vratiIzlozbe = @"select izlozbaID,nazivIzlozbe from tbl_Izlozba";
                SqlDataAdapter daIzlozba = new SqlDataAdapter(vratiIzlozbe, konekcija);
                DataTable dtIzlozba= new DataTable();
                daIzlozba.Fill(dtIzlozba);
                cbIzlozba.ItemsSource = dtIzlozba.DefaultView;
                daIzlozba.Dispose();
                dtIzlozba.Dispose();

                string vratiKategorije = @"select kategorijaID,nazivKategorije from tbl_Kategorije";
                SqlDataAdapter daKategorija = new SqlDataAdapter(vratiKategorije, konekcija);
                DataTable dtKategorija = new DataTable();
                daKategorija.Fill(dtKategorija);
                cbKategorije.ItemsSource = dtKategorija.DefaultView;
                daKategorija.Dispose();
                dtKategorija.Dispose();

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
            if (slikaID.HasValue)
            {
                UcitajPodatkeSlike();
            }
            else
            {
                txtNazivSlike.Focus();
            }

            
        }
        private void UcitajPodatkeSlike()
        {
            

            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Slika WHERE slikaID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", slikaID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtNazivSlike.Text = reader["nazivSlike"].ToString();
                    txtGodinaNastanka.Text = reader["godinaNastanka"].ToString();
                    txtTehnika.Text = reader["tehnika"].ToString();
                    txtDimenzije.Text = reader["dimenzije"].ToString();
                    cbAutor.SelectedValue = reader["autorID"];
                    cbIzlozba.SelectedValue = reader["izlozbaID"];
                    cbKategorije.SelectedValue = reader["kategorijaID"];
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja podataka slike: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (string.IsNullOrWhiteSpace(txtNazivSlike.Text) || string.IsNullOrWhiteSpace(txtGodinaNastanka.Text) 
                || string.IsNullOrWhiteSpace(txtTehnika.Text) || string.IsNullOrWhiteSpace(txtDimenzije.Text)
               || cbAutor.SelectedValue == null || cbIzlozba.SelectedValue == null || cbKategorije.SelectedValue == null)
            {
                MessageBox.Show("Sva polja moraju biti popunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtGodinaNastanka.Text, out _))
            {
                MessageBox.Show("Godina mora biti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                konekcija.Open();
               
               
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                if(slikaID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Slika SET nazivSlike = @nazivSlike, godinaNastanka = @godinaNastanka, tehnika = @tehnika, dimenzije = @dimenzije, 
                        autorID = @autorID, izlozbaID = @izlozbaID, kategorijaID = @kategorijaID
                        WHERE slikaID = @id";
                    cmd.Parameters.AddWithValue("@id", slikaID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Slika(nazivSlike,godinaNastanka,tehnika,dimenzije,autorID,izlozbaID,kategorijaID)
                                values(@nazivSlike,@godinaNastanka,@tehnika,@dimenzije,@autorID,@izlozbaID,@kategorijaID)";
                }
                cmd.Parameters.Add("@nazivSlike", SqlDbType.NChar).Value = txtNazivSlike.Text;
                cmd.Parameters.Add("@godinaNastanka", SqlDbType.Int).Value = txtGodinaNastanka.Text;
                cmd.Parameters.Add("@tehnika", SqlDbType.NChar).Value = txtTehnika.Text;
                cmd.Parameters.Add("@dimenzije", SqlDbType.NChar).Value = txtDimenzije.Text;
                cmd.Parameters.Add("@autorID", SqlDbType.Int).Value = cbAutor.SelectedValue;
                cmd.Parameters.Add("@izlozbaID", SqlDbType.Int).Value = cbIzlozba.SelectedValue;
                cmd.Parameters.Add("@kategorijaID", SqlDbType.Int).Value = cbKategorije.SelectedValue;
                
              
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
