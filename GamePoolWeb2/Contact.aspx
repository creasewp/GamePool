<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="GamePoolWeb2.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>For more information:</h3>
    <address>
        John Schroeder<br />
        12002 N.  Riverwood Dr.<br />
        Spokane, WA 99218<br />
        <abbr title="Phone">P:</abbr>
        808.927.0480
    </address>

    <address>
        <strong>General Questions:</strong> <a href="mailto:johnschroeder.schroeder@gmail.com">johnschroeder.schroeder@gmail.com</a>
        <strong>Website Support:</strong>   <a href="mailto:w.creasey@comcast.net">w.creasey@comcast.net</a><br />
    </address>
</asp:Content>
