<%@ Page Title="Leaderboard" Language="C#" MasterPageFile="~/Site.Master" Async="True" EnableViewState="True"
    AutoEventWireup="true" CodeBehind="Leaderboard.aspx.cs" Inherits="GamePoolWeb2.Leaderboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>

    </h3>
    <asp:Button runat="server" Text="Refresh" OnClick="RefreshButton_Click"/>
    <br/>
    <asp:Image runat="server" Width="16" ImageUrl="\Images\16x16-present-icon.png" Visible='<%# Convert.ToBoolean(Eval("IsEligible")) %>' /> = eligible for prizes
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">        
        <AlternatingRowStyle BackColor="LightBlue"></AlternatingRowStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="User">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("UserName") %>' ></asp:Label>
                    <asp:Image runat="server" Width="16" ImageUrl="\Images\16x16-present-icon.png" Visible='<%# Convert.ToBoolean(Eval("IsEligible")) %>' />
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:BoundField DataField="PoolScore" HeaderText="Score" ReadOnly="true" >
                <HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="60" Font-Bold="true"></ItemStyle>
            </asp:BoundField>    
            <asp:BoundField DataField="LostPoints" HeaderText="Lost" ReadOnly="true" >
                <HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="60"></ItemStyle>
            </asp:BoundField>    
            <asp:BoundField DataField="PossiblePoints" HeaderText="Possible" ReadOnly="true" >
                <HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="60"></ItemStyle>
            </asp:BoundField>    
            <asp:BoundField DataField="WinPercent" HeaderText="Win %" ReadOnly="true" >
                <HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="60"></ItemStyle>
            </asp:BoundField>    
        </Columns>
    </asp:GridView>
                
</asp:Content>
