﻿using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        protected async void Page_Load(object sender, EventArgs e) //網頁刷新時執行的內容，基本上所有方法或事件執行前都會先進行網頁刷新
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);

            if (!IsPostBack)
            {

                if (Convert.ToString(Session["game"]) == "" || Convert.ToString(Session["user"]) == "")
                {
                    Response.Redirect("WebForm_GameSelection", false);
                }

                Label_Hello.Text = "Hello! " + Convert.ToString(Session["user"]);
                Label_Hello.Font.Size = FontUnit.Larger;
                Label_Hello.Font.Bold = true;

                Class_Game newGame = new Class_Game();
                newGame.rank("InBetween", "score", GridView1);
                porkInitial();


                if (room_number == null)
                {
                    //Session["IsInRoom"] = false;                
                    ListBox_BattleRoom.Visible = true;
                    GridView1.Visible = true;
                    Button_ExitRoom.Enabled = false;
                    Button_EnterRoom.Enabled = true;
                    Timer_Status.Enabled = true;
                    Timer_Status.Interval = 500;
                    await roomMonitor();
                }
                else
                {
                    //Session["IsInRoom"] = true;
                    TextBox_Room.Text = room_number;
                    ListBox_BattleRoom.Visible = false;
                    GridView1.Visible = false;
                    Button_ExitRoom.Enabled = true;
                    Button_EnterRoom.Enabled = false;
                    Timer_Status.Enabled = false;

                    
                    Timer_RoomUser.Enabled = true;
                    Timer_RoomUser.Interval = 500;
                    
                }
            }
            else
            {
                if (room_number == null)
                {
                }
                else
                {
                    String deal_status = Convert.ToString(Application[game_name + "_" + room_number + "_deal_status"]);
                    if (deal_status.Equals("ReSet"))
                    {
                        //Button_Start.Enabled = true;
                        //Button_Bet.Enabled = false;
                        //Button_Pass.Enabled = false;
                        //await ButtonEnabledControl("ReStart");
                        //Timer_RoomUser.Enabled = true;
                        //Timer_RoomUser.Interval = 1000;
                    }
                    else
                    {
                        //Button_Start.Enabled = false;
                        //Button_Bet.Enabled = true;
                        //Button_Pass.Enabled = true;
                        //await ButtonEnabledControl("Processing");
                    }

                    /*
                    String game_name = Convert.ToString(Session["game"]);
                    String user_name = Convert.ToString(Session["user"]);
                    ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                    int count = room_user.Count;
                    int order = room_user.IndexOf(user_name);
                    */
                    /*
                    Boolean IsBattleStart = Convert.ToBoolean(Application[game_name + "_IsBattleStart_" + room_number]);
                    if (IsBattleStart)
                    {
                        await InitialDeal();
                        //Timer_RoomUser.Enabled = false;
                    }
                    */
                    /*
                    String deal_status = Convert.ToString(Application[game_name + "_" + room_number + "_deal_status"]);
                    if (deal_status.Equals("GameStart"))
                    {
                        await InitialDeal();
                    }
                    */
                }
            }

            



            //Boolean IsInRoom = Convert.ToBoolean(Session["IsInRoom"]);


            /*
            if (!IsInRoom)
            {
                Timer_Status.Enabled = true;
                Timer_Status.Interval = 1000;
                await roomMonitor();
            }
            else
            {
                Timer_Status.Enabled = false;
            }

            if (IsInRoom)
            {
                Timer_Battle.Enabled = true;
                Timer_Battle.Interval = 1000;
            }
            */

            /*
            if (IsInRoom && !IsBattleStart)
            {
                Timer_RoomUser.Enabled = true;
                Timer_RoomUser.Interval = 1000;                           
            }
            else
            {                
                Timer_RoomUser.Enabled = false;
                //Timer_Battle.Enabled = true;
                //Timer_Battle.Interval = 1000;
            }
           */
        }

        protected void GridView1_RawDataBound(object sender, GridViewRowEventArgs e) //GridView1綁定data source的資料後執行的內容
        {
            e.Row.Cells[1].Visible = false; //隱藏game欄位
            e.Row.Cells[3].Visible = false; //隱藏recordtype欄位          

        }



        protected async void Button_Start_Click(object sender, EventArgs e) 
        {
            /*
            Timer_RoomUser.Enabled = true;
            Timer_RoomUser.Interval = 1000;
            await InitialDeal();
            */
            /*
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            String user_name = Convert.ToString(Session["user"]);
            int order = room_user.IndexOf(user_name);
            Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "GameStart";
            */

            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            if (room_number == null)
            {
                await InitialDeal();
            }
            else
            {
                Application[game_name + "_" + room_number + "_deal_status"] = "GameStart";
            }  
                   
        }

        protected async void Button_PlayerAction_Click(object sender, EventArgs e)
        {
            await PlayerAction(sender);
        }
        /*
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
                    Application[game_name + "_" + room_number+ "_pork_"+ "player" + (order + 1)] = porkpath + "bicycle_backs.jpg";
                    Application[game_name + "_" + room_number  + "_action_" + "player" + (order + 1)] = "Pass";
                    Application[game_name + "_" + room_number + "_status"] = "PlayerDeal";

                    Button_Bet.Enabled = true;
                    Button_Pass.Enabled = true;
                }          
            }
        }
        */
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
            //roomMonitor();
        }

        protected async void Timer_RoomUser_Tick(object sender, EventArgs e)
        {
            await roomMonitor();
        }

        protected async void Timer_Battle_Tick(object sender, EventArgs e)
        {
            String game_name = Convert.ToString(Session["game"]);
            String room_number = Convert.ToString(Request.QueryString["id"]);
            Boolean IsBattleStart = Convert.ToBoolean(Application[game_name + "_IsBattleStart_" + room_number]);
            if (IsBattleStart)
            {
                await InitialDeal();
                //Timer_RoomUser.Enabled = false;
            }
        }

        /*protected void Timer_GameProcess_Tick(object sender, EventArgs e)
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
                        //UpdateScore_Opening();
                    }
                    break;
                case "PlayerDeal":
                    {
                        PlayerAction(sender);
                    }
                    break;
                case "GameResults":
                    {
                        GameResults();                        
                    }
                    break;
                case "UpdateScore_Action":
                    {                        
                        //UpdateScore_Action();
                    }
                    break;
            }           
        }
        */

        #endregion

        #region "game operation"


        protected async Task roomMonitor()  //根據遊戲人數進行版面調整
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
                /*
                if (await GameStatusCheck("GameStart", false))
                {
                    Application[game_name + "_" + room_number + "_deal_status"] = "GameStart";
                }
                */
                
                //String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)]);
                String deal_status = Convert.ToString(Application[game_name + "_" + room_number + "_deal_status"]);

               if (deal_status.Equals("GameStart"))
                {        
                    await InitialDeal();
                    //Timer_RoomUser.Enabled = false;
                    //Timer_Battle.Enabled = true;
                    //Timer_Battle.Interval = 1000;
                }
                else if(deal_status.Equals("RandomDeal_First") || deal_status.Equals("RandomDeal") || deal_status.Equals("ReSet"))
                {
                    
                }
                else
                {
                    String user_name = Convert.ToString(Session["user"]);
                    ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                    int order = room_user.IndexOf(user_name);
                    int count = room_user.Count;

                    Session["order"] = order;

                    for (int i = 0; i <= 3; i++)
                    {
                        if (i <= count - 1)
                        {
                            ((Image)player[i]).Visible = true;

                            if (i == order)
                            {
                                ((Label)player_name[i]).Text = Convert.ToString(room_user[i]) + " (me)";
                            }
                            else
                            {
                                ((Label)player_name[i]).Text = Convert.ToString(room_user[i]);
                            }

                            Application[game_name + "_" + room_number + "_score_" + "player" + (i + 1)] = 50000;
                        }
                        else
                        {
                            ((Image)player[i]).Visible = false;
                            ((Label)player_name[i]).Text = "";
                            Application[game_name + "_" + room_number + "_score_" + "player" + (i + 1)] = "";
                        }
                    }
                }

                
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
                pork = (ArrayList)Application["pork" + room_number];

                n = pork.Count;                
                String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_deal_status"]);

                if (user.Equals("deal1") && user_status.Equals("GameStart"))
                {
                    Application[game_name + "_" + room_number + "_deal_status"] = "RandomDeal_First";
                    m = random.Next(1, n + 1) - 1;
                    picture = pork[m].ToString();
                    Application[game_name + "_" + room_number + "_pork_" + user] = picture;

                    if (IsRemove)
                    {
                        pork.RemoveAt(m);
                        Application["pork" + room_number] = pork;
                    }

                }
                else if(user.Equals("deal2") && user_status.Equals("RandomDeal_First"))
                {
                    Application[game_name + "_" + room_number + "_deal_status"] = "RandomDeal";
                    m = random.Next(1, n + 1) - 1;
                    picture = pork[m].ToString();
                    Application[game_name + "_" + room_number + "_pork_" + user] = picture;

                    if (IsRemove)
                    {
                        pork.RemoveAt(m);
                        Application["pork" + room_number] = pork;
                    }

                }
                else if(!user.Equals("deal1") && !user.Equals("deal2"))
                {
                    //Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                    m = random.Next(1, n + 1) - 1;
                    picture = pork[m].ToString();
                    Application[game_name + "_" + room_number + "_pork_" + user] = picture;

                    if (IsRemove)
                    {
                        pork.RemoveAt(m);
                        Application["pork" + room_number] = pork;
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

        protected async Task pokeOpening(Image image, String newImageUrl)
        {
            do
            {
                ((Image)image).ImageUrl = newImageUrl;
            } while (((Image)image).ImageUrl != newImageUrl);
            
            await Task.Run(() => { _ = Loading(); });
        }


        protected async Task InitialDeal()
        {
            ArrayList player = new ArrayList();
            player.Add(Image_player1);
            player.Add(Image_player2);
            player.Add(Image_player3);
            player.Add(Image_player4);

            for (int i=0;i<=3;i++)
            {
                ((Image)player[i]).ImageUrl = porkpath + "bicycle_backs.jpg";
            }


            Button_Start.Enabled = false;
            Button_Bet.Enabled = true;
            Button_Pass.Enabled = true;
            //await ButtonEnabledControl("Processing");

            String room_number = Convert.ToString(Request.QueryString["id"]);

            if (room_number == null)
            {
                RandomPork(room_number, "deal1", Image_deal1);
                RandomPork(room_number, "deal2", Image_deal2);

                await Opening ();
            }
            else
            {
                int order = Convert.ToInt32(Session["order"]);
                String game_name = Convert.ToString(Session["game"]);
                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "InitialDeal";
                if (await GameStatusCheck("InitialDeal"))
                {
                }

                Timer_RoomUser.Enabled = false;
                Button_ExitRoom.Enabled = false;

                
                //Application[game_name + "_" + room_number + "_status"] = "InitialDeal";
                ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                //Application[game_name + "_IsBattleStart_" + room_number] = true;
                


                //Session["IsInRoom"] = false;
                /*
                if (await GameStatusCheck("InitialDeal"))
                {

                }
                */

                String user_status;
                /*
                if (!user_status.Equals("RandomDeal"))
                {
                    RandomPork(room_number, "deal1", Image_deal1);
                    RandomPork(room_number, "deal2", Image_deal2);
                }
                */
                do
                {
                    user_status = Convert.ToString(Application[game_name + "_" + room_number + "_deal_status"]);
                    
                    RandomPork(room_number, "deal1", Image_deal1);
                    RandomPork(room_number, "deal2", Image_deal2);                   
                    
                } while (!user_status.Equals("RandomDeal"));                


                String deal1_new = Convert.ToString(Application[game_name + "_" + room_number + "_pork_" + "deal1"]);
                String deal2_new = Convert.ToString(Application[game_name + "_" + room_number + "_pork_" + "deal2"]);

                //Image_deal1.ImageUrl = deal1_new;
                //Image_deal2.ImageUrl = deal2_new;
                await pokeOpening(Image_deal1, deal1_new);
                await pokeOpening(Image_deal2, deal2_new);



                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "OpeningCheck";

                /*
                if (Image_deal1.ImageUrl.Equals(deal1_new) && Image_deal2.ImageUrl.Equals(deal2_new))
                {
                    Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "OpeningCheck";
                }
                */
                if (await GameStatusCheck("OpeningCheck"))
                {
                    //Application[game_name + "_" + room_number + "_status"] = "OpeningCheck";
                    //Application[game_name + "_IsBattleStart_" + room_number] = false; 
                    await Opening ();
                }   
            }

        }

        protected async Task Opening() //發牌
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            ArrayList pork;            
            int least_pork;

            if (room_number == null)
            {
                pork = (ArrayList)Session["pork"];
                Label_Count.Text = "本輪剩下" + ((pork.Count / 2) - 1).ToString() + "場";
                Label_Count.Font.Size = FontUnit.Small;
                Label_Count.Font.Bold = false;
                least_pork = 3;

                if (Math.Abs(ChangeToNumber(Image_deal1) - ChangeToNumber(Image_deal2)) <= 1)
                {
                    Label_Result.Text = "You Lose! Please start again.";

                    Label_NowCoin.Text = (Convert.ToInt32(Label_NowCoin.Text) - Convert.ToInt32(TextBox_BetCoin.Text)).ToString();

                    Button_Start.Enabled = true;
                    Button_Bet.Enabled = false;
                    Button_Pass.Enabled = false;
                    //await ButtonEnabledControl("ReStart");

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
                    Label_Result.Text = "Bet or Pass?";
                }

            }
            else
            {                
                pork = (ArrayList)Application["pork" + room_number];
                int order = Convert.ToInt32(Session["order"]);
                String game_name = Convert.ToString(Session["game"]);


                Label_Count.Text = "本輪剩下" + ((pork.Count / 2) - 1).ToString() + "場";
                Label_Count.Font.Size = FontUnit.Small;
                Label_Count.Font.Bold = false;

                //Button_Start.Enabled = false;
                //Button_Bet.Enabled = true;
                //Button_Pass.Enabled = true;
                await ButtonEnabledControl("Processing");

                ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                least_pork = count + 2;
                await ButtonEnabledControl("Processing");

                
                Application[game_name + "_" + room_number + "_reDeal"] = false;

                ArrayList player = new ArrayList();
                player.Add(Image_player1);
                player.Add(Image_player2);
                player.Add(Image_player3);
                player.Add(Image_player4);

                if (Math.Abs(ChangeToNumber(Image_deal1) - ChangeToNumber(Image_deal2)) <= 1)
                {
                    Label_Result.Text = "You Lose! Please start again.";

                    
                    String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)]);
                    Button_Start.Enabled = true;
                    Button_Bet.Enabled = false;
                    Button_Pass.Enabled = false;

                    //if (!user_status.Equals("UpdateScore_Label"))
                    //{
                    int newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]) - Convert.ToInt32(TextBox_BetCoin.Text);


                    if (newCoin <= 0)
                    {
                        porkInitial();
                        coinInitial();
                        Label_Count.Text = "輸到脫褲子! 重新開局!";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                        //Button_Start.Enabled = true;
                        await ButtonEnabledControl("ReStart");
                    }
                    else if (pork.Count < least_pork)
                    {
                        porkInitial();
                        Label_Count.Text = "本輪已結束，新的一輪即將開始";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                        //Button_Start.Enabled = true;
                        await ButtonEnabledControl("ReStart");
                    }

                    Label_NowCoin.Text = newCoin.ToString();

                    //if (Label_NowCoin.Text.Equals(newCoin.ToString()))
                    //{
                    //    Application[game_name + "_" + room_number  + "_status_" + "player" + (order + 1)] = "UpdateScore_Label";
                    //}
                    //}
                    //else
                    //{
                    Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)] = Label_NowCoin.Text;
                    //String tempCoin = Convert.ToString(Application[game_name + "_" + room_number+ "_score_" + "player" + (order + 1)]);
                    //if (tempCoin.Equals(Label_NowCoin.Text))
                    //{
                    Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "OpeningScore";
                    //}

                    //if (GameStatusCheck("UpdateScore_Opening"))
                    //{
                    //    Application[game_name + "_" + room_number + "_status"] = "UpdateScore_Opening";
                    //    Application[game_name + "_" + room_number + "_reDeal"] = true;
                    //}
                    //} 

                    ArrayList player_score = new ArrayList();
                    player_score.Add(Label_score1);
                    player_score.Add(Label_score2);
                    player_score.Add(Label_score3);
                    player_score.Add(Label_score4);

                    ArrayList newScore = await Listening_Application(game_name + "_" + room_number + "_score_" + "player", "OpeningScore");
                    for (int i = 0; i <= newScore.Count - 1; i++)
                    {
                        ((Label)player_score[i]).Text = Convert.ToString(newScore[i]);
                    }

                    Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "ReStart";
                    if (await GameStatusCheck("ReStart"))
                    {
                        //await InitialDeal();
                        Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                        Timer_RoomUser.Enabled = true;
                        Timer_RoomUser.Interval = 500;
                    }

                }
                else
                {
                    Label_Result.Text = "Bet or Pass?";
                }
            }
        }

        protected async Task PlayerAction(object sender)
        {
            ArrayList player = new ArrayList();
            player.Add(Image_player1);
            player.Add(Image_player2);
            player.Add(Image_player3);
            player.Add(Image_player4);

            Button_Bet.Enabled = false;
            Button_Pass.Enabled = false;
          

            String room_number = Convert.ToString(Request.QueryString["id"]);
            ArrayList pork;
            int least_pork;
            
            if (room_number == null)
            {
                Button_Start.Enabled = true;

                pork = (ArrayList)Session["pork"];
                least_pork = 3;

                if (sender == Button_Bet)
                {  
                    RandomPork(room_number, "player1", Image_player1, false);
                    await Bet ();                    

                    if (pork.Count < least_pork)
                    {
                        porkInitial();
                        Label_Count.Text = "本輪已結束，新的一輪即將開始";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                    }
                }
                else if(sender == Button_Pass)
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
                        //await InitialDeal ();
                        //Timer_RoomUser.Enabled = true;
                        //Timer_RoomUser.Interval = 500;


                    }
                }                
            }
            else
            {
                String game_name = Convert.ToString(Session["game"]);
                int order = Convert.ToInt32(Session["order"]);
                ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                pork = (ArrayList)Application["pork" + room_number];
                least_pork = 2 + room_user.Count;
                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "PlayerAction";

                if (sender == Button_Bet)
                {
                    Application[game_name + "_" + room_number + "_action_" + "player" + (order + 1)] = "Bet";
                    RandomPork(room_number, "player" + (order + 1), (Image)player[order], false);

                    //Application[game_name + "_" + room_number +"_status"] = "PlayerDeal";

                    
                    
                    //String Action;
                    
                    //bool AllAction = true;
                    //String newUrl;

                    /*
                    for (int i = 0; i <= room_user.Count - 1; i++)
                    {
                        Action = Convert.ToString(Application[game_name + "_" + room_number + "_action_" + "player" + (i + 1)]);
                        if (Action.Equals("Bet"))
                        {
                            newUrl = Convert.ToString(Application[game_name + "_" + room_number + "_pork_" + "player" + (i + 1)]);
                            ((Image)player[i]).ImageUrl = newUrl;
                        }
                        else if (Action.Equals("Pass"))
                        {
                            Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                        }
                        else
                        {
                            AllAction = false;
                        }
                    }


                    if (((Image)player[order]).ImageUrl.Equals(Convert.ToString(Application[game_name + "_" + room_number + "_pork_" + "player" + (order + 1)])))
                    {
                        Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "GameResults";
                    }



                    // Application[game_name+"_" + room_number + "_action_"+ "player" + (order + 1) ] = false;

                    if (AllAction)
                    {
                        if (GameStatusCheck("GameResults"))
                        {
                            //for (int i = 0; i <= room_user.Count - 1; i++)
                            //{
                            //    Application[game_name + "_" + room_number + "_action_"+ "player" + (order + 1) ] = "None";
                            //}

                            Application[game_name + "_" + room_number + "_status"] = "GameResults";
                        }
                    }
                    */
                }
                else if (sender == Button_Pass)
                {
                    Application[game_name + "_" + room_number + "_action_" + "player" + (order + 1)] = "Pass";                               
                    //Application[game_name + "_" + room_number + "_pork_" + "player" + (order + 1)] = porkpath + "bicycle_backs.jpg";
                   
                    //Application[game_name + "_" + room_number + "_status"] = "PlayerDeal";

                    //Button_Bet.Enabled = true;
                    //Button_Pass.Enabled = true;
                   
                }

                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "PorkOpening";
                await Listening_pork();
            }
        }


        protected async Task ButtonEnabledControl(String Status)
        {
            switch(Status)
            {
                case "Processing":
                    {
                        do
                        {
                            Button_Start.Enabled = false;
                            Button_Bet.Enabled = true;
                            Button_Pass.Enabled = true;
                            await Task.Run(() => { _ = Loading(); });
                        } while(Button_Start.Enabled || !Button_Bet.Enabled || !Button_Pass.Enabled);    
                                                
                    }
                    break;
                case "ReStart":
                    {
                        do
                        {
                            Button_Start.Enabled = true;
                            Button_Bet.Enabled = false;
                            Button_Pass.Enabled = false;
                            await Task.Run(() => { _ = Loading(); });
                        } while (!Button_Start.Enabled || Button_Bet.Enabled || Button_Pass.Enabled);
                    }
                    break;
            }

            

        }

        protected async Task<Boolean> GameStatusCheck(String Status, Boolean IsWaiting = true)
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            int count = room_user.Count;
            Boolean IsListening = true;
            String playerStatus = "";

            if(IsWaiting)
            {
                do
                {
                    IsListening = true;
                    for (int i = 0; i <= count - 1; i++)
                    {
                        playerStatus = Convert.ToString(Application[game_name + "_" + room_number + "_status" + "_" + "player" + (i + 1)]);

                        await Task.Run(() => { _ = Loading(); });

                        if (!playerStatus.Equals(Status))
                        {
                            IsListening = false;
                        }
                    }

                } while (!IsListening);

                //return true;
            }
            else
            {
                for (int i = 0; i <= count - 1; i++)
                {
                    playerStatus = Convert.ToString(Application[game_name + "_" + room_number + "_status" + "_" + "player" + (i + 1)]);

                    //await Task.Run(() => { _ = Loading(); });

                    if (!playerStatus.Equals(Status))
                    {
                        IsListening = false;
                    }
                }

                
            }

            return IsListening;

        }

        private async Task Loading()
        {
            await Task.Delay(20);
        }


        protected async Task<ArrayList> Listening_Application(String ApplicationName, String Status)
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
                IsListening = true;
                for (int i = 0; i <= count - 1; i++)
                {
                    content = Convert.ToString(Application[ApplicationName + (i + 1)]);

                    if (content == null || content == "")
                    {
                        IsListening = false;
                    }
                }

            } while (!IsListening);


            if (await GameStatusCheck(Status))
            {
                for (int i = 0; i <= count - 1; i++)
                {
                    Results.Add(Convert.ToString(Application[ApplicationName + (i + 1)]));
                    //Application.Remove(ApplicationName + (i + 1));
                }
            }
            return Results;
        }

        /*
        protected Boolean GameStart(String Status)
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            int count = room_user.Count;
            Boolean IsListening = true;
            String playerStatus = "";

            do
            {
                IsListening = true;
                for (int i = 0; i <= count - 1; i++)
                {
                    playerStatus = Convert.ToString(Application[game_name + "_" + room_number + "_status" + "_" + "player" + (i + 1)]);

                    if (!playerStatus.Equals(Status))
                    {
                        IsListening = false;
                    }
                }

            } while (!IsListening);

            return true;
        }
        */

        protected async Task Listening_pork()
        {
            String oldImageUrl;
            String newImageUrl;
            Boolean IsListening = true;

            ArrayList player = new ArrayList();
            player.Add(Image_player1);
            player.Add(Image_player2);
            player.Add(Image_player3);
            player.Add(Image_player4);

            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            int count = room_user.Count;
            int order = Convert.ToInt32(Session["order"]);

            String ApplicationName = game_name + "_" + room_number + "_action_" + "player";
            ArrayList Action = await Listening_Application(ApplicationName, "PorkOpening");
            //ArrayList newImageUrl = Listening_Application(game_name + "_" + room_number + "_pork_" + "player"); 

            String content = "";

            do
            {
                IsListening = true;
                for (int i = 0; i <= count - 1; i++)
                {
                    if (Convert.ToString(Action[i]).Equals("Bet"))
                    {
                        oldImageUrl = ((Image)player[i]).ImageUrl;
                        newImageUrl = Convert.ToString(Application[game_name + "_" + room_number + "_pork_" + "player" + (i + 1)]);                        
                        ((Image)player[i]).ImageUrl = newImageUrl;
                        content = ((Image)player[i]).ImageUrl;
                        if (newImageUrl == oldImageUrl || !content.Equals(newImageUrl))
                        {
                            IsListening = false;
                        }
                    }
                }
            } while (!IsListening);
            /*
            Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "pork_opening";

            if(GameStatusCheck("pork_opening"))
            {
                for (int i = 0; i <= count - 1; i++)
                {
                    Application[ApplicationName + (i + 1)] = "";
                }
            }
            */

            String userAction = Convert.ToString(Action[order]);
            await GameResults(userAction);

        }

        

        protected async Task GameResults(String Action = "")
        {
            String room_number = Convert.ToString(Request.QueryString["id"]);
            String game_name = Convert.ToString(Session["game"]);
            ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            ArrayList pork = (ArrayList)Application["pork" + room_number];
            int least_pork = 2 + room_user.Count;

            await Bet(Action);

            Button_Start.Enabled = true;
            Button_Bet.Enabled = false;
            Button_Pass.Enabled = false;            
            //await ButtonEnabledControl("ReStart");

            if (pork.Count < least_pork)
            {
                porkInitial();
                Label_Count.Text = "本輪已結束，新的一輪即將開始";
                Label_Count.Font.Size = FontUnit.Larger;
                Label_Count.Font.Bold = true;
            }
        }
        protected async Task Bet(String Action = "") //下注跟進
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
                        //Button_Start.Enabled = true;                        
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
                        //Button_Start.Enabled = true;                       
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
                //String Action = Convert.ToString(Application[game_name + "_" + room_number + "_action_"  + "player" + (order + 1)]);

                int newCoin;
                //String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)]);
                //ArrayList Action = Listening_Application(game_name + "_" + room_number + "_action_" + "player");
                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "ScoreUpdate";


                //if(!user_status.Equals("UpdateScore_Label"))
                //{

                if (Action.Equals("Pass"))
                {
                    Label_Result.Text = "Pass";
                    newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]);
                    //Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                }
                else if ((ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal1)) * (ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal2)) == 0)
                {
                    Label_Result.Text = "Bump! You Lose!";
                    newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]) - 2 * Convert.ToInt32(TextBox_BetCoin.Text);
                }
                else if ((ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal1)) * (ChangeToNumber((Image)player[order]) - ChangeToNumber(Image_deal2)) > 0)
                {
                    Label_Result.Text = "Out of door! You Lose!";
                    newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]) - Convert.ToInt32(TextBox_BetCoin.Text);
                }
                else
                {
                    Label_Result.Text = "In door! You Win!";
                    newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]) + Convert.ToInt32(TextBox_BetCoin.Text);
                }

                if (newCoin <= 0)
                {
                    porkInitial();
                    coinInitial();
                    Label_Count.Text = "輸到脫褲子! 重新開局!";
                    Label_Count.Font.Size = FontUnit.Larger;
                    Label_Count.Font.Bold = true;
                    //Button_Start.Enabled = true;
                    //await ButtonEnabledControl("ReStart");
                }
                else
                {
                    
                    Label_NowCoin.Text = newCoin.ToString();

                    //if (Label_NowCoin.Text.Equals(newCoin.ToString()))
                    //{
                    //Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "UpdateScore_Label";


                    //}
                    Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)] = Label_NowCoin.Text;
                    Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "ActionScore";
                    ArrayList newScore = await Listening_Application(game_name + "_" + room_number + "_score_" + "player", "ActionScore");
                    for (int i = 0; i <= newScore.Count - 1; i++)
                    {
                        ((Label)player_score[i]).Text = Convert.ToString(newScore[i]);
                    }

                    Timer_RoomUser.Enabled = true;
                    Timer_RoomUser.Interval = 500;
                    Application[game_name + "_" + room_number + "_deal_status"] = "ReSet";
                    //Button_Start.Enabled = true;
                    //await ButtonEnabledControl("ReStart");


                    //Application[game_name + "_IsBattleStart_" + room_number] = false;


                }
                //}
                //else
                //{       
                //Application[game_name + "_" + room_number  + "_score_" + "player" + (order + 1)] = Label_NowCoin.Text;
                //String tempCoin = Convert.ToString(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]);
                //if (tempCoin.Equals(Label_NowCoin.Text))
                //{
                //    Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "UpdateScore_Action";
                //}

                //if (GameStatusCheck("UpdateScore_Action"))
                //{
                //    for (int i = 0; i <= room_user.Count - 1; i++)
                //    {
                //        Application[game_name + "_" + room_number + "_action_"+ "player" + (order + 1)] = "None";
                //    }
                //
                //    Application[game_name + "_" + room_number + "_status"] = "UpdateScore_Action";
                //}
                //} 

            }
        }

        




        /*
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
                ((Label)player_score[i]).Text = Convert.ToString(Application[game_name + "_" + room_number + "_score" + "_" + "player" + (i + 1)]);

              if (Label_NowCoin.Text.Equals(((Label)player_score[order]).Text))
              {
                  Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "PlayerDeal";
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
                ((Label)player_score[i]).Text = Convert.ToString(Application[game_name + "_" + room_number+ "_score_" + "player" + (i + 1)]);

                if (Label_NowCoin.Text.Equals(((Label)player_score[order]).Text))
                {
                    Application[game_name + "_" + room_number + "_status" + "_" + "player" + (order + 1)] = "InitialDeal";
                }
               
            }

            if (GameStatusCheck("InitialDeal"))
            {
                Application[game_name + "_" + room_number + "_status"] = "InitialDeal";
            }
        }
        */



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
            Timer_RoomUser.Enabled = true;
            Timer_RoomUser.Interval = 500;

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

                //Session["IsInRoom"] = true;                
                Response.Redirect(new_url); //跳轉至有帶有房號的網址
            }
            else
            {
                MessageBox.Show("請輸入房號!");
            }
        }


        protected void ExitRoom(String room_number, String game_name, String user_name, int max_user)
        {
            Timer_RoomUser.Enabled = false;

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
            
            Response.Redirect(game_name);
        }








        #endregion






       


    }

}