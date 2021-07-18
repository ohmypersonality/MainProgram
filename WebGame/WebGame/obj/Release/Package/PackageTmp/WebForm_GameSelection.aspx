<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="WebForm_GameSelection.aspx.cs" Inherits="WebGame.WebForm_GameSelection"%>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>Modern Business - Start Bootstrap Template</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />
    </head>
    <body class="d-flex flex-column" >
    <form id="form1" runat="server" method="post"> 
        <main class="flex-shrink-0">

            <!-- Pricing section-->
            <section class="bg-light py-5">
                <div class="container px-5 my-5">
                    <div class="text-center mb-5">
                        <h1 class="fw-bolder">Play now</h1>
                        <p class="lead fw-normal text-muted mb-0">Have a good time!</p>
                    </div>
                    <div class="row gx-5 justify-content-center">
                        <!-- Pricing card free-->
                        <div class="col-lg-6 col-xl-4">

                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/pic/icon/InBetween.png" OnClick="Button_Click" Height="360px" Width="410px" />
                                  
                        </div>
                        <!-- Pricing card pro-->
                        <div class="col-lg-6 col-xl-4">
                            <div class="card mb-5 mb-xl-0">

                                   <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/pic/icon/JumpGame.jpg" OnClick="Button_Click" Height="360px" Width="410px" />

                            </div>
                        </div>
                        <!-- Pricing card enterprise-->
                        <!--
                        <div class="col-lg-6 col-xl-4">
                            <div class="card">
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/pic/icon/mora.jpg" OnClick="Button_Click" Height="360px" Width="410px" />
                            </div>
                        </div>
                            -->
                    </div>
                </div>
            </section>
        </main>

            </form> 
    </body>
</html>
