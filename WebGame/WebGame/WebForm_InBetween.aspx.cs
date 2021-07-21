using Microsoft.Ajax.Utilities;
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
        String porkpath = "~/pic/poker/";        
        String room_number;
        String game_name;
        String user_name;

        #region "event"

        protected async void Page_Load(object sender, EventArgs e) //網頁刷新時執行的內容，基本上所有方法或事件執行前都會先進行網頁刷新
        {
            room_number = Convert.ToString(Request.QueryString["id"]); //以get方式取得網址上"?id="後面的房號
            game_name = Convert.ToString(Session["game"]);
            user_name = Convert.ToString(Session["user"]);
            Session["room"] = room_number;

            if (!IsPostBack)
            { 
                if (Convert.ToString(Session["game"]) == "" || Convert.ToString(Session["user"]) == "")
                {
                    Response.Redirect("WebForm_GameSelection", false);
                }

                Label_Hello.Text = "Hello! " + Convert.ToString(Session["user"]);
                Label_Hello.Font.Size = FontUnit.Larger;
                Label_Hello.Font.Bold = true;                

                porkInitial();


                if (room_number == null)
                {
                    Button_Record.Text = "Record and ReSet";
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
                    Button_Record.Text = "ReSet";
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

            if (room_number == null)
            {
                Class_Game newGame = new Class_Game();
                newGame.rank("InBetween", "score", GridView1);
            }
        }

        protected void GridView1_RawDataBound(object sender, GridViewRowEventArgs e) //GridView1綁定data source的資料後執行的內容
        {
            e.Row.Cells[1].Visible = false; //隱藏game欄位
            e.Row.Cells[3].Visible = false; //隱藏recordtype欄位          

        }



        protected async void Button_Start_Click(object sender, EventArgs e) 
        {           
            //String room_number = Convert.ToString(Request.QueryString["id"]);
            //String game_name = Convert.ToString(Session["game"]);
            if (room_number == null)
            {
                await InitialDeal();
            }
            else
            {
                Application[game_name + "_" + room_number + "_status"] = "GameStart";
            }  
                   
        }

        protected async void Button_PlayerAction_Click(object sender, EventArgs e)
        {
            await ButtonEnabledControl("WaitResults");            
            await PlayerAction(sender);
        }
        
        protected void Button_Record_Click(object sender, EventArgs e)
        {
            if (room_number == null)
            {
                int nowCoin = Convert.ToInt32(Label_NowCoin.Text);
                if (nowCoin > 0)
                {
                    Class_Game newGame = new Class_Game();
                    newGame.record("InBetween", user_name, "score", nowCoin);
                    newGame.rank("InBetween", "score", GridView1);
                }
                else
                {
                    MessageBox.Show("低於0分沒有存檔價值！");
                }                
            }            

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
            String new_room_number = Convert.ToString(TextBox_Room.Text);
            //String game_name = Convert.ToString(Session["game"]);
            //String user_name = Convert.ToString(Session["user"]);
            int max_user = 4;

            EnterRoom(new_room_number, game_name, user_name, max_user);
        }

        
        protected void ListBox_BattleRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            String room_name = Convert.ToString(ListBox_BattleRoom.SelectedItem);
            TextBox_Room.Text = room_name.Substring(0, room_name.IndexOf(":"));
        }
        
               

        protected void Button_ExitRoom_Click(object sender, EventArgs e)
        {
            //String last_room_number = Convert.ToString(Request.QueryString["id"]); 
            //String game_name = Convert.ToString(Session["game"]);

            if (room_number != null) //若有房號，則在跳轉之前需先執行ExitRoom
            {
                //String user_name = Convert.ToString(Session["user"]);
                int max_user = 4;
                int order = Convert.ToInt32(Session["order"]);
                Application[game_name + "_exit_room_order"] = room_number + ":" + order;
                Timer_RoomUser.Enabled = false;
                Application[game_name + "_" + room_number + "_status"] = "ExitRoom";
                ExitRoom(room_number, game_name, order, max_user);
            }
        }

        protected void Timer_Status_Tick(object sender, EventArgs e)
        {
            //String room_number = Convert.ToString(Request.QueryString["id"]);
            //String game_name = Convert.ToString(Session["game"]);
            Load_Room(ListBox_BattleRoom, room_number, game_name);
            //roomMonitor();
        }

        protected async void Timer_RoomUser_Tick(object sender, EventArgs e)
        {
            await roomMonitor();
        }

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

            ArrayList player_score = new ArrayList();
            player_score.Add(Label_score1);
            player_score.Add(Label_score2);
            player_score.Add(Label_score3);
            player_score.Add(Label_score4);

            //String room_number = Convert.ToString(Request.QueryString["id"]);

            if (room_number == null)
            {
                for (int i = 1; i <= 3; i++)
                {
                    ((Image)player[i]).Visible = false;
                }

            }
            else
            {
                //String game_name = Convert.ToString(Session["game"]);               
                String status = Convert.ToString(Application[game_name + "_" + room_number + "_status"]);

                if (status.Equals("GameStart"))
                {        
                    await InitialDeal();
                    //Timer_RoomUser.Enabled = false;
                }
                else if(status.Equals("RandomDeal_First") || status.Equals("RandomDeal") || status.Equals("ReSet"))
                {
                    
                }
                else
                {
                    //String user_name = Convert.ToString(Session["user"]);
                    await updateOrder();
                    //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                    Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                    //int order = room_user.IndexOf(user_name);
                    int order = Convert.ToInt32(Session["order"]);
                    int count = room_user.Count;
                    int initial_score = 50000;

                    //Session["order"] = order;

                    for (int i = 0; i <= 3; i++)
                    {
                        if (i <= count - 1)
                        {
                            ((Image)player[i]).Visible = true;

                            if (Application[game_name + "_" + room_number + "_score_" + "player" + (i + 1)] == null)
                            {
                                Application[game_name + "_" + room_number + "_score_" + "player" + (i + 1)] = initial_score;
                            }
                            ((Label)player_score[i]).Text = Convert.ToString(Application[game_name + "_" + room_number + "_score_" + "player" + (i + 1)]);

                            if (i == order)
                            {
                                ((Label)player_name[i]).Text = Convert.ToString(room_user[i]) + " (me)";
                                Label_NowCoin.Text = ((Label)player_score[i]).Text;
                            }
                            else
                            {
                                ((Label)player_name[i]).Text = Convert.ToString(room_user[i]);
                            }                       
                        }
                        else
                        {
                            ((Image)player[i]).Visible = false;
                            ((Label)player_name[i]).Text = "";
                            ((Label)player_score[i]).Text = "";
                            Application.Remove(game_name + "_" + room_number + "_status_" + "player" + (i + 1));
                            Application.Remove(game_name + "_" + room_number + "_action_" + "player" + (i + 1));
                            Application.Remove(game_name + "_" + room_number + "_pork_" + "player" + (i + 1));
                            Application.Remove(game_name + "_" + room_number + "_score_" + "player" + (i + 1));
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

            //String room_number = Convert.ToString(Request.QueryString["id"]);

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
            if (room_number == null)
            {
                Label_NowCoin.Text = "50000";
                TextBox_BetCoin.Text = "1000";
            }
            else
            {
                ArrayList player_score = new ArrayList();
                player_score.Add(Label_score1);
                player_score.Add(Label_score2);
                player_score.Add(Label_score3);
                player_score.Add(Label_score4);

                Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                int initial_score = 50000;

                for (int i =0;i<=count-1;i++)
                {
                    Application[game_name + "_" + room_number + "_score_" + "player" + (i + 1)] = initial_score;
                    ((Label)player_score[i]).Text = initial_score.ToString();
                }
            }

            
        }

        protected void RandomPork(String room_number, String user, Image image, bool IsRemove = true) //隨機發牌
        {
            ArrayList pork;            
            int n;
            int m;
            String picture;
            Random random = new Random();

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
                //String game_name = Convert.ToString(Session["game"]);                
                              
                String room_status = Convert.ToString(Application[game_name + "_" + room_number + "_status"]);

                if (user.Equals("deal1") && room_status.Equals("GameStart"))
                {                    
                    Application[game_name + "_" + room_number + "_status"] = "RandomDeal_First";
                    pork = (ArrayList)Application["pork" + room_number];
                    n = pork.Count;
                    m = random.Next(1, n + 1) - 1;
                    picture = pork[m].ToString();
                    Application[game_name + "_" + room_number + "_pork_" + user] = picture;

                    if (IsRemove)
                    {
                        pork.RemoveAt(m);
                        Application["pork" + room_number] = pork;
                    }

                }
                else if(user.Equals("deal2") && room_status.Equals("RandomDeal_First"))
                {
                    Application[game_name + "_" + room_number + "_status"] = "RandomDeal";
                    pork = (ArrayList)Application["pork" + room_number];
                    n = pork.Count;
                    m = random.Next(1, n + 1) - 1;
                    picture = pork[m].ToString();
                    Application[game_name + "_" + room_number + "_pork_" + user] = picture;

                    if (IsRemove)
                    {
                        pork.RemoveAt(m);
                        Application["pork" + room_number] = pork;
                        Application["pork2" + room_number] = pork;
                    }

                }
                else if(!user.Equals("deal1") && !user.Equals("deal2"))
                {
                    if (IsRemove)
                    {
                        pork = (ArrayList)Application["pork" + room_number];
                    }
                    else
                    {
                        pork = (ArrayList)Application["pork2" + room_number];
                    }
                        
                    n = pork.Count;
                    m = random.Next(1, n + 1) - 1;
                    picture = pork[m].ToString();
                    Application[game_name + "_" + room_number + "_pork_" + user] = picture;

                    pork.RemoveAt(m);
                    if (IsRemove)
                    {                        
                        Application["pork" + room_number] = pork;                       
                    }
                    else
                    {                        
                        Application["pork2" + room_number] = pork;
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
            
            await Task.Run(() => { _ = Load(20); });
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


            //Button_Start.Enabled = false;
            //Button_Bet.Enabled = true;
            //Button_Pass.Enabled = true;            
            await ButtonEnabledControl("WaitResults");

            //String room_number = Convert.ToString(Request.QueryString["id"]);

            if (room_number == null)
            {
                RandomPork(room_number, "deal1", Image_deal1);
                await Task.Delay(20);
                RandomPork(room_number, "deal2", Image_deal2);

                await Opening ();
            }
            else
            {
                int order = Convert.ToInt32(Session["order"]);
                //String game_name = Convert.ToString(Session["game"]);
                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "InitialDeal";                

                if (await StatusGate("InitialDeal", "OpeningCheck"))
                {
                }

                Button_ExitRoom.Enabled = false;
                Timer_RoomUser.Enabled = false;                

                //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                int count = room_user.Count;
               
                String room_status = Convert.ToString(Application[game_name + "_" + room_number + "_status"]);

                
                while (!room_status.Equals("RandomDeal"))
                {
                    if(room_status.Equals("GameStart"))
                    {
                        RandomPork(room_number, "deal1", Image_deal1);
                        await Task.Delay(20);
                        RandomPork(room_number, "deal2", Image_deal2);
                    }                    

                    room_status = Convert.ToString(Application[game_name + "_" + room_number + "_status"]);
                }
               

                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "Dealed";

                if (await StatusGate("Dealed", "OpeningCheck"))
                {
                }

                String deal1_new = Convert.ToString(Application[game_name + "_" + room_number + "_pork_" + "deal1"]);
                String deal2_new = Convert.ToString(Application[game_name + "_" + room_number + "_pork_" + "deal2"]);
                               
                await pokeOpening(Image_deal1, deal1_new);
                await pokeOpening(Image_deal2, deal2_new);

                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "OpeningCheck";
                               

                if (await StatusGate("OpeningCheck", "OpeningScore"))
                {                    
                    await Opening();
                }
            }

        }

        protected async Task Opening() //發牌
        {
            //String room_number = Convert.ToString(Request.QueryString["id"]);
            ArrayList pork;            
            int least_pork;

            if (room_number == null)
            {
                pork = (ArrayList)Session["pork"];
                Label_Count.Text = "本輪剩下" + ((pork.Count / 2) - 1).ToString() + "場";
                Label_Count.Font.Size = FontUnit.Medium;
                Label_Count.Font.Bold = false;
                least_pork = 3;

                if (Math.Abs(ChangeToNumber(Image_deal1) - ChangeToNumber(Image_deal2)) <= 1)
                {
                    Label_Result.Text = "You Lose! Please start again.";
                    Label_NowCoin.Text = (Convert.ToInt32(Label_NowCoin.Text) - Convert.ToInt32(TextBox_BetCoin.Text)).ToString();
                    

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

                    Button_Record.Enabled = true;
                    await ButtonEnabledControl("GameStart");

                }
                else
                {
                    Label_Result.Text = "Bet or Pass?";
                    await ButtonEnabledControl("WaitAction");
                }

            }
            else
            {                
                pork = (ArrayList)Application["pork" + room_number];
                int order = Convert.ToInt32(Session["order"]);
                //String game_name = Convert.ToString(Session["game"]);


                Label_Count.Text = "本輪剩下" + ((pork.Count / 2) - 1).ToString() + "場";
                Label_Count.Font.Size = FontUnit.Medium;
                Label_Count.Font.Bold = false;

                Button_Start.Enabled = false;
                Button_Bet.Enabled = true;
                Button_Pass.Enabled = true;
                //await ButtonEnabledControl("Processing");

                //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                least_pork = count + 2;
                //await ButtonEnabledControl("Processing");

                
                //Application[game_name + "_" + room_number + "_reDeal"] = false;

                ArrayList player = new ArrayList();
                player.Add(Image_player1);
                player.Add(Image_player2);
                player.Add(Image_player3);
                player.Add(Image_player4);

                if (Math.Abs(ChangeToNumber(Image_deal1) - ChangeToNumber(Image_deal2)) <= 1)
                {
                    Label_Result.Text = "You Lose! Please start again.";

                    
                    //String user_status = Convert.ToString(Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)]);
                    
                    int newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]) - Convert.ToInt32(TextBox_BetCoin.Text);


                    if (newCoin <= 0)
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
                        Button_ExitRoom.Enabled = true;
                        Application[game_name + "_" + room_number + "_status"] = "NewGame";
                    }

                    Label_NowCoin.Text = newCoin.ToString();
                 
                    Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)] = Label_NowCoin.Text;                 
                    Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "OpeningScore";

                    if (await StatusGate("OpeningScore", "OpeningScore"))
                    {
                    }

                    ArrayList player_score = new ArrayList();
                    player_score.Add(Label_score1);
                    player_score.Add(Label_score2);
                    player_score.Add(Label_score3);
                    player_score.Add(Label_score4);
                                        

                    ArrayList newScore = await Listening_Application(game_name + "_" + room_number + "_score_" + "player");
                    for (int i = 0; i <= newScore.Count - 1; i++)
                    {
                        ((Label)player_score[i]).Text = Convert.ToString(newScore[i]);
                    }

                    //Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "ReStart";
                  
                    //if (await StatusGate("ReStart", "InitialDeal"))
                    //{
                        //await InitialDeal();
                        Application[game_name + "_" + room_number + "_status"] = "ReSet";
                        Button_Record.Enabled = true;
                        Timer_RoomUser.Enabled = true;
                        Timer_RoomUser.Interval = 500;
                    //}

                    await ButtonEnabledControl("GameStart");

                }
                else
                {
                    Label_Result.Text = "Bet or Pass?";
                    await ButtonEnabledControl("WaitAction");
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

            //String room_number = Convert.ToString(Request.QueryString["id"]);
            ArrayList pork;
            int least_pork;
            
            if (room_number == null)
            {
                Button_Start.Enabled = true;

                pork = (ArrayList)Session["pork"];
                least_pork = 3;

                if (sender == Button_Bet)
                {
                    Label_Result.Text = "Bet";
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
                    Label_Result.Text = "Pass";
                    if (pork.Count < least_pork)
                    {
                        porkInitial();
                        Label_Count.Text = "本輪已結束，新的一輪即將開始";
                        Label_Count.Font.Size = FontUnit.Larger;
                        Label_Count.Font.Bold = true;
                    }
                   
                }                
            }
            else
            {
                //String game_name = Convert.ToString(Session["game"]);
                int order = Convert.ToInt32(Session["order"]);
                //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                pork = (ArrayList)Application["pork" + room_number];
                least_pork = 2 + room_user.Count;
                //Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "PlayerAction";

                if (sender == Button_Bet)
                {
                    Label_Result.Text = "Bet";
                    Application[game_name + "_" + room_number + "_action_" + "player" + (order + 1)] = "Bet";
                    RandomPork(room_number, "player" + (order + 1), (Image)player[order], false);

                }
                else if (sender == Button_Pass)
                {
                    Label_Result.Text = "Pass";
                    Application[game_name + "_" + room_number + "_action_" + "player" + (order + 1)] = "Pass"; 
                }

                //Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "PorkOpening";
                await Listening_pork();
            }
        }


        protected async Task ButtonEnabledControl(String Status)
        {
            switch(Status)
            {
                case "GameStart":
                    {
                        do
                        {
                            Button_Start.Enabled = true;
                            Button_Bet.Enabled = false;
                            Button_Pass.Enabled = false;
                            await Task.Run(() => { _ = Load(20); });
                        } while (!Button_Start.Enabled || Button_Bet.Enabled || Button_Pass.Enabled);
                    }
                    break;
                case "WaitAction":
                    {
                        do
                        {
                            Button_Start.Enabled = false;
                            Button_Bet.Enabled = true;
                            Button_Pass.Enabled = true;
                            await Task.Run(() => { _ = Load(20); });
                        } while(Button_Start.Enabled || !Button_Bet.Enabled || !Button_Pass.Enabled);    
                                                
                    }
                    break;
                case "WaitResults":
                    {
                        do
                        {
                            Button_Start.Enabled = false;
                            Button_Bet.Enabled = false;
                            Button_Pass.Enabled = false;
                            await Task.Run(() => { _ = Load(20); });
                        } while (Button_Start.Enabled || Button_Bet.Enabled || Button_Pass.Enabled);

                    }
                    break;

            }
        }

        protected async Task<Boolean> GameStatusCheck(String Status, Boolean IsListening = true)
        {
            //String room_number = Convert.ToString(Request.QueryString["id"]);
            //String game_name = Convert.ToString(Session["game"]);
            //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
            int count = room_user.Count;
            Boolean Results = true;
            String playerStatus = "";
            int match_count = 0;

            if(IsListening)
            {
                do
                {
                    Results = true;
                    match_count = 0;
                    for (int i = 0; i <= count - 1; i++)
                    {
                        playerStatus = Convert.ToString(Application[game_name + "_" + room_number + "_status" + "_" + "player" + (i + 1)]);

                        await Task.Run(() => { _ = Load(20); });

                        if (!playerStatus.Equals(Status))
                        {
                            Results = false;
                            //MessageBox.Show("target status = "+ Status+",\nbut player"+(i+1)+" is"+ playerStatus);
                            Label_Result.Text = "target status = " + Status + ",\nbut player" + (i + 1) + " is" + playerStatus;
                        }
                        else
                        {
                            match_count += 1;
                        }
                    }

                } while (!Results);

                //return true;
            }
            else
            {
                for (int i = 0; i <= count - 1; i++)
                {
                    playerStatus = Convert.ToString(Application[game_name + "_" + room_number + "_status" + "_" + "player" + (i + 1)]);

                    //await Task.Run(() => { _ = Load(); });

                    if (!playerStatus.Equals(Status))
                    {
                        Results = false;
                    }
                }

                
            }

            return Results;

        }

        private async Task Load(int timeout)
        {
            await Task.Delay(timeout);
        }


        protected async Task<Boolean> StatusGate(String Status1, String Status2)
        {
            //String room_number = Convert.ToString(Request.QueryString["id"]);
            //String game_name = Convert.ToString(Session["game"]);
            //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
            int count = room_user.Count;
            Boolean Results = true;
            String playerStatus = "";
            int match_count1 = 0;
            int match_count2 = 0;

           
                do
                {
                    Results = true;
                    match_count1 = 0;
                    match_count2 = 0;
                    for (int i = 0; i <= count - 1; i++)
                    {
                        playerStatus = Convert.ToString(Application[game_name + "_" + room_number + "_status" + "_" + "player" + (i + 1)]);

                        await Task.Run(() => { _ = Load(20); });

                        if (playerStatus.Equals(Status1))
                        {
                            match_count1 += 1;
                        }
                        else if(playerStatus.Equals(Status2))
                        {
                            match_count2 += 1;
                        }
                        else
                        {                            
                            Results = false;
                        }
                    }

                    if(match_count1==0)
                    {
                        Results = false;
                    }

                } while (!Results);
           
            return Results;
        }
               

        protected async Task<ArrayList> Listening_Application(String ApplicationName)
        {
            ArrayList Results = new ArrayList();
            //String room_number = Convert.ToString(Request.QueryString["id"]);
            //String game_name = Convert.ToString(Session["game"]);
            //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
            int count = room_user.Count;
            Boolean IsListening = true;
            String content = "";

            do
            {
                IsListening = true;
                for (int i = 0; i <= count - 1; i++)
                {
                    content = Convert.ToString(Application[ApplicationName + (i + 1)]);

                    await Task.Run(() => { _ = Load(20); });

                    if (content == null || content == "")
                    {
                        IsListening = false;
                    }
                }

            } while (!IsListening);

            
            for (int i = 0; i <= count - 1; i++)
            {
                Results.Add(Convert.ToString(Application[ApplicationName + (i + 1)]));                
            }
            return Results;
        }
       
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

            //String room_number = Convert.ToString(Request.QueryString["id"]);
            //String game_name = Convert.ToString(Session["game"]);
            //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
            int count = room_user.Count;
            int order = Convert.ToInt32(Session["order"]);

            String ApplicationName = game_name + "_" + room_number + "_action_" + "player";            

            ArrayList Action = await Listening_Application(ApplicationName);
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
                        //((Image)player[i]).ImageUrl = newImageUrl;
                        await pokeOpening((Image)player[i], newImageUrl);
                        content = ((Image)player[i]).ImageUrl;
                        if (content.Equals(porkpath + "bicycle_backs.jpg") || content.Equals(oldImageUrl) || !content.Equals(newImageUrl) )
                        {
                            IsListening = false;
                        }
                    }
                }
            } while (!IsListening);
            
            
            Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "PorkOpened";
            if (await StatusGate("PorkOpened", "ScoreUpdated"))
            {

            }
           
            String userAction = Convert.ToString(Action[order]);
            await GameResults(userAction);

        }

        

        protected async Task GameResults(String Action = "")
        {
            //String room_number = Convert.ToString(Request.QueryString["id"]);
            //String game_name = Convert.ToString(Session["game"]);
            //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
            Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
            ArrayList pork = (ArrayList)Application["pork" + room_number];
            int least_pork = 2 + room_user.Count;

            await Bet(Action);            

            if (pork.Count < least_pork)
            {
                porkInitial();
                Label_Count.Text = "本輪已結束，新的一輪即將開始";
                Label_Count.Font.Size = FontUnit.Larger;
                Label_Count.Font.Bold = true;
                Button_ExitRoom.Enabled = true;
                Application[game_name + "_" + room_number + "_status"] = "NewGame";
            }

            await ButtonEnabledControl("GameStart");
        }
        protected async Task Bet(String Action = "") //下注跟進
        {

            //String room_number = Convert.ToString(Request.QueryString["id"]);

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

                Button_Record.Enabled = true;

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

                //String game_name = Convert.ToString(Session["game"]);
                //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];
                Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                int count = room_user.Count;
                int order = Convert.ToInt32(Session["order"]);
               
                int newCoin;                
                //Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "ScoreUpdate";


                if (Action.Equals("Pass"))
                {
                    Label_Result.Text = "Pass";
                    newCoin = Convert.ToInt32(Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)]);
                    //Application[game_name + "_" + room_number + "_status"] = "ReSet";
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
               

                String ApplicationName = game_name + "_" + room_number + "_action_" + "player";
                for (int i = 0; i <= count - 1; i++)
                {
                    Application[ApplicationName + (i + 1)] = "";
                }

                if (newCoin <= 0)
                {
                    porkInitial();
                    coinInitial();
                    Label_Count.Text = "輸到脫褲子! 重新開局!";
                    Label_Count.Font.Size = FontUnit.Larger;
                    Label_Count.Font.Bold = true;                    
                }
                else
                {
                    
                    Label_NowCoin.Text = newCoin.ToString();
                                     
                    Application[game_name + "_" + room_number + "_score_" + "player" + (order + 1)] = Label_NowCoin.Text;
                    //Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "ActionScore";
                    Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "ScoreUpdated";
                    if (await StatusGate("ScoreUpdated", "ScoreUpdated"))
                    {

                    }
                    ArrayList newScore = await Listening_Application(game_name + "_" + room_number + "_score_" + "player");
                    for (int i = 0; i <= newScore.Count - 1; i++)
                    {
                        ((Label)player_score[i]).Text = Convert.ToString(newScore[i]);
                    }

                    Timer_RoomUser.Enabled = true;
                    Timer_RoomUser.Interval = 500;
                    Application[game_name + "_" + room_number + "_status"] = "ReSet";                 
                }

                Button_Record.Enabled = true;
            }
        }
    

        #endregion

        #region "battle room"


        //本來就有無限房間，只秀出有人的房號
        protected void Load_Room(ListBox room_list, String room_number, String game_name)
        {
            //String test = Convert.ToString(Application["test"]);
            
            if(Application[game_name + "_exit_room_order"] !=null)
            {
                String exit_room_order = Convert.ToString(Application[game_name + "_exit_room_order"]);
                String exit_room = exit_room_order.Substring(0, exit_room_order.IndexOf(":"));
                int exit_order = Convert.ToInt32(exit_room_order.Substring(exit_room_order.IndexOf(":") + 1));
                String status = Convert.ToString(Application[game_name + "_" + exit_room + "_status"]);
                if (status.Equals("ExitRoom"))
                {
                    int max_user = 4;                    
                    ExitRoom(exit_room, game_name, exit_order, max_user, false);
                }
            }
            

            if (Application[game_name + "_room"] != null)
            {
                room_list.Items.Clear();

                Dictionary<String, String> room = (Dictionary<String, String>)Application[game_name + "_room"];
                String tempString = "";
                String room_status = "";
                String room_status_string = "";

                foreach (KeyValuePair<string, string> kvp in room)
                {
                    room_status = Convert.ToString(Application[game_name + "_" + kvp.Key + "_status"]);
                    
                    switch(room_status)
                    {                      
                        case "GameStart":
                        case "RandomDeal_First":
                        case "RandomDeal":
                        case "ReSet":
                            {
                                room_status_string = "(進行中)";
                            }
                            break;
                        default:
                            {
                                room_status_string = "(可進入)";
                            }
                            break;
                    }

                    tempString = kvp.Key + ":" + room_status_string+kvp.Value;
                    room_list.Items.Add(tempString);
                }

                if (Application[game_name + "_" + room_number] != null)
                {
                    //ArrayList room_user = new ArrayList();
                    Dictionary<int, String> room_user = new Dictionary<int, String>();
                    //room_user = (ArrayList)Application[game_name + "_" + room_number];
                    room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                    String user = "";

                    for (int i = 0; i <= room_user.Count - 1; i++)
                    {
                        user = user + room_user[i] + "</br>";
                    }                    
                }
            }
            else
            {
                room_list.Items.Clear();
            }

        }
        protected void EnterRoom(String room_number, String game_name, String user_name, int max_user)
        {
            Timer_RoomUser.Enabled = true;
            Timer_RoomUser.Interval = 500;            

            
            if (room_number != "") //確定欄位有值
            {
                String RoomStatus = Convert.ToString(Application[game_name + "_" + room_number + "_status"]);
                if (RoomStatus.Equals("GameStart") || RoomStatus.Equals("RandomDeal_First") || RoomStatus.Equals("RandomDeal") || RoomStatus.Equals("ReSet"))
                {
                    MessageBox.Show("該房間已開始遊戲，無法進入!");
                }
                else
                {
                    String new_url = game_name + "?id=" + room_number; //將房號以變數形式加在新網址當中
                    Dictionary<String, String> room = new Dictionary<String, String>(); //我們希望房號跟使用者能夠分開紀錄，以便查詢，因此使用帶鍵/值對的Dictionary來存放有人的房號
                                                                                        //ArrayList room_user = new ArrayList(); //以ArayList來存放特定房號中的所有user
                    Dictionary<int, String> room_user = new Dictionary<int, String>();
                    String all_user = ""; //room要加入的對象為(room_number, all_user)，all_user為含有所有user的字串

                    if (Application[game_name + "_room"] != null) //確定有房間有人
                    {
                        room = (Dictionary<String, String>)Application[game_name + "_room"]; //由於已有房間有人，Application[game_name + "_room"]有紀錄，因此先讀出紀錄


                        if (Application[game_name + "_" + room_number] != null) //確定要進入的房間有人
                        {
                            //room_user = (ArrayList)Application[game_name + "_" + room_number]; //由於要進入的房間有人，Application[game_name + "_" + room_number]有紀錄，因此先讀出紀錄
                            room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];
                            Session["order"] = room_user.Count;

                            if (room_user.Count < max_user) //判斷房間是否滿了，最多max_user個人
                            {
                                //room_user.Add(user_name); //新增一名進入房間的使用者
                                room_user.Add(room_user.Count, user_name);
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
                            Session["order"] = 0;
                            room_user.Add(0, user_name);
                            all_user = "(" + room_user.Count + "/" + max_user + ")" + Convert.ToString(room_user[0]);
                            room.Add(room_number, all_user);
                        }

                    }
                    else //所有的房間都沒有人
                    {
                        Session["order"] = 0;
                        room_user.Add(0, user_name);
                        all_user = "(" + room_user.Count + "/" + max_user + ")" + Convert.ToString(room_user[0]);
                        room.Add(room_number, all_user);
                    }

                    Application[game_name + "_" + room_number] = room_user; //更新Application[game_name + "_" + room_number]中的資訊
                    Application[game_name + "_room"] = room; //更新Application[game_name+"_room"]中的資訊     

                    String last_room_number = Convert.ToString(Request.QueryString["id"]); //以get方式取得網址上"?id="後面的房號
                    if (last_room_number != null) //若有房號，則在跳轉之前需先執行ExitRoom
                    {
                        int order = Convert.ToInt32(Session["order"]);
                        ExitRoom(last_room_number, game_name, order, max_user);
                    }

                    //Session["IsInRoom"] = true;
                    Application[game_name + "_" + room_number + "_count"] = Convert.ToInt32(Application[game_name + "_" + room_number + "_count"]) + 1;
                    Response.Redirect(new_url); //跳轉至有帶有房號的網址
                }
                
            }
            else
            {
                MessageBox.Show("請輸入房號!");
            }
        }


        protected void ExitRoom(String room_number, String game_name, int order, int max_user, Boolean IsButtonClick = true)
        {
            Application[game_name + "_" + room_number + "_status"] = "ExitingRoom";
            Dictionary<String, String> room = (Dictionary<String, String>)Application[game_name + "_room"];//讀出哪些房間有人的紀錄
            //ArrayList room_user = (ArrayList)Application[game_name + "_" + room_number];//讀出該房號有哪些人的紀錄
            Dictionary<int, String> room_user = (Dictionary<int, String>)Application[game_name + "_" + room_number];

            if (room_user.Count == 1) //確認該房號的使用者是否只有一人
            {
                room_user.Clear(); //直接將該房號有哪些人的紀錄清空
                Application.Remove(game_name + "_" + room_number);
                Application.Remove(game_name + "_" + room_number + "_status");
                Application.Remove(game_name + "_" + room_number + "_count");                
                Application.Remove(game_name + "_" + room_number + "_pork_" + "deal1");
                Application.Remove(game_name + "_" + room_number + "_pork_" + "deal2");
                Application.Remove("pork" + room_number);
                Application.Remove("pork2" + room_number);

                if (room.Count == 1) //確認只有一間房間有人
                {
                    room.Clear(); //直接將哪些房間有人的紀錄清空
                    Application.Remove(game_name + "_room");                    
                }
                else //不只一間房間有人
                {
                    room.Remove(room_number); //僅刪除該房號有人的紀錄
                    Application[game_name + "_room"] = room; //更新Application[game_name+"_room"]中的資訊
                }
            }
            else //該房號的使用者不是只有一人
            {
                //room_user.Remove(user_name); //僅刪除該房號有此人的紀錄
                room_user.Remove(order);
                String all_user = "";
                String temp_user = "";
                /*
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
                */
                for (int i = 0; i <= room_user.Count - 1; i++) //需用for loop將所有使用者串成一個字串，再丟入all_user中
                {
                    if (i >= order)
                    {
                        temp_user = room_user[i + 1];
                        room_user.Remove(i + 1);
                        room_user.Add(i, temp_user);
                    }

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
                Application[game_name + "_" + room_number + "_status_" + "player" + (order + 1)] = "OrderUpdated";
                Application[game_name + "_" + room_number] = room_user; //更新Application[game_name + "_" + room_number]中的資訊
                Application[game_name + "_room"] = room; //更新Application[game_name+"_room"]中的資訊 

                Application[game_name + "_" + room_number + "_count"] = Convert.ToInt32(Application[game_name + "_" + room_number + "_count"]) - 1;
                Application[game_name + "_" + room_number + "_status"] = "ExitedRoom";
            }            

            if (IsButtonClick)
            {
                Response.Redirect(game_name);
            }
        }

        protected async Task updateOrder()
        {
            if(Application[game_name + "_exit_room_order"] != null)
            {
                String exit_room_order = Convert.ToString(Application[game_name + "_exit_room_order"]);
                String exit_room = exit_room_order.Substring(0, exit_room_order.IndexOf(":"));
                int exit_order = Convert.ToInt32(exit_room_order.Substring(exit_room_order.IndexOf(":") + 1));
                String status = Convert.ToString(Application[game_name + "_" + exit_room + "_status"]);
                if (status.Equals("ExitedRoom"))
                {
                    int order = Convert.ToInt32(Session["order"]);
                    //int exit_order = Convert.ToInt32(Application[game_name + "_exit_room_order"]);
                    if (order > exit_order)
                    {
                        Session["order"] = order - 1;
                    }

                    Application[game_name + "_" + exit_room + "_status_" + "player" + (order + 1)] = "OrderUpdated";
                    if (await StatusGate("OrderUpdated", "ApplicationDelete"))
                    {
                        Application.Remove(game_name + "_exit_room_order");
                    }
                    Application[game_name + "_" + exit_room + "_status_" + "player" + (order + 1)] = "ApplicationDelete";
                }
            }
        }
       

        #endregion









    }

}