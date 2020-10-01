<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="00test.aspx.cs" Inherits="CeramicaGlobo2018._00test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Content/css/fontawesome-all.min.css" rel="stylesheet" />
    <link href="Content/css/jssocials.css" rel="stylesheet" />
    <link href="Content/css/jssocials-theme-flat.css" rel="stylesheet" />
    <script src="Content/Scripts/jquery.js"></script>
    <script src="Content/Scripts/jssocials.js"></script>
</head>
<body>
    <form id="form1" runat="server">


        <i class="fas fa-camera-retro"></i>

        <div class="share"></div>

    </form>
    <script>
        $(".share").jsSocials({
            showLabel: false,
            showCount: false,
            shares: ["facebook", "twitter", "googleplus", "pinterest", "email"]
        });
    </script>
</body>
</html>
