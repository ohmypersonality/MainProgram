<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="WebForm_InBetween.aspx.cs" Inherits="WebGame.WebForm_InBetween" Async="true"%>
  

<!DOCTYPE html>
<html>

<head>

  <meta charset="UTF-8">

  <title>射龍門</title>

<style> <!--以style標籤來定義區塊排版格式，後續再以class標籤使用-->


body{
	margin: 0;
	padding: 0;
	background: #fff;

	color: #fff;
	font-family: Arial;
	font-size: 12px;
}

.body{
	position: absolute;
	top: -20px;
	left: -20px;
	right: -40px;
	bottom: -40px;
	width: auto;
	height: auto;
	background-image: url("../pic/poker/gambling_table.jpg");
	background-size: cover;
	-webkit-filter: blur(5px);
	z-index: 0;
}

.Label_header{
    position: absolute;
	top: calc(10% - 75px);
	left: calc(10% - 50px);
	height: 150px;
	width: 350px;
	padding: 10px;
	z-index: 2;    
}

.Label_player{
    position: absolute;
	top: calc(10% - 75px);
	left: calc(20% - 50px);
	height: 150px;
	width: 350px;
	padding: 10px;
	z-index: 2;
    display: grid;
    grid-template-columns:repeat(4,1fr);
    grid-gap:3px;
}

.Label_score{
    position: absolute;
	top: calc(12% - 75px);
	left: calc(20% - 50px);
	height: 150px;
	width: 350px;
	padding: 10px;
	z-index: 2;
    display: grid;
    grid-template-columns:repeat(4,1fr);
    grid-gap:3px;
}

.Image_poker{	
    position: absolute;
	top: calc(15% - 75px);
	left: calc(10% - 50px);
	height: 150px;
	width: 6000px;
	padding: 10px;
	z-index: 2;   
}

.GridView{
    position: absolute;
    top: calc(45% - 75px);
    left: calc(40% - 50px);
    height: 150px;
    width: 1000px;
    padding: 10px;
    z-index: 2;
}    


.updateButtonAndStatus{
	position: absolute;
	top: calc(45% - 75px);
	left: calc(10% - 50px);
	height: 150px;
	width: 6000px;
	padding: 10px;
	z-index: 2;   
}


.OtherObject{
	position: absolute;
	top: calc(62.5% - 75px);
	left: calc(10% - 50px);
	height: 70px;
	width: 6000px;
	padding: 10px;
	z-index: 2;   
}

.BattleRoom{
	position: absolute;
	top: calc(70% - 75px);
	left: calc(10% - 50px);
	height: 150px;
	width: 6000px;
	padding: 10px;
	z-index: 2;   
}


</style>

  
<body>   
    <div class="body"></div>
    <script>
        //window.onload = function () {
        //    window.open("WebForm_Logout.aspx");
        //    return "Are you sure to close?";
        //}

        //window.onbeforeunload = function () {
        //    return "";    
        //}
</script>
    <!--在html中使用伺服器控制項，一定要用form標籤，而且只能用一次-->
    <form id="form1" runat="server" method="post"> 
        <!--在html中使用UpdatePanel，一定要前面加這一行，而且只能用一次-->
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>  
        <!--以class標籤來使用區塊排版格式，該class在前面的style標籤中定義-->
        <div class="Label_header">  
            <asp:Label ID="Label_Hello" runat="server" Text="" style="color:white;"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>         
                  
        <!--使用UpdatePanel之後，只有內部的控制項會受到Timer影響-->
        <!--在UpdatePanel的Trigger屬性中加入Timer的Tick事件-->
        <asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true"> 
            <ContentTemplate>                
                <div class="Label_player">
                    <asp:Label ID="Label_player1" runat="server" Width="180px" style="color:orange;"></asp:Label>
                    <asp:Label ID="Label_player2" runat="server" Width="180px" style="color:orange;"></asp:Label>
                    <asp:Label ID="Label_player3" runat="server" Width="180px" style="color:orange;"></asp:Label>
                    <asp:Label ID="Label_player4" runat="server" Width="180px" style="color:orange;"></asp:Label>
                </div>

                <div class="Label_score">
                    <asp:Label ID="Label_score1" runat="server" Width="180px" style="color:orange;"></asp:Label>
                    <asp:Label ID="Label_score2" runat="server" Width="180px" style="color:orange;"></asp:Label>
                    <asp:Label ID="Label_score3" runat="server" Width="180px" style="color:orange;"></asp:Label>
                    <asp:Label ID="Label_score4" runat="server" Width="180px" style="color:orange;"></asp:Label>
               </div>
                
               <div class="Image_poker">
                    <asp:Image ID="Image_deal1" runat="server" Height="240px" ImageUrl="~/pic/poker/bicycle_backs.jpg" Width="180px" />
                    <asp:Image ID="Image_player1" runat="server" ImageUrl="~/pic/poker/bicycle_backs.jpg" Height="240px" Width="180px" />
                    <asp:Image ID="Image_player2" runat="server" ImageUrl="~/pic/poker/bicycle_backs.jpg" Height="240px" Width="180px" Visible="False"/>
                    <asp:Image ID="Image_player3" runat="server" ImageUrl="~/pic/poker/bicycle_backs.jpg" Height="240px" Width="180px" Visible="False"/>
                    <asp:Image ID="Image_player4" runat="server" ImageUrl="~/pic/poker/bicycle_backs.jpg" Height="240px" Width="180px" Visible="False"/>
                    <asp:Image ID="Image_deal2" runat="server" ImageUrl="~/pic/poker/bicycle_backs.jpg" Height="240px" Width="180px" />
               </div> 
                
              <div class="updateButtonAndStatus">
                    
                    <asp:Button ID="Button_Start" runat="server" OnClick="Button_Start_Click" Text="Start" />
                    <asp:Button ID="Button_Bet" runat="server" OnClick="Button_PlayerAction_Click" Text="Bet" Enabled="false" />    
                    <asp:Button ID="Button_Pass" runat="server" OnClick="Button_PlayerAction_Click" Text="Pass" Enabled="false" /> 
                    <asp:Button ID="Button_Record" runat="server" OnClick="Button_Record_Click" Text="Record and ReSet" Enabled="false" />  
                    <br />
                    <br />
                    <asp:Label ID="Label3" runat="server" Text="NowCoin: " style="color:white;"></asp:Label>
                    <asp:Label ID="Label_NowCoin" runat="server"  Text=50000 style="color:white;"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="Label_Count" runat="server" Text="一輪有25場" style="color:orange;"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="Label_Result" runat="server" Text="請點擊Start開局" style="color:orange;"></asp:Label>   
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    
             </div>

            </ContentTemplate>            
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="Timer_RoomUser" EventName="Tick" />                
            </Triggers>
        </asp:UpdatePanel>

        <div class="GridView">
            <asp:GridView ID="GridView1" runat="server" Width="600px" CellSpacing="1" OnRowDataBound="GridView1_RawDataBound" style="color:white;">
            </asp:GridView>     
        </div>

        <div class="OtherObject">
            
            <asp:Label ID="Label1" runat="server" Text="BetCoin: " style="color:white;"></asp:Label>
            <asp:TextBox ID="TextBox_BetCoin" runat="server" Text=1000 OnTextChanged="TextBox_BetCoin_TextChanged"></asp:TextBox>
            <br />
            <br />            
            <asp:Label ID="Label4" runat="server" Text="Battle Room: " style="color:white;"></asp:Label>
            <asp:TextBox ID="TextBox_Room" runat="server" Text=""></asp:TextBox>            
        </div> 

        <div class="BattleRoom">
            <asp:Button ID="Button_EnterRoom" runat="server" OnClick="Button_EnterRoom_Click" Text="Enter Room" /> 
            <asp:Button ID="Button_ExitRoom" runat="server" OnClick="Button_ExitRoom_Click" Text="Exit Room" Enabled="false" />
            <br />            
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:ListBox ID="ListBox_BattleRoom" runat="server" OnSelectedIndexChanged="ListBox_BattleRoom_SelectedIndexChanged" Width="316px" AutoPostBack="True"></asp:ListBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer_Status" EventName="Tick" />
                    <asp:AsyncPostBackTrigger ControlID="Timer_RoomUser" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:Label ID="Label5" runat="server" Text="" style="color:white;"></asp:Label>
        </div>
    
        <asp:Timer ID="Timer_Status" runat="server" OnTick="Timer_Status_Tick">
        </asp:Timer>

        <asp:Timer ID="Timer_RoomUser" runat="server" OnTick="Timer_RoomUser_Tick">
        </asp:Timer>        
        
    </form> 

    <p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </p>
 

<body>

</html>