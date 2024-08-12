using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kisiselwebprojev2
{
    public partial class login : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sayfa yüklendiğinde çalışacak kodları buraya ekleyebilirsiniz.
        }

        
        protected void Button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = username.Text;
            string sifre = password.Text;

            // Kullanıcı adı ve şifre boş kontrolü
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                Response.Write("Kullanıcı adı veya şifre boş olamaz!");
                lblUpdateStatus.Text = "Kullanıcı adı veya şifre boş olamaz!";
                return; // Geri kalan kodu çalıştırmadan çık
            }

            // SQL sorgusu
            string query = "SELECT COUNT(*) FROM Login WHERE username = @username AND password = @password";

            using (SqlConnection connection = new SqlConnection(@"Data Source=HALIL;Initial Catalog=proje;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", kullaniciAdi);
                    command.Parameters.AddWithValue("@password", sifre);

                    connection.Open();

                    // Kullanıcı adı ve şifre kontrolü
                    int result = (int)command.ExecuteScalar();

                    if (result > 0)
                    {
                        // Kullanıcı adı ve şifre doğruysa, panel.aspx sayfasına yönlendir
                        Response.Redirect("panel.aspx");
                    }
                    else
                    {
                        // Kullanıcı adı veya şifre hatalıysa, hata mesajı göster
                        Response.Write("Kullanıcı adı veya şifre hatalı!");
                        lblUpdateStatus.Text = "Kullanıcı adı veya şifre hatalı!";
                    }
                }
            }

        }
    }
}