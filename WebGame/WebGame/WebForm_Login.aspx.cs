using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebGame
{
    public partial class WebForm_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["game"]) == "")
            {
                Response.Redirect("WebForm_GameSelection",false);
            }
        }

        protected void Button_Login_Click(object sender, EventArgs e)
        {
            Session["user"] = Request.Form["user"];
            Response.Redirect(Convert.ToString(Session["game"]));
        }

    }
}