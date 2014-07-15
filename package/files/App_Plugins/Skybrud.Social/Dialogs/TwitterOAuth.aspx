<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TwitterOAuth.aspx.cs" Inherits="Skybrud.Social.Umbraco.App_Plugins.Skybrud.Social.Dialogs.TwitterOAuth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Twitter OAuth</title>
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,700" rel="stylesheet" type="text/css">
    <style>
        body {
            margin: 0;
            font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif;
            font-size: 12px;
        }
        .umb-panel-header {
            height: 99px;
            background: #f8f8f8;
            border-bottom: 1px solid #d9d9d9;
            padding: 0 20px;
            line-height: 99px;
        }
        h1 {
            margin: 0;
            line-height: 99px;
            font-size: 18px;
            font-weight: normal;
        }
        .content {
            padding: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="umb-panel-header">
            <h1>Twitter OAuth</h1>
        </div>
        <div class="content">
            <asp:Literal runat="server" ID="Content" />
        </div>
    </div>
    </form>
</body>
</html>
