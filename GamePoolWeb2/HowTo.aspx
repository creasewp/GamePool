<%@ Page Title="How To" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HowTo.aspx.cs" Inherits="GamePoolWeb2.HowTo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    
    <p>
        <h3><b>Select a winning team...</b></h3>
        <ul>
            First, browse to <a href="\MyPicks.aspx">My Picks</a>.

            Then click on the name of the team you predict will win that bowl game. <br/>
            <asp:Image runat="server" ImageUrl="\Images\SelectTeam.png" Height="70"  /><br/>
            Your choice will be identified with a thumbs up icon.<br/>
            <asp:Image runat="server" ImageUrl="\Images\TeamSelected.png" Height="70"  />
        </ul>


        <h3><b>Change your Ranking Score...</b></h3>
        <ul id="list">
            <li>
                The default mode is 'Worksheet Mode'. In this mode, you must enter your confidence score for each game in the appropriate textbox.</br>
                <asp:Image runat="server" ImageUrl="\Images\WorksheetConfidence.png" Height="70"  /></br>
                After entering your confidence score for all the games, click <a href='#saveChanges'>here</a> to learn how to save your changes.
            </li>
            </br>
            <strong>-- OR --</strong>
            </br></br>
            Unchecking the 'Worksheet Mode' checkbox enables movement of games as described below.</br>

            There are two ways to change your Ranking Score for a bowl game.<br/>
            
            <li>
                Click on the green Up or Down arrow to move a game up or down one at a time
                <br/>
                For example, if the current bowl game has a Ranking Score of 38,<br/>
                <asp:Image runat="server" ImageUrl="\Images\RankScore38.png" Height="70" /> <br/>
                clicking the down arrow will change the ranking score to 37.<br/>
                <asp:Image runat="server" ImageUrl="\Images\RankScore37.png" Height="70" /> <br/>
                <asp:Image runat="server" ImageUrl="\Images\blank square.png" Height="30" />                
            </li>
            <li>
                If you want to move a game more than one at a time, click the Edit link<br/>
                <asp:Image runat="server" ImageUrl="\Images\EditRankScore.png" Height="70" /> <br/>
                This will provide a textbox where you can manually type in a new rank score. Click Update to update your list.<br/>
                <asp:Image runat="server" ImageUrl="\Images\EditTextboxRankScore.png" Height="70" /> <br/>
                <asp:Image runat="server" ImageUrl="\Images\blank square.png" Height="30" />                
            </li>
            <li>
                Once updated, your list should look something like this. </br>
                <asp:Image runat="server" ImageUrl="\Images\MovedTo15.png" Height="210" /> <br/>
                Note that each game must have a unique Ranking Score. When you move a game using the Edit link,
                the website will re-calculate the other games to make room for your change.
                <br/>
                <asp:Image runat="server" ImageUrl="\Images\blank square.png" Height="30" />                
            </li>                
        </ul>
        <div id="saveChanges">
            </br>
            </br>
        </div>
        <h3 ><b>Save your changes</b></h3>
        <ul>
            When ready, be sure to save your changes. Click the Save Changes button at the top of the <a href="\MyPicks.aspx">My Picks</a> page.
            <br/>
            <asp:Image runat="server" ImageUrl="\Images\SaveChanges.png" Height="180" /> <br/>
        </ul>

        <h3><b>Track your progress</b></h3>
        <ul>
            Once the games have started, you can see how you did on individual game by going back to the 
            <a href="\MyPicks.aspx">My Picks</a> page. <br/>
            Here, you can see when each game is scheduled to be played.<br/>
            When the game is over, you can see the final score and how many points you were awarded.         
            <br/>
            Below you can see that we picked the correct winner for the Gildan New Mexico Bowl and were awarded 2 points.<br/>
            We did not pick the correct winner of the Royal Purple Las Vegas Bowl and were awarded 0 points.<br/>

            <asp:Image runat="server" ImageUrl="\Images\BowlResults.png" /> <br/>
            <br/>
            You can also track your overall progress on the <a href="/Leaderboard.aspx">Leaderboard</a>.
        </ul>

    </p>
</asp:Content>
