using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace BookKeeping.src
{
    public partial class setting : System.Web.UI.Page
    {
        protected string user_id; // 新增成員變數

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 获取当前用户的 ID（你的实现方式可能不同）
                string userID = Session["UserID"] as string;

                // 检查是否有用户 ID
                if (!string.IsNullOrEmpty(userID))
                {
                    // 使用用户 ID 查询数据库以获取用户数据
                    string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // 创建 SQL 查询
                        string query = "SELECT nickname, gender, user_id, birthday FROM `112-112502`.user WHERE user_id = @UserID";

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                

                                if (reader.Read())
                                {
                                    // 从数据库中获取用户数据并填充到 Label 中
                                    nickname.Text = reader["nickname"].ToString();
                                    gender.Text = reader["gender"].ToString();
                                    account.Text = reader["user_id"].ToString();

                                    DateTime birthdayDate = (DateTime)reader["birthday"];
                                    birthdate.Text = birthdayDate.ToString("yyyy/MM/dd");

                                    if (reader["gender"].ToString() == "男生")
                                    {
                                        Ava.ImageUrl = "images/avatar/ava_boy.png";
                                    }
                                    else
                                    {
                                        Ava.ImageUrl = "images/avatar/ava_girl.png";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        protected void Logout_Click(object sender, EventArgs e)
        {
            // 清除用戶的 Session，以確保用戶登出
            Session.Clear();

            // 重定向到登入頁面
            Response.Redirect("login.aspx");
        }
    }
}