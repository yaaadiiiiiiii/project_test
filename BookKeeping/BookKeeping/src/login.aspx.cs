using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _BookKeeping
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }


        protected void register_Click(object sender, EventArgs e)
        {
            Response.Redirect("register.aspx"); // 導向到註冊頁面
        }

        protected MySqlConnection DBConnection()
        {
            string connection = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connection);
            conn.Open();
            return conn;
        }

        //protected void LoginButton_Click(object sender, EventArgs e)
        //{
        //    string connection = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        //    MySqlConnection conn = new MySqlConnection(connection);
        //    conn.Open();

        //    MySqlCommand cmd = new MySqlCommand("SELECT * FROM `112-112502`.user基本資料 WHERE user_id = @userid AND password = @password", conn);
        //    cmd.Parameters.AddWithValue("@userid", UserAcc.Text);
        //    cmd.Parameters.AddWithValue("@password", UserPwd.Text);

        //    MySqlDataReader reader = cmd.ExecuteReader();
        //    // search

        //    if (reader.Read())
        //    {
        //        // 成功
        //        state.Text = "Login Successful";
        //    }
        //    else
        //    {
        //        // error
        //        state.Text = "帳號或密碼錯誤！";
        //    }
            
        //}

        protected void Login_click(object sender, EventArgs e)
        {
            string username = UserAcc.Text.Trim();
            string password = UserPwd.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                state.Text = "請輸入帳號和密碼";
                return; // Don't proceed with login
            }

            string connection = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connection);
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `112-112502`.user WHERE BINARY user_id = @userid AND password = @password", conn);
            cmd.Parameters.AddWithValue("@userid", UserAcc.Text);
            cmd.Parameters.AddWithValue("@password", UserPwd.Text);

            MySqlDataReader reader = cmd.ExecuteReader();
            // search

            if (reader.Read())
            {
                Session["UserID"] = UserAcc.Text;
                // 成功
                Response.Redirect("~/src/main.aspx");
            }
            else
            {
                // error
                state.Text = "帳號或密碼錯誤！";
            }
        }


    }
}