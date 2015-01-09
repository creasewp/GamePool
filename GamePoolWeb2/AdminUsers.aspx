﻿<%@ Page Title="Admin Users" Language="C#" MasterPageFile="~/Site.Master" Async="True" EnableViewState="True"
    AutoEventWireup="true" CodeBehind="AdminUsers.aspx.cs" Inherits="GamePoolWeb2.AdminUsers" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>

    </h3>
    <asp:Button runat="server" Text="Lock All Users" OnClick="LockAllUsers_Click"/>
    <asp:Button runat="server" Text="Unlock All Users" OnClick="UnlockAllUsers_Click"/>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" OnRowEditing="GridView1_RowEditing" 
        OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelling" OnRowDeleting="GridView1_RowDeleting">
        <AlternatingRowStyle BackColor="LightBlue"></AlternatingRowStyle>
        <Columns>            
            <asp:BoundField ReadOnly="true" DataField="Id" HeaderText="Id"/>
            <asp:BoundField DataField="UserName" HeaderText="User Name"/>
            <asp:BoundField DataField="IsLocked" HeaderText="Locked"/>
            <asp:BoundField DataField="IsEligible" HeaderText="Eligible"/>
            <asp:BoundField DataField="PoolScore" HeaderText="Score"/>
        </Columns>
    </asp:GridView>
                
</asp:Content>
