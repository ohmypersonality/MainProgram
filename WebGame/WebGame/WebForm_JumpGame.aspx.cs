using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebGame
{
    public partial class WebForm_JumpGame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["game"]) == "" || Convert.ToString(Session["user"]) == "")
            {
                Response.Redirect("WebForm_GameSelection");
            }

            Class_Game newGame = new Class_Game();
            newGame.rank("JumpGame", "score", GridView1);
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {           
            Response.Redirect("WebForm_GameSelection", false);
        }

        protected void GridView1_RawDataBound(object sender, GridViewRowEventArgs e) //GridView1綁定data source的資料後執行的內容
        {
            e.Row.Cells[1].Visible = false; //隱藏game欄位
            e.Row.Cells[3].Visible = false; //隱藏recordtype欄位          

        }

        protected void Button_Record_Click(object sender, EventArgs e)
        {
            int score = Convert.ToInt32(Request.Params["Score"]);
            if(score>0)
            {
                Class_Game newGame = new Class_Game();
                newGame.record("JumpGame", Convert.ToString(Session["user"]), "score", score);
                newGame.rank("JumpGame", "score", GridView1);
            }            
        }
    }
}