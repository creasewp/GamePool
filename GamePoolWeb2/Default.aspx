<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GamePoolWeb2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Bowl Game Fun</h1>
        <p class="lead">Welcome!</p>
        
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                <li>Read the <a href="/Rules.aspx">Rules</a></li>
                <li>Learn <a href="/HowTo.aspx">How To</a>...</li>
                <li><a href="/Account/Register.aspx">Register</a> to create an account</li>
                <li><a href="/MyPicks.aspx">Pick</a> your winning teams, and rank your predictions</li>
                <li>Cheer on your favorite teams</li>
                <li>Have fun!</li>
            </p>
        </div>
    </div>

</asp:Content>
