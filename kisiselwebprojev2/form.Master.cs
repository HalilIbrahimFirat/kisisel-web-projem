using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kisiselwebprojev2
{
    public partial class form : System.Web.UI.MasterPage
    {
        private string connectionString = "Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
                FillGridView();
            }

        }

        protected void btnListe_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Mesajlar";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();
                GridView1.DataSource = reader;
                GridView1.DataBind();

                connection.Close();
            }
        }

        protected void medyaGuncelle_Click(object sender, EventArgs e)
        {
            string twitterLink = txtTwitterLink.Text;
            string instagramLink = txtInstagramLink.Text;
            string linkedInLink = txtLinkedInLink.Text;

            // SQL veritabanına yeni satır ekleme işlemini gerçekleştir
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO SocialMediaLinks (TwitterLink, InstagramLink, LinkedInLink) VALUES (@TwitterLink, @InstagramLink, @LinkedInLink)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@TwitterLink", twitterLink);
                insertCommand.Parameters.AddWithValue("@InstagramLink", instagramLink);
                insertCommand.Parameters.AddWithValue("@LinkedInLink", linkedInLink);

                insertCommand.ExecuteNonQuery();
            }
        }

       
        private void BindGridView()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Mesajlar", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = GridView1.Rows[rowIndex];
                string mesajID = selectedRow.Cells[0].Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Mesajlar WHERE MesajID = @MesajID";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@MesajID", mesajID);
                    deleteCommand.ExecuteNonQuery();
                }

                // Veriyi tekrar bağlamak için, kendi uygulamanıza uygun şekilde ayarlayın.
                BindGridView();
            }
        }
        private void BindGridView2()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Login", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = GridView2.Rows[rowIndex];
                string username = selectedRow.Cells[0].Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Login WHERE Username = @Username";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@Username", username);
                    deleteCommand.ExecuteNonQuery();
                }

                // Veriyi tekrar bağlamak için, kendi uygulamanıza uygun şekilde ayarlayın.
                BindGridView2();
            }
        }



        protected void btnSifreGüncelle_Click(object sender, EventArgs e)
        {
            string username = txtKullaniciAdi.Text;
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblUpdateStatus.Text = "Lütfen tüm alanları doldurun.";
                return;
            }

            if (CheckUserCredentials(username, oldPassword))
            {
                if (newPassword == confirmPassword)
                {
                    UpdatePasswordInDatabase(username, newPassword);
                    lblUpdateStatus.Text = "Şifre başarıyla güncellendi.";
                    // Güncelleme başarılı olduktan sonra temizlik yapabilirsiniz:
                    txtOldPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmPassword.Text = "";
                   

                    Response.Redirect(Request.RawUrl, true);
                }
                else
                {
                    lblUpdateStatus.Text = "Yeni şifreler eşleşmiyor.";
                }
            }
            else
            {
                lblUpdateStatus.Text = "Kullanıcı adı veya eski şifre hatalı.";
            }
        }


        private bool CheckUserCredentials(string username, string password)
        {
            // Kullanıcı adı ve eski şifrenin doğruluğunu kontrol et
            // Bu fonksiyonu veritabanınıza uygun şekilde güncelleyin
            // Örnek:
            // SELECT COUNT(*) FROM Kullanici WHERE username = @username AND password = @password

            int count = 0;

            using (SqlConnection connection = new SqlConnection("Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
            {
                string query = "SELECT COUNT(*) FROM Login WHERE username = @username AND password = @password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }

            return count > 0;
        }

        private void UpdatePasswordInDatabase(string username, string newPassword)
        {
            // Yeni şifreyi güncelle
            // Bu fonksiyonu veritabanınıza uygun şekilde güncelleyin
            // Örnek:
            // UPDATE Kullanici SET password = @newPassword WHERE username = @username

            using (SqlConnection connection = new SqlConnection("Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
            {
                string query = "UPDATE Login SET password = @newPassword WHERE username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newPassword", newPassword);
                    command.Parameters.AddWithValue("@username", username);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        protected void btnYeniKayit_Click(object sender, EventArgs e)
        {
            string yeniKullaniciAdi = txtYeniKullanici.Text;
            string yeniKullaniciSifre = txtYeniKullaniciSifre.Text;



            if (string.IsNullOrEmpty(yeniKullaniciAdi) || string.IsNullOrEmpty(yeniKullaniciSifre))
            {
                // Kullanıcı adı veya şifre boşsa hata mesajı göster
                lblStatus.Text = "Kullanıcı adı ve şifre boş olamaz.";
                return;
            }

            if (KayitOlustur(yeniKullaniciAdi, yeniKullaniciSifre))
            {
                // Yeni kayıt başarılıysa
                lblStatus.Text = "Yeni kayıt başarıyla oluşturuldu.";
                // Ekranı temizleme (isteğe bağlı)
                txtYeniKullanici.Text = "";
                txtYeniKullaniciSifre.Text = "";
            }
            else
            {
                // Yeni kayıt oluşturulamazsa
                lblStatus.Text = "Yeni kayıt oluşturma başarısız.";
            }
            FillGridView2();

        }
        private void FillGridView2()
        {
            try
            {
                DataTable dt = GetUsersFromDatabase(); // Kullanıcı verilerini getir
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını loglamak veya görüntülemek için kullanılabilir
                lblStatus.Text = "Bir hata oluştu: " + ex.Message;
            }
        }

        private bool KayitOlustur(string kullaniciAdi, string sifre)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
                {
                    string query = "INSERT INTO Login (Username, Password) VALUES (@username, @password)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", kullaniciAdi);
                        command.Parameters.AddWithValue("@password", sifre);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını loglamak veya görüntülemek için kullanılabilir
                lblStatus.Text = "Bir hata oluştu: " + ex.Message;
                return false;
            }
        }
        private void FillGridView()
        {
            try
            {
                DataTable dt = GetUsersFromDatabase(); // Kullanıcı verilerini getir
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını loglamak veya görüntülemek için kullanılabilir
                lblStatus.Text = "Bir hata oluştu: " + ex.Message;
            }
        }

        private DataTable GetUsersFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
            {
                string query = "SELECT Username, Password FROM Login";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            UpdateUserInfoInDatabase();
            GetUserInfoFromDatabase();
        }
        private void GetUserInfoFromDatabase()
        {
            string selectQuery = "SELECT * FROM KullaniciBilgileri WHERE KullaniciID = 1"; // Kullanıcı ID'si 1 olanı alalım

            using (SqlConnection connection = new SqlConnection(@"Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        txtYasadigiSehir.Text = reader["Ad"].ToString();
                        txtHakkimda.Text = reader["Hakkimda"].ToString();
                        txtAdSoyad.Text = reader["AdSoyad"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtDogumGunu.Text = reader["DogumTarihi"].ToString();
                        txtYasadigiSehir.Text = reader["Sehir"].ToString();
                    }

                    reader.Close();
                }
            }
        }

        private void UpdateUserInfoInDatabase()
        {
            string updateQuery = "UPDATE KullaniciBilgileri SET Ad=@Ad,AdSoyad = @AdSoyad, Email = @Email, DogumTarihi = @DogumTarihi, Sehir = @Sehir, Hakkimda=@Hakkimda WHERE KullaniciID = 1"; // Kullanıcı ID'si 1 olanı güncelleyelim

            using (SqlConnection connection = new SqlConnection(@"Data Source=HALIL;Initial Catalog=proje;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Ad", txtAd.Text);
                    command.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@DogumTarihi", txtDogumGunu.Text);
                    command.Parameters.AddWithValue("@Sehir", txtYasadigiSehir.Text);
                    command.Parameters.AddWithValue("@Hakkimda", txtHakkimda.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}




