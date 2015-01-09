<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rules.aspx.cs" Inherits="GamePoolWeb2.Rules" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Rules</h3>
    <p>The game is a pool. The player with the most points wins. Points are determined by your ranking of each game and if you chose
        the correct winner. Ranking values are based on your confidence level as to the outcome of the game.
    </p>
    <ol>
        <li> Choose which team you expect to win each game. The score of the game is not involved, only whether they win or lose.</li>
        <li> Decide which bowl game result you have the most confidence in. You will move this bowl game (including your choice of
            winning team) to the top of your list. For example, if you are 100% certain that Podunk U. will win the Toilet Bowl, 
            select Podunk U. as the winner and move the Toilet Bowl to the top of your list. If you have no idea who will win
            the NoName Bowl, place that game at the bottom of your list.
        </li>
        <li> Continue to rank the bowl games from 38 to 1, 38 being most confident to 1 being least confident. This is your ranking score.
            This is the number of points that will be awarded to you if you choose the winning team for that game.
        </li>
        <li>            
            You can change your selections anytime prior to the pool being closed 9:00 PM PST December 19th. 
            Be sure to save any changes you make.
            <%-- Be sure to choose your winners and rank the bowl games by December 19, 2014. --%>
            After this time, your selections will be
            locked in and you will not be allowed to change them.
        </li>
        <li> After each bowl game is played, if you selected the winning team, the website will award you the number of points
            that corresponds with your ranking score. For example, suppose you selected Podunk U. to win the Toilet Bowl and you 
            moved the Toilet Bowl at the top of your list with a ranking score of 38. If Podunk U. wins the Toilet Bowl, then 
            you will receive 38 points. If the opposing team won, then you would receive zero points for that game.
        </li>
        <li> After all 38 bowl games are played, the player with the most points wins.</li>
        <li> Prizes will be awarded to the players with the most points based on the total number of participants. Last place will also get a prize.</li>
    </ol>
</asp:Content>
