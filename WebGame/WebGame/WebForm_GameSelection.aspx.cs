using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebGame
{
    public partial class WebForm_GameSelection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }       
    


    protected void Button_Click(object sender, ImageClickEventArgs e)
        {
            if(sender==ImageButton1)
            {
                Session["game"] = "WebForm_InBetween.aspx";
            }
            else if (sender == ImageButton2)
            {
                Session["game"] = "WebForm_JumpGame.aspx";
            }

            Response.Redirect("WebForm_Login.aspx");

        }

    }
}