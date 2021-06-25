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
        }
    }
}