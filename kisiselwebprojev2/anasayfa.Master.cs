using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kisiselwebprojev2
{
    public partial class anasayfa : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
            {
                connection.Open();

                string selectQuery = "SELECT TOP 1 * FROM SocialMediaLinks ORDER BY SocialMediaLinkID DESC";
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                SqlDataReader reader = selectCommand.ExecuteReader();

                if (reader.Read())
                {
                    string twitterLink = reader["TwitterLink"].ToString();
                    string instagramLink = reader["InstagramLink"].ToString();
                    string linkedInLink = reader["LinkedInLink"].ToString();

                    // Bilgileri kullanarak sayfadaki elemanları güncelle
                    liTwitterLink.InnerHtml = $"<a href='{twitterLink}' rel='noopener' class='fab fa-twitter'></a>";
                    liInstagramLink.InnerHtml = $"<a href='{instagramLink}' rel='noopener' class='fab fa-instagram'></a>";
                    liLinkedInLink.InnerHtml = $"<a href='{linkedInLink}' rel='noopener' class='fab fa-linkedin'></a>";
                }

                reader.Close();
            }

            if (!IsPostBack)
            {
                // Hakkımda bilgisini veritabanından çek
                GetIntroductionFromDatabase();

            }
        }
        private void GetIntroductionFromDatabase()
        {
            string selectQuery = "SELECT Ad,AdSoyad, Sehir, DogumTarihi, Email, Hakkimda FROM KullaniciBilgileri WHERE KullaniciID = 1"; // Kullanıcı ID'si 1 olanı alalım

            using (SqlConnection connection = new SqlConnection(@"Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        LblAdSoyadBaslik.Text = reader["Ad"].ToString();
                        lblAdSoyad.Text = reader["AdSoyad"].ToString();
                        lblSehir.Text = reader["Sehir"].ToString();
                        lblDogumTarihi.Text = reader["DogumTarihi"].ToString();
                        lblEmail.Text = reader["Email"].ToString();
                        lblHakkimda.Text = reader["Hakkimda"].ToString();
                    }

                    reader.Close();
                }
            }
        }


        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            // TextBox'lardan kullanıcı bilgilerini al
            string adSoyad = cfName.Text;
            string email = cfEmail.Text;
            string mesaj = cfMessage.Text;

            // SQL sorgusunu oluştur
            string insertQuery = "INSERT INTO Mesajlar (AdSoyad, Email, Mesaj) VALUES (@AdSoyad, @Email, @Mesaj)";

            // Veritabanı bağlantı nesnesini oluştur
            using (SqlConnection connection = new SqlConnection("Data Source=HALIL;Initial Catalog=proje;Integrated Security=True"))
            {
                // SQL komut nesnesini oluştur
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@AdSoyad", adSoyad);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Mesaj", mesaj);

                    // Bağlantıyı aç
                    connection.Open();

                    try
                    {
                        // SQL komutunu çalıştır
                        int rowsAffected = command.ExecuteNonQuery();

                        // Kayıt başarılıysa, bilgileri güncelle
                        if (rowsAffected > 0)
                        {
                            Response.Write("Mesaj başarıyla kaydedildi!");
                        }
                        else
                        {
                            Response.Write("Mesaj kaydedilirken bir hata oluştu!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Bir hata oluştu: " + ex.Message);
                    }
                    finally
                    {
                        // Bağlantıyı kapat
                        connection.Close();
                    }
                }
            }
        }
    }
}
