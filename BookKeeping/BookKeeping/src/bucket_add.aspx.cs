using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.SessionState;

namespace _BookKeeping
{
    public partial class bucket_add : System.Web.UI.Page
    {

        protected string user_id ;
        public object ErrorMessagel { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = Session["UserID"] as string;
            WishUser.Text = FindName()+"想要";
        }

        protected MySqlConnection DBConnection() 
        {
            string connection = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connection);
            conn.Open();
            return conn;
        }

        protected string FindName()
        {
            MySqlConnection conn = DBConnection();
            string sql = "SELECT nickname FROM `112-112502`.user\r\nwhere user_id = @user_id";
            string user_name = string.Empty;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user_name = reader.GetString(0);
            }

            return user_name;

        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = DBConnection();


            string d_name = WishTextbox.Text;

            if (string.IsNullOrWhiteSpace(d_name))//判斷是否為空值
            {
                ErrorMessage1.Text = "請輸入願望名稱";
                ErrorMessage1.Visible = true;
                return;
            }

            if (!IsAllChineseLetters(d_name))//判斷是否為中文
            {
                ErrorMessage1.Text = "願望名稱只能包含中文文字";
                ErrorMessage1.Visible = true;
                return;
            }
            string sql_count = "SELECT count(*) FROM `112-112502`.bucket_list where user_id = @user_id and exchange_state is null;";
            MySqlCommand cmd_count = new MySqlCommand(sql_count, conn);
            cmd_count.Parameters.AddWithValue("@user_id", user_id);
            MySqlDataReader reader = cmd_count.ExecuteReader();
            reader.Read();
            int wish_count = reader.GetInt32(0);
            
            conn.Close();

            //判斷願望是否達到上限
            if (wish_count < 4)
            {
                conn.Open();
                string sql = "insert into `112-112502`.bucket_list(user_id, d_name, pass_state) values (@name, @d_name, 'r')";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", user_id);
                cmd.Parameters.AddWithValue("@d_name", d_name);

                int rowsaffected = cmd.ExecuteNonQuery();
                WishTextbox.Text = null;

                if (rowsaffected > 0)
                {
                    string script = "var overlay = document.getElementById('overlay');";
                    script += "overlay.style.display = 'block';"; // 顯示背景遮罩
                    script += "var imageBox = document.createElement('img');";
                    script += "imageBox.src = 'images/alert_2Y.png';";
                    script += "imageBox.className = 'custom-image';";
                    script += "document.body.appendChild(imageBox);";
                    script += "setTimeout(function() { overlay.style.display = 'none'; }, 2000);"; // 隱藏背景遮罩
                    script += "setTimeout(function() { imageBox.style.display = 'none'; window.location.href = '" + ResolveUrl("~/src/bucket_list.aspx") + "'; }, 2000);"; // 显示图像一段时间后跳转
                    ClientScript.RegisterStartupScript(GetType(), "新增成功", script, true);
                }
                else
                {
                    string script = "var overlay = document.getElementById('overlay');";
                    script += "overlay.style.display = 'block';"; // 顯示背景遮罩
                    script += "var imageBox = document.createElement('img');";
                    script += "imageBox.src = 'images/alert_2N.png';";
                    script += "imageBox.className = 'custom-image';";
                    script += "document.body.appendChild(imageBox);";
                    script += "setTimeout(function() { overlay.style.display = 'none'; }, 2000);"; // 隱藏背景遮罩
                    script += "setTimeout(function() { imageBox.style.display = 'none'; }, 2000);"; // 自动隐藏图像
                    ClientScript.RegisterStartupScript(GetType(), "新增失敗", script, true);
                }

            }
            else
            {
                WishTextbox.Text = null;
                string script = "var overlay = document.getElementById('overlay');";
                script += "overlay.style.display = 'block';"; // 顯示背景遮罩
                script += "var imageBox = document.createElement('img');";
                script += "imageBox.src = 'images/alert_dre_full.png';";
                script += "imageBox.className = 'custom-image';";
                script += "document.body.appendChild(imageBox);";
                script += "setTimeout(function() { overlay.style.display = 'none'; }, 2000);"; // 隱藏背景遮罩
                script += "setTimeout(function() { imageBox.style.display = 'none'; }, 2000);"; // 自动隐藏图像
                ClientScript.RegisterStartupScript(GetType(), "願望已滿", script, true);
            }
        }



            protected bool IsAllChineseLetters(string input)
        {
            // 是否只包含中文字符
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[\u4e00-\u9fa5]+$");

        }



    }
}