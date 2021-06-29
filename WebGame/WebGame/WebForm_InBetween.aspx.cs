using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace WebGame
{
    public partial class WebForm_InBetween : System.Web.UI.Page
    {
        
        string porkpath = "~/pic/poker/";
        Random random = new Random();

        #region "event"

        protected void Page_Load(object sender, EventArgs e) //網頁刷新時執行的內容，基本上所有方法或事件執行前都會先進行網頁刷新
        {
            if(Convert.ToString(Session["game"])=="" || Convert.ToString(Session["user"]) == "")
            {
                Response.Redirect("WebForm_GameSelection");
            } 
            
            Label_Hello.Text = "Hello! " + Convert.ToString(Session["user"]);
            Label_Hello.Font.Size = FontUnit.Larger;
            Label_Hello.Font.Bold = true;

            Class_Game newGame = new Class_Game();
            newGame.rank("InBetween", "score", GridView1);

            
            if (!IsPostBack)
            {
                porkInitial();                
            }


            String room_number = Convert.ToString(Request.QueryString["id"]);
            if (room_number == null)
            {                
                Session["IsInRoom"] = false;                              
            }
            else
            {
                TextBox_Room.Text = room_number;
                Session["IsInRoom"] = true;
            }

            String game_name = Convert.ToString(Session["game"]);
            Boolean IsInRoom = Convert.ToBoolean(Session["IsInRoom"]);
            Boolean IsBattleStart = Convert.ToBoolean(Application[game_name + "_IsBattleStart_" + room_number]);

            if (!IsInRoom)
            {
                Timer_Status.Enabled = true;
                Timer_Status.Interval = 1000;
                user_setting();
            }
            else
            {
                Timer_Status.Enabled = false;
            }

            if (IsInRoom && !IsBattleStart)
            {
                Timer_RoomUser.Enabled = true;
                Timer_RoomUser.Interval = 1000;                           
            }
            else
            {
                Timer_RoomUser.Enabled = false;
            }

            if (IsInRoom && IsBattleStart)
            {
                Timer_GameProcess.Enabled = true;
                Timer_GameProcess.Interval = 1000;                              
            }
            else
            {
                Timer_GameProcess.Enabled = false;
            }
        }



        protected ArrayList Listening_Application(String ApplicationName)
        {
            ArrayList Results = new ArrayList();
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            int count = room_user.Count;            
            Boolean IsListening = true;
            String content = "";   
           
            do
            {
                for (int i = 0; i <= count - 1; i++)
                {                    
                    content = Convert.ToString(Application[ApplicationName+"_"+i]);
                    IsListening = true;
                    if (content == null || content == "")
                    {
                        IsListening = false;
                    }
                }

            } while (!IsListening);


            for (int i = 0; i <= count - 1; i++)
            {
                Results.Add(Convert.ToString(room_user[i]));
            } 

            return Results;
        }

        protected void Button_Test_Click(object sender, EventArgs e)
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String user_name = Convert.ToString(Session["user"]);
            int order = Convert.ToInt32(Session["order"]);

            if (room_number != null)
            {
                Application["Test_" + order] = user_name;
                String ApplicationName = "Test";
                ArrayList results = Listening_Application(ApplicationName);
                String message = "";
                for (int j=0;j<= results.Count-1;j++)
                {
                    message += Convert.ToString(results[j]) + "\n";
                }

                Label2.Text = message;
            }

        }


        protected void Button_Start_Click(object sender, EventArgs e) 
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            
            if (room_number == null)
            {
                RandomPork(room_number, "deal1", Image_deal1);
                RandomPork(room_number, "deal2", Image_deal2);

                Image_player1.ImageUrl = porkpath + "bicycle_backs.jpg";
                Image_player2.ImageUrl = porkpath + "bicycle_backs.jpg";
                Image_player3.ImageUrl = porkpath + "bicycle_backs.jpg";
                Image_player4.ImageUrl = porkpath + "bicycle_backs.jpg";

                Button_Start.Enabled = false;
                Button_Bet.Enabled = true;
                Button_Pass.Enabled = true;

                Opening();
            }
            else
            {
                String game_name = Convert.ToString(Session["game"]);
                Application[game_name + "_" + room_number + "_status"] = "InitialDeal";
                Application[game_name + "_IsBattleStart_" + room_number] = true;
                //Session["IsInRoom"] = false;
            }  
        }

        protected void Button_Bet_Click(object sender, EventArgs e)
        {
            ArrayList player = new ArrayList();
            player.Add(Image_player1);
            player.Add(Image_player2);
            player.Add(Image_player3);
            player.Add(Image_player4);

            String room_number = Convert.ToString(Request.QueryString["id"]);            
            ArrayList pork;
            int least_pork;

            if (room_number == null)
            {
                least_pork = 3;
                pork = (ArrayList)Session["pork"];
                RandomPork(room_number, "player1", (Image)player[0], false);
                Bet();
                Button_Start.Enabled = true;
                Button_Bet.Enabled = false;
                Button_Pass.Enabled = false;

                if (pork.Count < least_pork)
                {
                    porkInitial();
                    Label_Count.Text = "本輪已結束，新的一輪即將開始";
                    Label_Count.Font.Size = FontUnit.Larger;
                    Label_Count.Font.Bold = true;
                }
            }
            else
            {
                String game_name = Convert.ToString(Session["game"]);                
                int order = Convert.ToInt32(Session["order"]);                          
                RandomPork(room_number, "player" + (order + 1), (Image)player[order], false);
                Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_action"] = "Bet";
                Application[game_name + "_" + room_number +"_status"] = "PlayerDeal";
            } 
        }

        protected void Button_Pass_Click(object sender, EventArgs e)
        {
            ArrayList pork = (ArrayList)Session["pork"];
            Button_Start.Enabled = true;
            Button_Bet.Enabled = false;
            Button_Pass.Enabled = false;

            String room_number = Convert.ToString(Request.QueryString["id"]);
            int least_pork = 3;
            if (room_number == null)
            {
                if (pork.Count < least_pork)
                {
                    porkInitial();
                    Label_Count.Text = "本輪已結束，新的一輪即將開始";
                    Label_Count.Font.Size = FontUnit.Larger;
                    Label_Count.Font.Bold = true;
                }
                else
                {
                    RandomPork(room_number, "deal1", Image_deal1);
                    RandomPork(room_number, "deal2", Image_deal2);
                    Image_player1.ImageUrl = porkpath + "bicycle_backs.jpg";                    

                    Button_Start.Enabled = false;
                    Button_Bet.Enabled = true;
                    Button_Pass.Enabled = true;
                    Opening();
                }
            }
            else
            {
                String game_name = Convert.ToString(Session["game"]);
                ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                least_pork = 2 + room_user.Count;

                if (pork.Count < least_pork)
                {
                    porkInitial();
                    Label_Count.Text = "本輪已結束，新的一輪即將開始";
                    Label_Count.Font.Size = FontUnit.Larger;
                    Label_Count.Font.Bold = true;
                }
                else
                {              
                    int order = Convert.ToInt32(Session["order"]);
                    Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_pork"] = porkpath + "bicycle_backs.jpg";
                    Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_action"] = "Pass";
                    Application[game_name + "_" + room_number + "_status"] = "PlayerDeal";

                    Button_Bet.Enabled = true;
                    Button_Pass.Enabled = true;
                }          
            }
        }

        protected void Button_Record_Click(object sender, EventArgs e)
        {
            Class_Game newGame = new Class_Game();
            newGame.record("InBetween", Convert.ToString(Session["user"]), "score", Convert.ToInt32(Label_NowCoin.Text));
            newGame.rank("InBetween", "score", GridView1);
            porkInitial();
            coinInitial();
            Label_Count.Text = "重新開局!";
        }

        protected void TextBox_BetCoin_TextChanged(object sender, EventArgs e) //TextBox_BetCoin內容變更時執行的內容
        {
            if (Convert.ToInt32(TextBox_BetCoin.Text) < 1000)
            {
                TextBox_BetCoin.Text = "1000";
            }
            else if (Convert.ToInt32(TextBox_BetCoin.Text) > Convert.ToInt32(Label_NowCoin.Text))
            {
                TextBox_BetCoin.Text = Label_NowCoin.Text;
            }
        }

        protected void GridView1_RawDataBound(object sender, GridViewRowEventArgs e) //GridView1綁定data source的資料後執行的內容
        {
            e.Row.Cells[1].Visible = false; //隱藏game欄位
            e.Row.Cells[3].Visible = false; //隱藏recordtype欄位          

        }

        protected void Button_EnterRoom_Click(object sender, EventArgs e)
        {
            String room_number = Convert.ToString(TextBox_Room.Text);
            String game_name = Convert.ToString(Session["game"]);
            String user_name = Convert.ToString(Session["user"]);
            int max_user = 4;

            EnterRoom(room_number, game_name, user_name, max_user);
        }

        protected void ListBox_BattleRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            String room_name = Convert.ToString(ListBox_BattleRoom.SelectedItem);
            TextBox_Room.Text = room_name.Substring(0, room_name.IndexOf(":"));
        }
               

        protected void Button_ExitRoom_Click(object sender, EventArgs e)
        {
            String last_room_number = Convert.ToString(Request.QueryString["id"]); //以get方式取得網址上"?id="後面的房號
            String game_name = Convert.ToString(Session["game"]);

            if (last_room_number != null) //若有房號，則在跳轉之前需先執行ExitRoom
            {
                String user_name = Convert.ToString(Session["user"]);
                int max_user = 4;
                ExitRoom(last_room_number, game_name, user_name, max_user);
            }
        }

        protected void Timer_Status_Tick(object sender, EventArgs e)
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            Load_Room(ListBox_BattleRoom, Label5, room_number, game_name);
            //user_setting();
        }

        protected void Timer_RoomUser_Tick(object sender, EventArgs e)
        {
            user_setting();
        }

        protected void Timer_GameProcess_Tick(object sender, EventArgs e)
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            String room_status = Convert.ToString(Application[game_name + "_" + room_number + "_status"]);
            switch(room_status)
            {
                case "InitialDeal":
                    {
                        InitialDeal();                        
                    }
                    break;
                case "OpeningCheck":
                    {
                        Opening();                       
                    }
                    break;
                case "UpdateScore_Opening":
                    {
                        UpdateScore_Opening();
                    }
                    break;
                case "PlayerDeal":
                    {
                        PlayerDeal();
                    }
                    break;
                case "GameResults":
                    {
                        GameResults();                        
                    }
                    break;
                case "UpdateScore_Action":
                    {                        
                        UpdateScore_Action();
                    }
                    break;
            }           
        }


        #endregion

        #region "game operation"

        protected void user_setting()  //根據遊戲人數進行版面調整
        {
            ArrayList player = new ArrayList();
            player.Add(Image_player1);
            player.Add(Image_player2);
            player.Add(Image_player3);
            player.Add(Image_player4);

            ArrayList player_name = new ArrayList();
            player_name.Add(Label_player1);
            player_name.Add(Label_player2);
            player_name.Add(Label_player3);
            player_name.Add(Label_player4);
                       

            String room_number = Convert.ToString(Request.QueryString["id"]);

            if (room_number == null)
            {
                for (int i = 1; i <= 3; i++)
                {
                    ((Image)player[i]).Visible = false;
                }

            }
            else
            {
                String game_name = Convert.ToString(Session["game"]);
                String user_name = Convert.ToString(Session["user"]);
                ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                int order = room_user.IndexOf(user_name);
                Session["order"] = order;
                int count = room_user.Count;
                

                for (int i = 0; i <= 3; i++)
                {
                    if (i <= count - 1)
                    {
                        ((Image)player[i]).Visible = true;

                        if(i==order)
                        {
                            ((Label)player_name[i]).Text = Convert.ToString(room_user[i])+" (me)";
                        }
                        else
                        {
                            ((Label)player_name[i]).Text = Convert.ToString(room_user[i]);
                        }
                        
                        Application[game_name + "_" + room_number + "_" + "player" + (i + 1) + "_score"] = 50000;
                    }
                    else
                    {
                        ((Image)player[i]).Visible = false;
                        ((Label)player_name[i]).Text = "";
                        Application[game_name + "_" + room_number + "_" + "player" + (i + 1) + "_score"] = "";
                    }
                }
            }
        }

        protected int ChangeToNumber(Image image) //將已翻開的撲克牌結果轉成數值，以利後續的比大小

        {
            int length = image.ImageUrl.Length;
            String pathstring = image.ImageUrl.Substring(length - 6, 2);
            int n = Convert.ToInt32(pathstring);
            int m = n % 13;

            if (m == 0)
            {
                return 13;
            }
            else
            {
                return m;
            }
        }


        protected void porkInitial() //將撲克牌牌堆初始化
        {
            String key;
            ArrayList pork = new ArrayList();
            for (int i = 1; i <= 52; i++)
            {
                key = i.ToString("00");
                pork.Add(porkpath + "p" + key + ".jpg");
            }

            String room_number = Convert.ToString(Request.QueryString["id"]);

            if (room_number == null)
            {
                Session["pork"] = pork;
            }
            else
            {
                Application["pork" + room_number] = pork;
            }

        }

        protected void coinInitial() //將賭金初始化
        {
            Label_NowCoin.Text = "50000";
            TextBox_BetCoin.Text = "1000";
        }


        protected void InitialDeal()
        {     
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            int order = Convert.ToInt32(Session["order"]);
            String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_deal_status"]);

            if(!user_status.Equals("RandomDeal"))
            {
                RandomPork(room_number, "deal1", Image_deal1);
                RandomPork(room_number, "deal2", Image_deal2);
            }
          

            String deal1_new = Convert.ToString(Application[game_name + "_" + room_number + "_" + "deal1" + "_pork"]);
            String deal2_new = Convert.ToString(Application[game_name + "_" + room_number + "_" + "deal2" + "_pork"]);
           
           Image_deal1.ImageUrl = deal1_new;
           Image_deal2.ImageUrl = deal2_new;    

           Image_player1.ImageUrl = porkpath + "bicycle_backs.jpg";
           Image_player2.ImageUrl = porkpath + "bicycle_backs.jpg";
           Image_player3.ImageUrl = porkpath + "bicycle_backs.jpg";
           Image_player4.ImageUrl = porkpath + "bicycle_backs.jpg";

           Button_Start.Enabled = false;
           Button_Bet.Enabled = true;
           Button_Pass.Enabled = true;

           

           if (Image_deal1.ImageUrl.Equals(deal1_new) && Image_deal2.ImageUrl.Equals(deal2_new))
           {
                Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"] = "OpeningCheck";
           }

           if(GameStatusCheck("OpeningCheck"))
            {
                Application[game_name + "_" + room_number + "_status"] = "OpeningCheck";
            }

        }

        protected void PlayerDeal()
        {
            ArrayList player = new ArrayList();
            player.Add(Image_player1);
            player.Add(Image_player2);
            player.Add(Image_player3);
            player.Add(Image_player4);

            String room_number = Convert.ToString(Request.QueryString["id"]);
            ArrayList pork;
            int least_pork;           
           
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            least_pork = 2 + room_user.Count;
            pork = (ArrayList)Application["pork" + room_number];
            String Action;
            bool AllAction=true;
            int order = Convert.ToInt32(Session["order"]);
            String newUrl;
            

            for (int i = 0; i <= room_user.Count - 1; i++)
              {
                 Action = Convert.ToString(Application[game_name+"_" + room_number + "_" + "player" + (i + 1) + "_action"]);
                 if (Action.Equals("Bet"))
                 {
                    newUrl = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (i + 1) + "_pork"]);
                    ((Image)player[i]).ImageUrl = newUrl;                  
                 }
                 else if(Action.Equals("Pass"))
                {
                    Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                }
                else
                {
                    AllAction = false;
                }
              }


            if(((Image)player[order]).ImageUrl.Equals(Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_pork"])))
            {
                Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"]= "GameResults";
            }



            // Application[game_name+"_" + room_number + "_" + "player" + (order + 1) + "_action"] = false;

            if (AllAction)
            {
                if(GameStatusCheck("GameResults"))
                {
                    //for (int i = 0; i <= room_user.Count - 1; i++)
                    //{
                    //    Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_action"] = "None";
                    //}

                    Application[game_name + "_" + room_number + "_status"] = "GameResults";
                }               
            }
            
        }             


        protected Boolean GameStatusCheck(String Status)
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            String playerStatus;           

            for(int i =0;i<=room_user.Count-1;i++)
            {
                playerStatus = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (i + 1) + "_status"]);
                if (!playerStatus.Equals(Status))
                {
                    return false;
                }                
            }

            return true;
        }


        protected void GameResults()
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            ArrayList pork = (ArrayList)Application["pork" + room_number];
            int least_pork = 2 + room_user.Count;

            Bet();

            Button_Bet.Enabled = false;
            Button_Pass.Enabled = false;
            Button_Start.Enabled = true;

            if (pork.Count < least_pork)
            {
                porkInitial();
                Label_Count.Text = "本輪已結束，新的一輪即將開始";
                Label_Count.Font.Size = FontUnit.Larger;
                Label_Count.Font.Bold = true;
                Button_Start.Enabled = true;
            }
        }

        protected void Opening() //發牌
        {
            ArrayList pork = (ArrayList)Session["pork"];
            Label_Count.Text = "本輪剩下" + ((pork.Count / 2) - 1).ToString() + "場";
            Label_Count.Font.Size = FontUnit.Small;
            Label_Count.Font.Bold = false;
            int least_pork=3;
            String room_number = Convert.ToString(Request.QueryString["id"]);
            if (room_number != null)
            {
                String game_name = Convert.ToString(Session["game"]);
                ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                least_pork = 2 + room_user.Count;
                Application[game_name + "_" + room_number + "_reDeal"] = false;
            }


            if (Math.Abs(ChangeToNumber(Image_deal1) - ChangeToNumber(Image_deal2)) <= 1)
            {
                Label_Result.Text = "You Lose! Please start again.";
                if (room_number == null)
                {
                    Label_NowCoin.Text = (Convert.ToInt32(Label_NowCoin.Text) - Convert.ToInt32(TextBox_BetCoin.Text)).ToString();

                    Button_Start.Enabled = true;
                    Button_Bet.Enabled = false;
                    Button_Pass.Enabled = false;

                    if (Convert.ToInt32(Label_NowCoin.Text) <= 0)
                    {
                        porkInitial();
                        coinInitial();
                        Label_Count.Text = "輸到脫褲子! 重新開局!";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                    }
                    else if (pork.Count < least_pork)
                    {
                        porkInitial();
                        Label_Count.Text = "本輪已結束，新的一輪即將開始";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                    }
                }
                else
                {
                    String game_name = Convert.ToString(Session["game"]);
                    ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                    int count = room_user.Count;
                    int order = Convert.ToInt32(Session["order"]);
                    String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"]);
                    Button_Bet.Enabled = false;
                    Button_Pass.Enabled = false;

                    if (!user_status.Equals("UpdateScore_Label"))
                    {
                        int newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"]) - Convert.ToInt32(TextBox_BetCoin.Text);
                        

                        if (newCoin <= 0)
                        {
                            porkInitial();
                            coinInitial();
                            Label_Count.Text = "輸到脫褲子! 重新開局!";
                            Label_Count.Font.Size = FontUnit.Larger;
                            Label_Count.Font.Bold = true;
                            Button_Start.Enabled = true;

                        }
                        else if (pork.Count < least_pork)
                        {
                            porkInitial();
                            Label_Count.Text = "本輪已結束，新的一輪即將開始";
                            Label_Count.Font.Size = FontUnit.Larger;
                            Label_Count.Font.Bold = true;
                            Button_Start.Enabled = true;
                        }

                        Label_NowCoin.Text = newCoin.ToString();

                        if (Label_NowCoin.Text.Equals(newCoin.ToString()))
                        {
                            Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"] = "UpdateScore_Label";
                        }
                    }
                    else
                    {
                        Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"] = Label_NowCoin.Text;
                        String tempCoin = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"]);
                        if (tempCoin.Equals(Label_NowCoin.Text))
                        {
                            Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"] = "UpdateScore_Opening";
                        }

                        if (GameStatusCheck("UpdateScore_Opening"))
                        {
                            Application[game_name + "_" + room_number + "_status"] = "UpdateScore_Opening";
                            Application[game_name + "_" + room_number + "_reDeal"] = true;
                        }
                    } 
                }
            }
            else
            {
                Label_Result.Text = "Bet or Pass?";
            }
        }


        protected void UpdateScore_Opening()
        {
            ArrayList player_score = new ArrayList();
            player_score.Add(Label_score1);
            player_score.Add(Label_score2);
            player_score.Add(Label_score3);
            player_score.Add(Label_score4);

            String game_name = Convert.ToString(Session["game"]);
            String room_number = Convert.ToString(Request.QueryString["id"]);           
            int order=Convert.ToInt32(Session["order"]);           

            for (int i = 0; i <= 3; i++)
            {
                ((Label)player_score[i]).Text = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (i + 1) + "_score"]);

              if (Label_NowCoin.Text.Equals(((Label)player_score[order]).Text))
              {
                  Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"] = "PlayerDeal";
              }                             
            }

            if (GameStatusCheck("PlayerDeal"))            
            {                
                if(Convert.ToBoolean(Application[game_name + "_" + room_number + "_reDeal"]))
                {
                    Application[game_name + "_" + room_number + "_status"] = "InitialDeal";
                    Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                }
                else
                {
                    Application[game_name + "_" + room_number + "_status"] = "PlayerDeal";
                }                
            }

        }

        protected void UpdateScore_Action()
        {
            ArrayList player_score = new ArrayList();
            player_score.Add(Label_score1);
            player_score.Add(Label_score2);
            player_score.Add(Label_score3);
            player_score.Add(Label_score4);

            String game_name = Convert.ToString(Session["game"]);
            String room_number = Convert.ToString(Request.QueryString["id"]);
            int order = Convert.ToInt32(Session["order"]);

            for (int i = 0; i <= 3; i++)
            {
                ((Label)player_score[i]).Text = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (i + 1) + "_score"]);

                if (Label_NowCoin.Text.Equals(((Label)player_score[order]).Text))
                {
                    Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"] = "InitialDeal";
                }
               
            }

            if (GameStatusCheck("InitialDeal"))
            {
                Application[game_name + "_" + room_number + "_status"] = "InitialDeal";
            }
        }


        protected void Bet() //下注跟進
        {

            String room_number = Convert.ToString(Request.QueryString["id"]);

            if (room_number == null)
            {
                if ((ChangeToNumber(Image_player1) - ChangeToNumber(Image_deal1)) * (ChangeToNumber(Image_player1) - ChangeToNumber(Image_deal2)) == 0)
                {
                    Label_Result.Text = "Bump! You Lose!";
                    Label_NowCoin.Text = (Convert.ToInt32(Label_NowCoin.Text) - 2 * Convert.ToInt32(TextBox_BetCoin.Text)).ToString();
                    if (Convert.ToInt32(Label_NowCoin.Text) <= 0)
                    {
                        porkInitial();
                        coinInitial();
                        Label_Count.Text = "輸到脫褲子! 重新開局!";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                    }
                }
                else if ((ChangeToNumber(Image_player1) - ChangeToNumber(Image_deal1)) * (ChangeToNumber(Image_player1) - ChangeToNumber(Image_deal2)) > 0)
                {
                    Label_Result.Text = "Out of door! You Lose!";
                    Label_NowCoin.Text = (Convert.ToInt32(Label_NowCoin.Text) - Convert.ToInt32(TextBox_BetCoin.Text)).ToString();
                    if (Convert.ToInt32(Label_NowCoin.Text) <= 0)
                    {
                        porkInitial();
                        coinInitial();
                        Label_Count.Text = "輸到脫褲子! 重新開局!";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                    }
                }
                else
                {
                    Label_Result.Text = "In door! You Win!";
                    Label_NowCoin.Text = (Convert.ToInt32(Label_NowCoin.Text) + Convert.ToInt32(TextBox_BetCoin.Text)).ToString();                   
                }

            }
            else
            {                
                ArrayList player = new ArrayList();
                player.Add(Image_player1);
                player.Add(Image_player2);
                player.Add(Image_player3);
                player.Add(Image_player4);

                ArrayList player_score = new ArrayList();
                player_score.Add(Label_score1);
                player_score.Add(Label_score2);
                player_score.Add(Label_score3);
                player_score.Add(Label_score4);

                String game_name = Convert.ToString(Session["game"]);
                ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                int order = Convert.ToInt32(Session["order"]);
                String Action = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_action"]);

                int newCoin;
                String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"]);

                if(!user_status.Equals("UpdateScore_Label"))
                {

                    if (Action.Equals("Pass"))
                    {
                        Label_Result.Text = "Pass";
                        newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"]);
                    }
                    else if ((ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal1)) * (ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal2)) == 0)
                    {
                        Label_Result.Text = "Bump! You Lose!";
                        newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"]) - 2 * Convert.ToInt32(TextBox_BetCoin.Text);
                    }
                    else if ((ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal1)) * (ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal2)) > 0)
                    {
                        Label_Result.Text = "Out of door! You Lose!";
                        newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"]) - Convert.ToInt32(TextBox_BetCoin.Text);
                    }
                    else
                    {
                        Label_Result.Text = "In door! You Win!";
                        newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"]) + Convert.ToInt32(TextBox_BetCoin.Text);
                    }

                    if (newCoin <= 0)
                    {
                        porkInitial();
                        coinInitial();
                        Label_Count.Text = "輸到脫褲子! 重新開局!";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                        Button_Start.Enabled = true;
                    }
                    else
                    {
                        Label_NowCoin.Text = newCoin.ToString(); 

                        if (Label_NowCoin.Text.Equals(newCoin.ToString()))
                        {
                            Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"] = "UpdateScore_Label";
                        }                        
                    }
                }
                else
                {       
                    Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"] = Label_NowCoin.Text;
                    String tempCoin = Convert.ToString(Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_score"]);
                    if (tempCoin.Equals(Label_NowCoin.Text))
                    {
                        Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_status"] = "UpdateScore_Action";
                    }

                    if (GameStatusCheck("UpdateScore_Action"))
                    {
                        for (int i = 0; i <= room_user.Count - 1; i++)
                        {
                            Application[game_name + "_" + room_number + "_" + "player" + (order + 1) + "_action"] = "None";
                        }

                        Application[game_name + "_" + room_number + "_status"] = "UpdateScore_Action";
                    }
                } 
            }  
        }

        /*
        protected void RandomPork(Image image, bool IsRemove = true) //隨機發牌
        {
            ArrayList pork = (ArrayList)Session["pork"];

            int n = pork.Count;
            int m = random.Next(1, n + 1) - 1;
            String picture;
            picture = pork[m].ToString();
            image.ImageUrl = picture;

            if (IsRemove)
            {
                pork.RemoveAt(m);
                Session["pork"] = pork;
            }
        }
        */

        protected void RandomPork(String room_number, String user, Image image, bool IsRemove = true) //隨機發牌
        {
            ArrayList pork;
            int n;
            int m;
            String picture;

            if (room_number == null)
            {
                pork = (ArrayList)Session["pork"];
                n = pork.Count;
                m = random.Next(1, n + 1) - 1;                
                picture = pork[m].ToString();
                image.ImageUrl = picture;

                if (IsRemove)
                {
                    pork.RemoveAt(m);
                    Session["pork"] = pork;
                }
            }
            else
            {
                String game_name = Convert.ToString(Session["game"]);
                String old_pork = Convert.ToString(Application[game_name + "_" + room_number + "_" + user + "_pork"]);
                pork = (ArrayList)Application["pork" + room_number];
                
                n = pork.Count;
                m = random.Next(1, n + 1) - 1;
                picture = pork[m].ToString();
                Application[game_name + "_" + room_number + "_" + user + "_pork"] = picture;


                if(user.Equals("deal1") || user.Equals("deal2"))
                {
                    int order = Convert.ToInt32(Session["order"]);
                    String new_pork = Convert.ToString(Application[game_name + "_" + room_number + "_" + user + "_pork"]);
                    String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_deal_status"]);
                    if (!new_pork.Equals(old_pork))
                    {
                        if (user_status.Equals("RandomDeal_First"))
                        {
                            Application[game_name + "_" + room_number + "_deal_status"] = "RandomDeal";
                        }
                        else
                        {
                            Application[game_name + "_" + room_number + "_deal_status"] = "RandomDeal_First";
                        }
                    }
                }
                else
                {
                    Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                }


                if (IsRemove)
                {
                    pork.RemoveAt(m);
                    Application["pork" + room_number] = pork;
                }
            } 
            
        }

        #endregion

        #region "battle room"


        //本來就有無限房間，只秀出有人的房號
        protected void Load_Room(ListBox room_list, Label user_list, String room_number, String game_name)
        {
            if (Application[game_name + "_room"] != null)
            {
                room_list.Items.Clear();

                Dictionary<String, String> room = (Dictionary<String, String>)Application[game_name + "_room"];
                String tempString = "";

                foreach (KeyValuePair<string, string> kvp in room)
                {
                    tempString = kvp.Key + ":" + kvp.Value;
                    room_list.Items.Add(tempString);
                }

                if (Application[game_name + "_" + room_number] != null)
                {
                    ArrayList room_user = new ArrayList();
                    room_user = (ArrayList)Application[game_name + "_" + room_number];
                    String user = "";

                    for (int i = 0; i <= room_user.Count - 1; i++)
                    {
                        user = user + room_user[i] + "</br>";
                    }

                    user_list.Text = user;
                }
            }

        }


        protected void EnterRoom(String room_number, String game_name, String user_name, int max_user)
        {
            if (room_number != "") //確定欄位有值
            {
                String new_url = game_name + "?id=" + room_number; //將房號以變數形式加在新網址當中
                Dictionary<String, String> room = new Dictionary<String, String>(); //我們希望房號跟使用者能夠分開紀錄，以便查詢，因此使用帶鍵/值對的Dictionary來存放有人的房號
                ArrayList room_user = new ArrayList(); //以ArayList來存放特定房號中的所有user
                String all_user = ""; //room要加入的對象為(room_number, all_user)，all_user為含有所有user的字串

                if (Application[game_name + "_room"] != null) //確定有房間有人
                {
                    room = (Dictionary<String, String>)Application[game_name + "_room"]; //由於已有房間有人，Application[game_name + "_room"]有紀錄，因此先讀出紀錄


                    if (Application[game_name + "_" + room_number] != null) //確定要進入的房間有人
                    {
                        room_user = (ArrayList)Application[game_name + "_" + room_number]; //由於要進入的房間有人，Application[game_name + "_" + room_number]有紀錄，因此先讀出紀錄

                        if (room_user.Count < max_user) //判斷房間是否滿了，最多max_user個人
                        {
                            room_user.Add(user_name); //新增一名進入房間的使用者
                        }
                        else //判斷不能加入，通知使用者並跳出此函式
                        {
                            MessageBox.Show("人數已滿，請找其他房間!");
                            return;
                        }


                        for (int i = 0; i <= room_user.Count - 1; i++) //需用for loop將所有使用者串成一個字串，再丟入all_user中
                        {
                            if (i == 0)
                            {
                                all_user = "(" + room_user.Count + "/" + max_user + ")" + Convert.ToString(room_user[i]);
                            }
                            else
                            {
                                all_user += "," + Convert.ToString(room_user[i]);
                            }
                        }

                        if (room.ContainsKey(room_number)) //確認讀出的紀錄中，正要進入的房間是否有紀錄(理論上 Application[game_name + "_" + room_number] 有東西就應該有紀錄)
                        {
                            room.Remove(room_number); //先刪除已存在的紀錄(後面新增進入房間的使用者後再重新加入)
                        }

                        room.Add(room_number, all_user);
                    }
                    else //確定要進入的房間沒有人
                    {
                        room_user.Add(user_name);
                        all_user = "(" + room_user.Count + "/" + max_user + ")" + Convert.ToString(room_user[0]);
                        room.Add(room_number, all_user);
                    }

                }
                else //所有的房間都沒有人
                {
                    room_user.Add(user_name);
                    all_user = "(" + room_user.Count + "/" + max_user + ")" + Convert.ToString(room_user[0]);
                    room.Add(room_number, all_user);
                }

                Application[game_name + "_" + room_number] = room_user; //更新Application[game_name + "_" + room_number]中的資訊
                Application[game_name + "_room"] = room; //更新Application[game_name+"_room"]中的資訊     

                String last_room_number = Convert.ToString(Request.QueryString["id"]); //以get方式取得網址上"?id="後面的房號
                if (last_room_number != null) //若有房號，則在跳轉之前需先執行ExitRoom
                {
                    ExitRoom(last_room_number, game_name, user_name, max_user);
                }

                Session["IsInRoom"] = true;
                ListBox_BattleRoom.Visible = false;
                GridView1.Visible = false;
                Response.Redirect(new_url); //跳轉至有帶有房號的網址
            }
            else
            {
                MessageBox.Show("請輸入房號!");
            }
        }


        protected void ExitRoom(String room_number, String game_name, String user_name, int max_user)
        {
            Dictionary<String, String> room = (Dictionary<String, String>)Application[game_name + "_room"];//讀出哪些房間有人的紀錄
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];//讀出該房號有哪些人的紀錄

            if (room_user.Count == 1) //確認該房號的使用者是否只有一人
            {
                room_user.Clear(); //直接將該房號有哪些人的紀錄清空

                if (room.Count == 1) //確認只有一間房間有人
                {
                    room.Clear(); //直接將哪些房間有人的紀錄清空
                }
                else //不只一間房間有人
                {
                    room.Remove(room_number); //僅刪除該房號有人的紀錄
                }
            }
            else //該房號的使用者不是只有一人
            {
                room_user.Remove(user_name); //僅刪除該房號有此人的紀錄
                String all_user = "";
                for (int i = 0; i <= room_user.Count - 1; i++) //需用for loop將所有使用者串成一個字串，再丟入all_user中
                {
                    if (i == 0)
                    {
                        all_user = "(" + room_user.Count + "/" + max_user + ")" + Convert.ToString(room_user[i]);
                    }
                    else
                    {
                        all_user += "," + Convert.ToString(room_user[i]);
                    }
                }

                if (room.ContainsKey(room_number)) //確認讀出的紀錄中，正要進入的房間是否有紀錄(理論上 Application[game_name + "_" + room_number] 有東西就應該有紀錄)
                {
                    room.Remove(room_number); //先刪除已存在的紀錄(後面新增進入房間的使用者後再重新加入)
                }

                room.Add(room_number, all_user);
            }

            Application[game_name + "_" + room_number] = room_user; //更新Application[game_name + "_" + room_number]中的資訊
            Application[game_name + "_room"] = room; //更新Application[game_name+"_room"]中的資訊     

            //Session["IsInRoom"] = false;
            TextBox_Room.Text = "";
            ListBox_BattleRoom.Visible = true;
            GridView1.Visible = true;
            Response.Redirect(game_name);
        }








        #endregion






       


    }

}