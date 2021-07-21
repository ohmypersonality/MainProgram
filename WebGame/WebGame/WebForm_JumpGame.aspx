<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="WebForm_JumpGame.aspx.cs" Inherits="WebGame.WebForm_JumpGame" %>
<html>
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <script type="text/javascript" src="https://gc.kis.v2.scr.kaspersky-labs.com/FD126C42-EBFA-4E12-B309-BB3FDD723AC1/main.js?attr=WeXZ5461lZM7MUas-JbjmwsOBsNRX5EocORs5SmJFWdejXHNc-oZUwRtWR2LBEmFUsPnRpbHt2kBJoDTeD5thFiXvqGfkBCprbB2aZeW2Ywi78KRSu7mSFeHzTiZ50y0D40sB4wRYG7tJCgEwmnOwlFYOC4MQJvJ4QcDFZrdVFQr0CG_n06d0V4809ZhMa0K-Sv1jymNj3Qtl7ZjMzrg7Lm0F5UDtnG5hWBf0LGCe7C0AOZvRFpj55bL75Wd2xDkFmWiEvUaPNoJBUaJFuze1xezn-1zqXRUnII-vLdSZNlB3gRkJ9eiZi0JoHZGXhiHGc1338PhnY3VH5IWaSY0It7MP46WyegMSgaWtdCRVLUkQqKuOuQCu7tQSjmynxYs8ksEYn1TsF3hcXdr64TRv6iMZx0bnp60aEu-CkCJmSYZRDMk-Dgu8DiyNsmhqN__zrY9kX8U8Ec-aYg0dWOX1MxmUxocMgbNRgfGtbwz8zjKWxymuGVdoHyqq8TjoTOiC5KXoImQB6fTzNMlFHm1a6IqQhcJLsuL7787cq-RlZk01MH_kx8JGxdzkUqzrZ85nAFJtuId2D8YVgQTzjjhlWTu69JvPTpYIGcwETvsmR143owvJwipocghm9pf7RRNAnTldHkkhHBTSpB2CrbEew0iBSchX0CuFAWt9UM8JrJZ_Q0LrxpVHFPkniHjFc5s6wveCi1xYJlnlXXdUNTmYMfJqPXaXHYTtEf30Nf2FpZ3uk0bi56NTngw5GNrkP1OY0vTHTugo_U-BsONo5w54-aWNbMXeo2RVcEzT-mhem6kGn4kfyyqpAZxjJj4JDeSsiVxaNaYSbXblWm3mRd2DQSstX5R36UMARrg5xqagRZ-BZ2yfX7cghcpIUfv9aehukFrKQ1Ju9mCFwO_83Ov04if_VBzscvuSY4MUoWcWKLDf3Xxo2HB4f4Dc18Q-Hkkz4mLeclenke-0IgrErfd6rPrfpeRa6xysamnzK0zqxvavTHMGuu74HCQdszKD0kbh63J3rbXtU8sUBY1xTuvw1Cht6VLBJgjistA4Ue0wlnyiFcj2GdpC3V_GBAmMO5oNKLrOlu9XlfiN7-xCNkQH02nMaTDZh8C6nho5prmMfWGjyebGLH6ChbHZedwgMpAExlV6v8JNoORAgTW_5JCl4RhR4rZcNLF1oIuSFMN4LvaduOv8JkXGCEUzTYCk1s4D_GtKMKF_afnQT-jFlY08Q" nonce="4fcb83e25b161b98ac442db94c0cb521" charset="UTF-8"></script><link rel="stylesheet" crossorigin="anonymous" href="https://gc.kis.v2.scr.kaspersky-labs.com/E3E8934C-235A-4B0E-825A-35A08381A191/abn/main.css?attr=aHR0cHM6Ly9tYWlsLWF0dGFjaG1lbnQuZ29vZ2xldXNlcmNvbnRlbnQuY29tL2F0dGFjaG1lbnQvdS8wLz91aT0yJmlrPTcxOWNmZjUwZDQmYXR0aWQ9MC4xJnBlcm1tc2dpZD1tc2ctZjoxNzAzNDM4NDYxNjgyMjU5NTI3JnRoPTE3YTNkNDQxNzU0ZjE2NDcmdmlldz1hdHQmZGlzcD1zYWZlJnJlYWxhdHRpZD1mX2txYW9qMjVoMCZzYWRkYmF0PUFOR2pkSl9jYndLc0JQS2dfcTIzWU5FbngyS0luSF9BeERqcVowRl9GX3Z3YklfOTVCUFo5QnhHeTdXNV9kYXdMcEhNZHJBb3Bpc1U0eHpkTGRlNFRHOHIxVElJdWJTS3FxOU5fU3VEMy1nU1gzYUxMN0pFOGQ2QVVlNmZpZWpBd3BJNGFXRGZjNlQ4UURhRVRmbTV3ZGxBOU5NTHFNSzJROFNycmxmOEx4dkJrTEh2Y3VGclVseUpCX1JNWUUyNzF1RTNOZUlUUHdGT2hkUy1GTjVQMnAzald2cWxZaEcteFN0ZW0zRWpjLTVseXJzOXZySjN2VjZtOElEdXNRanA4RWJsaWZNLVhfczVzOHdtemhjbndtUmFJVHktX2JxTE91Wld4dG8xVGtGUkV4MTJUU2dmRExqbUdTOE4yZ19VMkZuYjlkY2c5Y3NCbU9TTkFrUnVTMndCd01MbTZPWmFlNGZtTDVHVnZrYmZOeUV3YU9KaDVoU3czUnlxckdUZGtyOVd2b3dOXzBTU2NRa01HY05TeFluUHNrVzMyLVhBaDk1WUJUVy1WZ05wOHJieW1peTEtMlVKeUo1dTNicFhTRUFEeFVITzRVS3h1cVFJdXJyM1NNM3JlSTBqMFpYS0pSOGNDR0RkOENSa2E0MGhXREJnb1JPM0swVWF6cXk1c29GWHhiWDZ1S0k4WVJBT3lBdmU1ekd5NlUxSzE0UHFDUjhpT19oUE1SdEEwOWZ1TzhHUVhxQXd2MERlMG9YVEp4UlNQWTlwcDJMRmhVMTUtRXl2VjR0V0YwN1RTck5GV3Z1QWprbElKMTJHX1c0TlpMODRiYnM1c0U4VXk4TmtlNEJjOHM3RV9mZS1rd0NvNlQwS25VRVdWV0tHSzVBMjNFekhPOFhaY0ZmWEhiQjR5N2RzME00SVY3dUJ4Wkk"/><style>
        canvas {
            border: 1px solid #d3d3d3;
            background-color: #f1f1f1;
        }
    </style>

</head>
<style>

.Logout{
    position: absolute;
	top: calc(10% - 75px);
	left: calc(80% - 50px);
	height: 150px;
	width: 350px;
	padding: 10px;
	z-index: 1;    
}

.Button_Record{
    position: absolute;
	top: calc(10% - 75px);
	left: calc(10% - 50px);
	height: 150px;
	width: 350px;
	padding: 10px;
	z-index: -1;    
}

.GridView{
    position: absolute;
    top: calc(45% - 75px);
    left: calc(5% - 50px);
    height: 150px;
    width: 1000px;
    padding: 10px;
    z-index: 2;        
}    

</style>
<body onload="startGame()">
    <input id="Radio1" type="radio" />
    <script>

        var myGamePiece;
        var myObstacles = [];
        var myScore;
        var recordTimes;
        

        function startGame() {
            myGamePiece = new component(30, 30, "red", 10, 120);
            myGamePiece.gravity = 0.05;
            myScore = new component("30px", "Consolas", "black", 280, 40, "text");
            myGameArea.start();
        }

        var myGameArea = {
            canvas: document.createElement("canvas"),
            start: function () {
                this.canvas.width = 480;
                this.canvas.height = 270;
                this.context = this.canvas.getContext("2d");
                document.body.insertBefore(this.canvas, document.body.childNodes[0]);
                this.frameNo = 0;
                this.interval = setInterval(updateGameArea, 20);
                recordTimes = 0;
            },
            clear: function () {
                this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
            }
        }

        function component(width, height, color, x, y, type) {
            this.type = type;
            this.score = 0;
            this.width = width;
            this.height = height;
            this.speedX = 0;
            this.speedY = 0;
            this.x = x;
            this.y = y;
            this.gravity = 0;
            this.gravitySpeed = 0;
            this.update = function () {
                ctx = myGameArea.context;
                if (this.type == "text") {
                    ctx.font = this.width + " " + this.height;
                    ctx.fillStyle = color;
                    ctx.fillText(this.text, this.x, this.y);
                } else {
                    ctx.fillStyle = color;
                    ctx.fillRect(this.x, this.y, this.width, this.height);
                }
            }
            this.newPos = function () {
                this.gravitySpeed += this.gravity;
                this.x += this.speedX;
                this.y += this.speedY + this.gravitySpeed;
                this.hitBottom();
            }
            this.hitBottom = function () {
                var rockbottom = myGameArea.canvas.height - this.height;
                if (this.y > rockbottom) {
                    this.y = rockbottom;
                    this.gravitySpeed = 0;
                }
            }
            this.crashWith = function (otherobj) {
                var myleft = this.x;
                var myright = this.x + (this.width);
                var mytop = this.y;
                var mybottom = this.y + (this.height);
                var otherleft = otherobj.x;
                var otherright = otherobj.x + (otherobj.width);
                var othertop = otherobj.y;
                var otherbottom = otherobj.y + (otherobj.height);
                var crash = true;
                if ((mybottom < othertop) || (mytop > otherbottom) || (myright < otherleft) || (myleft > otherright)) {
                    crash = false;
                }
                return crash;
            }
        }

        function updateGameArea() {
            var x, height, gap, minHeight, maxHeight, minGap, maxGap;
            for (i = 0; i < myObstacles.length; i += 1) {
                if (myGamePiece.crashWith(myObstacles[i])) {
                    if (recordTimes==0) {
                        window.document.getElementById('Button_Record').click();
                        recordTimes += 1;
                    }                    
                    return;
                }
            }
            myGameArea.clear();
            myGameArea.frameNo += 1;
            if (myGameArea.frameNo == 1 || everyinterval(150)) {
                x = myGameArea.canvas.width;
                minHeight = 20;
                maxHeight = 200;
                height = Math.floor(Math.random() * (maxHeight - minHeight + 1) + minHeight);
                minGap = 50;
                maxGap = 200;
                gap = Math.floor(Math.random() * (maxGap - minGap + 1) + minGap);
                myObstacles.push(new component(10, height, "green", x, 0));
                myObstacles.push(new component(10, x - height - gap, "green", x, height + gap));
            }
            for (i = 0; i < myObstacles.length; i += 1) {
                myObstacles[i].x += -1;
                myObstacles[i].update();
            }            
            myScore.text = "SCORE: " + myGameArea.frameNo;            
            myScore.update();
            myGamePiece.newPos();
            myGamePiece.update();
            setvalue(myGameArea.frameNo);                  
        }

        function everyinterval(n) {
            if ((myGameArea.frameNo / n) % 1 == 0) { return true; }
            return false;
        }

        function accelerate(n) {
            myGamePiece.gravity = n;
        }

        function setvalue(aValue) {
            var a = document.getElementById("Score");
            a.value = aValue;
        }

    </script>
    <br>
    <!--<button onmousedown="startGame()";" >Start</button> --> 
    <button onmousedown="accelerate(-0.2)" onmouseup="accelerate(0.05)">Jump</button>
    <!--<button onmousedown="window.location.reload();" >ReStart</button> -->     

    <form id="form1" runat="server" method="post">
        <div class="Logout"> 
            <asp:Button ID="Button_Logout" runat="server" OnClick="Button_Logout_Click" Text="Logout" />
        </div>
        <div class="Button_Record"> 
            <asp:Button ID="Button_Record" runat="server" OnClick="Button_Record_Click" Text="Record"/> 
        </div>
        <input type="hidden" id="Score" name="Score" runat="server"/>  
        <div class="GridView"> 
            <asp:GridView ID="GridView1" runat="server" Width="600px" CellSpacing="1" OnRowDataBound="GridView1_RawDataBound">
            </asp:GridView>
        </div>
    </form>
</body>  
</html>

