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
    /// Interaction logic for frmKategorije.xaml
    /// </summary>
    public partial class frmKategorije : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private int? kategorijaID;
        public frmKategorije(int? id)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            kategorijaID = id;
            if (kategorijaID.HasValue)
            {
                UcitajPodatkeKategorije();
            }
            else
            {
                txtNazivKategorije.Focus();
            }
           
        }
        private void UcitajPodatkeKategorije()
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Kategorije WHERE kategorijaID = @id", konekcija);
                cmd.Parameters.AddWithValue("@id", kategorijaID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtNazivKategorije.Text = reader["nazivKategorije"].ToString();
                    txtOpis.Text = reader["opis"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka kategorije: " , "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (string.IsNullOrWhiteSpace(txtNazivKategorije.Text) || string.IsNullOrWhiteSpace(txtOpis.Text) )
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
                if (kategorijaID.HasValue)
                {
                    cmd.CommandText = @"UPDATE tbl_Kategorije 
                                        SET nazivKategorije = @nazivKategorije, opis = @opis 
                                        WHERE kategorijaID = @id";
                    cmd.Parameters.AddWithValue("@id", kategorijaID);
                }
                else
                {
                    cmd.CommandText = @"Insert into tbl_Kategorije(nazivKategorije,opis)
                                values(@nazivKategorije,@opis)";
                }
                cmd.Parameters.Add("@nazivKategorije", SqlDbType.NChar).Value = txtNazivKategorije.Text;
                cmd.Parameters.Add("@opis", SqlDbType.NChar).Value = txtOpis.Text;
                
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
