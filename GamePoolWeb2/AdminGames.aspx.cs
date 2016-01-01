using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GamePool2Core.Entities;
using WebGrease.Css;

namespace GamePoolWeb2
{
    public partial class AdminGames : System.Web.UI.Page
    {
        private List<Game> m_Games;
        private List<Team> m_Teams; 

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["m_Games"] != null)
            {
                m_Games = (List<Game>)ViewState["m_Games"];
                m_Teams = (List<Team>)ViewState["m_Teams"];
            }
            else
            {

                Repository m_Repository = new Repository();
                //User user = await m_Repository.GetUser(Session["UserName"].ToString());
                //get session to make sure defaults are created
                await m_Repository.GetSettings();

                IList<Game> games = await m_Repository.GetGames();
                Console.WriteLine("games:" + games.Count);
                m_Games = new List<Game>();
                foreach (Game game in games.OrderBy(item => item.GameDateTime))
                {
                    m_Games.Add(game);
                }

                m_Games.Sort(new GameByDateTimeComparer());

                IList<Team> teams = await m_Repository.GetTeams();
                m_Teams = new List<Team>();
                Console.WriteLine("teams:" + teams.Count);
                foreach (Team team in teams.OrderBy(item => item.Description))
                {
                    m_Teams.Add(team);
                }

            }

        }

        public List<Team> GetTeams
        {
            get { return m_Teams; }
        }

        public string GetTeamDescription(object teamId)
        {
            string result = string.Empty;
            if (teamId != null)
            {
                Team matchTeam = m_Teams.SingleOrDefault(item => item.Id == teamId.ToString());
                if (matchTeam != null) result = matchTeam.Description;
            }
            return result;
        }

        protected override void OnPreRender(EventArgs e)
        {
            GridView1.DataSource = m_Games;
            GridView1.DataBind();            

            ViewState.Add("m_Games", m_Games);
            ViewState.Add("m_Teams", m_Teams);

            base.OnPreRender(e);
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataBind();
        }

        protected async void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            Game editGame = m_Games[GridView1.EditIndex];

            editGame.Description = ((TextBox)(row.Cells[2].Controls[0])).Text;

            editGame.HomeTeamId = ((DropDownList) (row.Cells[3].Controls[1])).SelectedValue;
            editGame.AwayTeamId = ((DropDownList)(row.Cells[4].Controls[1])).SelectedValue;

            editGame.Network = ((TextBox)(row.Cells[5].Controls[0])).Text;
            editGame.GameDateTime = ((TextBox)(row.Cells[6].Controls[0])).Text;
            editGame.HomeScore = Convert.ToByte(((TextBox)(row.Cells[7].Controls[0])).Text);
            editGame.AwayScore = Convert.ToByte(((TextBox)(row.Cells[8].Controls[0])).Text);
            editGame.IsGameFinished = Convert.ToBoolean(((TextBox)(row.Cells[9].Controls[0])).Text);

            // update the selected team in the repository
            Repository m_Repository = new Repository();

            if (string.IsNullOrWhiteSpace(editGame.Id))
            {
                editGame.Id = Guid.NewGuid().ToString();
                await m_Repository.InsertGame(editGame);
            }
            else
            {
                await m_Repository.UpdateGame(editGame);
                
            }

            GridView1.EditIndex = -1;

            //calc user scores
            CalculateScores_Click(sender, null);
        }

        protected void GridView1_RowCancelling(object sender, GridViewCancelEditEventArgs e)
        {
            Game editGame = m_Games[GridView1.EditIndex];
            if (string.IsNullOrWhiteSpace(editGame.Id))
                m_Games.Remove(editGame);
            GridView1.EditIndex = -1;
        }

        protected async void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Game deleteGame = m_Games[e.RowIndex];
            Repository m_Repository = new Repository();

            m_Games.Remove(deleteGame);
            await m_Repository.DeleteGame(deleteGame);

            GridView1.EditIndex = -1;

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //find the dropdownlist
            if (GridView1.EditIndex > -1 && (GridView1.EditIndex == e.Row.RowIndex))
            {
                Game deleteGame = m_Games[e.Row.RowIndex];
                ((DropDownList) (e.Row.Cells[3].Controls[1])).SelectedValue = deleteGame.HomeTeamId;
                ((DropDownList) (e.Row.Cells[4].Controls[1])).SelectedValue = deleteGame.AwayTeamId;
            }
        }

        protected void AddGame_Click(object sender, EventArgs e)
        {
            //create new team, add to collection.
            Game newTeam = new Game();
            m_Games.Insert(0, newTeam);
            //GridView1.EditIndex = m_Games.Count();
        }

        protected async void CalculateScores_Click(object sender, EventArgs e)
        {
            //get all users, get all user games, get all games
            Repository m_Repository = new Repository();
            IList<Game> games = await m_Repository.GetGames();
            foreach (Game game in games)
            {
                game.HomeSelectedCount = 0;
                game.AwaySelectedCount = 0;
            }

            IList<User> poolUsers = await m_Repository.GetPoolUsers(string.Empty);//TODO need all users if more than one pool
            foreach (User user in poolUsers)
            {
                int score = 0;
                int lostPoints = 0;
                int possiblePoints = 0;
                double winPercent = 0;
                int gamesCorrect = 0;
                int gamesIncorrect = 0;
                //get user games
                IList<UserGame> userGames = await m_Repository.GetUserGames(user.Id);
                foreach (UserGame userGame in userGames)
                {
                    //find the matching game
                    Game match = games.Single(item => item.Id == userGame.GameId);

                    if (userGame.WinnerTeamId == match.HomeTeamId)
                        match.HomeSelectedCount++;
                    else if (userGame.WinnerTeamId == match.AwayTeamId)
                        match.AwaySelectedCount++;

                    if (match.IsGameFinished)
                    {
                        string winningTeamId = (match.HomeScore > match.AwayScore) ? match.HomeTeamId : match.AwayTeamId;
                        if (userGame.WinnerTeamId == winningTeamId)
                        {
                            score += userGame.Confidence;
                            gamesCorrect++;
                        }
                        else
                        {
                            lostPoints += userGame.Confidence;
                            gamesIncorrect++;
                        }
                    }
                    else
                    {
                        possiblePoints += userGame.Confidence;
                    }
                }
                user.PoolScore = score;
                user.LostPoints = lostPoints;
                user.PossiblePoints = possiblePoints;
                user.WinPercent = 100 * Math.Round((double)((double)gamesCorrect / (double)(gamesCorrect + gamesIncorrect)), 3);
                m_Repository.UpdateUser(user);
            }
            //update games
            foreach (Game game in games)
            {
                m_Repository.UpdateGame(game);
            }
        }

        protected async void LockGames_Click(object sender, EventArgs e)
        {
            Repository m_Repository = new Repository();
            Setting setting = await m_Repository.GetSetting("IsLocked");
            setting.Value = "True";
            await m_Repository.UpdateSetting(setting);
        }

        protected async void UnlockGames_Click(object sender, EventArgs e)
        {
            Repository m_Repository = new Repository();
            Setting setting = await m_Repository.GetSetting("IsLocked");
            setting.Value = "False";
            await m_Repository.UpdateSetting(setting);
        }
    }
}