<%@ Page Title="My Picks" Language="C#" MasterPageFile="~/Site.Master" Async="True" EnableViewState="True"
    AutoEventWireup="true" CodeBehind="MyPicks.aspx.cs" Inherits="GamePoolWeb2.MyPicks" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>
        <asp:Label runat="server" ID="GamesLeftToPick"></asp:Label> 
        <br />
        <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red" Visible ="False">Unable to save. There is a duplicate confidence score</asp:Label> 
    </h3>
    <asp:Button runat="server" Text="Refresh" OnClick="RefreshButton_Click"/>
    <asp:Button runat="server" Text="Save Changes" OnClick="SaveButton_Click"/><%--<asp:ImageButton runat="server" ImageUrl="Images\disk_blue.png" OnClick="Save_Click" />--%>
    <br />
    <asp:CheckBox runat="server" AutoPostBack="true" Text="Worksheet Mode" ID="AutoSort" Checked="true" OnCheckedChanged="AutoSort_CheckedChanged" />
    <asp:DataList ID="DataList1" runat="server" OnItemCommand="DataList_Command" >
        <AlternatingItemStyle BackColor="LightBlue"></AlternatingItemStyle>
        <EditItemTemplate>
            <table width="100%">
                <asp:PlaceHolder runat="server" Visible='<%# IsNotLocked(Container.DataItem) %>'>
                    <tr>
                        <td><b><asp:Label ID="ProductIDLabel" runat="server"  Text='<%# Eval("Game.Description") %>' /></b>
                            (<asp:Label ID="Label2" runat="server" Text='<%# Eval("Game.GameDateTime") %>' />) 
                        </td>
                        <td style="text-align: right">                        
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Game.Network") %>' />
                        </td>
                    </tr>               
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" Text='<%# Eval("Game.HomeTeam.Description") %>' CommandName="SelectTeam" CommandArgument='<%# Eval("Id") + "," + Eval("Game.HomeTeam.Id") %>'></asp:LinkButton>                        
                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsHomeSelected(Container.DataItem) %>' />
                        </td>
                        <td rowspan="2" style="text-align: right">
                            <asp:TextBox runat="server" Width="45" ID="UserGameConfidence" Text='<%# Eval("Confidence") %>'></asp:TextBox>
                            <asp:LinkButton runat="server" Text="Update" CommandName="UpdateConfidence" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton runat="server" Text="Cancel" CommandName="CancelConfidence" ></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" Text='<%# Eval("Game.AwayTeam.Description") %>' CommandName="SelectTeam" CommandArgument='<%# Eval("Id") + "," + Eval("Game.AwayTeam.Id") %>'></asp:LinkButton>                        
                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsAwaySelected(Container.DataItem) %>' />
                        </td>
                    </tr>
                </asp:PlaceHolder>
<%--                <asp:PlaceHolder runat="server" Visible='<%# IsLocked(Container.DataItem) %>'>
                    <tr>
                        <td><b><asp:Label ID="Label3" runat="server"  Text='<%# Eval("Game.Description") %>' /></b>
                            <asp:PlaceHolder runat="server" Visible='<%# !Eval("Game.IsGameFinished") %>'>
                                (<asp:Label ID="Label4" runat="server" Text='<%# Eval("Game.GameDateTime") %>' />) 
                            </asp:PlaceHolder>
                        </td>
                        <td style="text-align: right">                        
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("Game.Network") %>' />
                        </td>
                    </tr>               
                    <tr >
                        <td>
                            <asp:Label runat="server" Text='<%# Eval("Game.HomeTeam.Description") %>' ></asp:Label>                                                    
                            <asp:Label Font-Bold="True" ID="HomeScore" runat="server" Visible='<%# Eval("Game.IsGameFinished") %>' Text='<%# Eval("Game.HomeScore") %>' />
                            

                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsHomeSelected(Container.DataItem) %>' />
                        </td>
                        <td rowspan="2" style="text-align: right">
                            (<asp:Label runat="server" Text='<%# Eval("Confidence") %>'></asp:Label>) <br />
                            <b>Your Score: <asp:Label runat="server" Text='<%# GetUserGameScore(Container.DataItem) %>' ForeColor='<%# GetUserGameScoreColor(Container.DataItem) %>'></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text='<%# Eval("Game.AwayTeam.Description") %>' ></asp:Label>                        
                            <asp:Label Font-Bold="True" ID="Label6" runat="server" Visible='<%# Eval("Game.IsGameFinished") %>' Text='<%# Eval("Game.AwayScore") %>' />

                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsAwaySelected(Container.DataItem) %>' />
                        </td>
                    </tr>
                </asp:PlaceHolder>--%>
            </table>            
        </EditItemTemplate>
        <ItemTemplate>
            <table width="100%" border="0">
                <tr>
                    <td colspan="2"><b><asp:Label ID="ProductIDLabel" runat="server"  Text='<%# Eval("Game.Description") %>' /></b>
                        (<asp:Label ID="Label2" runat="server" Text='<%# Eval("Game.GameDateTime") %>' />) 
                    </td>
                    <td style="text-align: right">                        
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Game.Network") %>' />
                    </td>
                </tr>               
                <asp:PlaceHolder runat="server" Visible='<%# IsNotLocked(Container.DataItem) %>'>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" Text='<%# Eval("Game.HomeTeam.Description") %>' CommandName="SelectTeam" CommandArgument='<%# Eval("Id") + "," + Eval("Game.HomeTeam.Id") %>'></asp:LinkButton>                        
                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsHomeSelected(Container.DataItem) %>' />
                        </td>
                        <td>
                            
                        </td>
                        <td rowspan="2" style="text-align: right">
                            (<asp:Label runat="server" Text='<%# Eval("Confidence") %>'></asp:Label>)
                            <asp:LinkButton runat="server" Text="Edit" CommandName="EditConfidence" ></asp:LinkButton>
                            <asp:ImageButton runat="server" ImageUrl="Images\arrow_up_green.png" CommandName="ConfidenceUp" CommandArgument='<%# Eval("Id") %>'  
                                Visible='<%# UpVisible(Container.DataItem) %>'/>
                            <asp:ImageButton runat="server" ImageUrl="Images\arrow_down_green.png" CommandName="ConfidenceDown" CommandArgument='<%# Eval("Id") %>' 
                                Visible='<%# DownVisible(Container.DataItem) %>'  />
                        </td>
                    </tr>
                    <tr style="visibility: <%# IsLocked(Container.DataItem) %>">
                        <td>
                            <asp:LinkButton runat="server" Text='<%# Eval("Game.AwayTeam.Description") %>' CommandName="SelectTeam" CommandArgument='<%# Eval("Id") + "," + Eval("Game.AwayTeam.Id") %>'></asp:LinkButton>                        
                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsAwaySelected(Container.DataItem) %>' />
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" Visible='<%# IsLocked(Container.DataItem) %>'>
                    <tr >
                        <td>
                            <asp:Label runat="server" Text='<%# Eval("Game.HomeTeam.Description") %>' ForeColor='<%# GetHomeScoreColor(Container.DataItem) %>'></asp:Label>                        
                        </td>
                        <td width="10%">
                            <asp:Label Font-Bold="True" ID="Label6" runat="server" Visible='<%# Eval("Game.IsGameFinished") %>' Text='<%# Eval("Game.HomeScore") %>' ForeColor='<%# GetHomeScoreColor(Container.DataItem) %>'/>
                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsHomeSelected(Container.DataItem) %>' />
                        </td>
                            
                        <td width="20%" rowspan="2" style="text-align: right">
                            My Rank: <asp:Label runat="server" Text='<%# Eval("Confidence") %>'></asp:Label> <br />
                            <b>My Points: <asp:Label runat="server" Text='<%# GetUserGameScore(Container.DataItem) %>' ForeColor='<%# GetUserGameScoreColor(Container.DataItem) %>'></asp:Label></b>
                        </td>
                    </tr>
                    <tr style="visibility: <%# IsNotLocked(Container.DataItem) %>">
                        <td>
                            <asp:Label runat="server" Text='<%# Eval("Game.AwayTeam.Description") %>' ForeColor='<%# GetAwayScoreColor(Container.DataItem) %>'></asp:Label>                        
                        </td>
                        <td>
                            <asp:Label Font-Bold="True" ID="Label7" runat="server" Visible='<%# Eval("Game.IsGameFinished") %>' Text='<%# Eval("Game.AwayScore") %>' ForeColor='<%# GetAwayScoreColor(Container.DataItem) %>'/>
                            <asp:Image runat="server" ImageUrl="Images\ic_thumb_up.png" Visible='<%# IsAwaySelected(Container.DataItem) %>' />
                        </td>
                            
                    </tr>
                </asp:PlaceHolder>
            </table>            
        </ItemTemplate>
    </asp:DataList>
                
</asp:Content>
