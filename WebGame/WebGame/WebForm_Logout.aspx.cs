using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebGame
{
    public partial class WebForm_Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String game_name = Convert.ToString(Session["game"]);
            String room_number = Convert.ToString(Session["room"]);
            int order = Convert.ToInt32(Session["order"]);

            Application[game_name + "_exit_room_order"] = room_number + ":" + order;
            Application[game_name + "_" + room_number + "_status"] = "ExitRoom";
        }
    }
}