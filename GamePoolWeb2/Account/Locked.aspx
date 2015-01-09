<%@ Page Title="Locked" Language="C#" Async="True" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Locked.aspx.cs" Inherits="GamePoolWeb2.Account.Locked" EnableEventValidation="false" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Sorry</h4>
        <hr />
        <a class="text-danger">The game has been locked. No new users can join at this time.</a>
    </div>
</asp:Content>
