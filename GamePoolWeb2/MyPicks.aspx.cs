using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using GamePool2Core.Entities;
using Microsoft.AspNet.Identity;

namespace GamePoolWeb2
{


    public partial class MyPicks : Page
    {

        private List<UserGame> m_UserGames;
        private List<Game> m_Games = new List<Game>(); 
        private bool m_IsLocked = false;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["UserName"] == null)
                Response.Redirect("Account/Login.aspx", false);
            else
            {
                //not very efficient, but having trouble saving in viewstate
                Repository m_Repository = new Repository();
                m_Games = (List<Game>)await m_Repository.GetGames();

                string userId = Context.User.Identity.GetUserId();

                if (ViewState["m_UserGames"] != null)
                {
                    m_UserGames = (List<UserGame>) ViewState["m_UserGames"];
                    //m_Games = (List<Game>)ViewState["m_Games"];
                    m_IsLocked = (bool)ViewState["m_IsLocked"];
                }
                else
                {
                    await LoadData();

                    //m_UserGames.Sort();
                }
            }

            //GridView1.DataSource = m_UserGames;
            //GridView1.DataBind();
        }

        private async Task LoadData()
        {
            Repository m_Repository = new Repository();
            User user = await m_Repository.GetUser(Request.Cookies["UserName"].Value);
            //change to system setting
            //m_IsLocked = user.IsLocked;
            Setting setting = await m_Repository.GetSetting("IsLocked");
            bool.TryParse(setting.Value, out m_IsLocked);            

            IList<Game> games = await m_Repository.GetGames();
            IList<UserGame> userGames = await m_Repository.GetUserGames(user.Id);

            m_UserGames = new List<UserGame>();

            foreach (UserGame userGame in userGames)
            {
                userGame.Game = games.Single(item => item.Id == userGame.GameId);
                m_UserGames.Add(userGame);
            }

            //default sort is by confidence
            //if we are locked, sort by date time
            if (m_IsLocked)
            {
                m_UserGames.Sort(new UserGameByDateTimeComparer());
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            DataList1.DataSource = m_UserGames;
            DataList1.DataBind();

            ViewState.Add("m_UserGames", m_UserGames);
            //ViewState.Add("m_Games", m_Games);
            ViewState.Add("m_IsLocked", m_IsLocked);

            if (m_UserGames != null)
            {
                int count = m_UserGames.Count(item => item.WinnerTeamId == string.Empty);
                GamesLeftToPick.Visible = (count == 0) ? false : true;
                GamesLeftToPick.Text = string.Format("You have {0} winners left to pick", count.ToString());
            }
            base.OnPreRender(e);
        }

        protected async void Save_Click(object sender, ImageClickEventArgs e)
        {
            Repository m_Repository = new Repository();
            await m_Repository.UpdateUserGames(m_UserGames);
        }

        protected async void RefreshButton_Click(object sender, EventArgs e)
        {
            await LoadData();
        }

        protected async void SaveButton_Click(object sender, EventArgs e)
        {
            Save_Click(sender, null);
        }

        public bool DownVisible(object obj)
        {
            UserGame userGame = (UserGame) obj;
            return (userGame.Confidence != 1) ? true : false;
        }

        public bool UpVisible(object obj)
        {
            UserGame userGame = (UserGame)obj;
            return (userGame.Confidence != m_UserGames.Count) ? true : false;
        }

        public bool IsHomeSelected(object obj)
        {
            UserGame userGame = (UserGame) obj;
            return userGame.WinnerTeamId == userGame.Game.HomeTeamId;
        }

        public bool IsAwaySelected(object obj)
        {
            UserGame userGame = (UserGame)obj;
            return userGame.WinnerTeamId == userGame.Game.AwayTeamId;
        }

        public bool IsLocked(object obj)
        {
            return (m_IsLocked);

        }

        public bool IsNotLocked(object obj)
        {
            return (!m_IsLocked);

        }

        public Color GetUserGameScoreColor(object obj)
        {
            string score = GetUserGameScore(obj);
            byte userGameScore;
            if (byte.TryParse(score, out userGameScore))
                return (userGameScore > 0) ? Color.Green : Color.Red;
            else
                return Color.Black;
        }

        public Color GetHomeScoreColor(object obj)
        {
            Color result = Color.Black;
            UserGame userGame = (UserGame)obj;
            Game match = m_Games.Single(item => item.Id == userGame.GameId);
            if (match.IsGameFinished)
            {
                result = (match.HomeScore > match.AwayScore) ? Color.Green : Color.Red;
            }
            return result;
        }

        public Color GetAwayScoreColor(object obj)
        {
            Color result = Color.Black;
            UserGame userGame = (UserGame)obj;
            Game match = m_Games.Single(item => item.Id == userGame.GameId);
            if (match.IsGameFinished)
            {
                result = (match.HomeScore < match.AwayScore) ? Color.Green : Color.Red;
            }
            return result;
        }

        public string GetUserGameScore(object obj)
        {
            string result = "TBD";
            if (m_Games.Count > 0)
            {
                UserGame userGame = (UserGame) obj;
                Game match = m_Games.Single(item => item.Id == userGame.GameId);
                if (match.IsGameFinished)
                {
                    //did the user guess right?
                    string winningTeamId = (match.HomeScore > match.AwayScore) ? match.HomeTeamId : match.AwayTeamId;
                    if (userGame.WinnerTeamId == winningTeamId)
                    {
                        result = userGame.Confidence.ToString();
                    }
                    else
                    {
                        result = "0";
                    }
                }
            }
            return result;
        }

        protected void DataList_Command(object sender, DataListCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ConfidenceUp":
                    ChangeConfidence(e.CommandArgument.ToString(), true);
                    break;
                case "ConfidenceDown":
                    ChangeConfidence(e.CommandArgument.ToString(), false);
                    break;
                case "SelectTeam":
                    //command argument is usergameid,teamid
                    string[] split = e.CommandArgument.ToString().Split(',');
                    UserGame userGame = m_UserGames.Single(item => item.Id == split[0]);
                    userGame.WinnerTeamId = split[1];
                    break;
                case "EditConfidence":                    
                    DataList1.EditItemIndex = e.Item.ItemIndex;
                    break;
                case "CancelConfidence":
                    DataList1.EditItemIndex = -1;
                    break;
                case "UpdateConfidence":
                    TextBox newConfidenceTextBox = (TextBox)e.Item.FindControl("UserGameConfidence");
                    byte newConfidence;
                    if (ValidConfidence(newConfidenceTextBox.Text, out newConfidence))
                    {
                        ChangeConfidence(e.CommandArgument.ToString(), newConfidence);
                    }
                    //ChangeConfidence();
                    break;
            }
        }


        private bool ValidConfidence(string text, out byte newValue)
        {
            //can we convert?
            if (byte.TryParse(text, out newValue))
            {
                //is this within the range of valid values 1 through... 39 for now
                //TODO
                if ((newValue >= 1) && (newValue <= 39))
                    return true;
            }
            return false;
        }


        private void ChangeConfidence(string UserGameId, byte newConfidence)
        {
            UserGame matchUserGame = m_UserGames.Single(item => item.Id == UserGameId);
            if (newConfidence > matchUserGame.Confidence)
            {
                //moving up
                for (int i = matchUserGame.Confidence + 1; i <= newConfidence; i++)
                {
                    UserGame match = m_UserGames.Single(item => item.Confidence == i);
                    match.Confidence--;
                }
                matchUserGame.Confidence = newConfidence;
            }
            else if (newConfidence < matchUserGame.Confidence)
            {
                //moving down
                for (int i = matchUserGame.Confidence - 1; i >= newConfidence; i--)
                {
                    UserGame match = m_UserGames.Single(item => item.Confidence == i);
                    match.Confidence++;
                }

                matchUserGame.Confidence = newConfidence;
            }
            //now, sort the list
            m_UserGames.Sort((s1, s2) => s1.Confidence.CompareTo(s2.Confidence));
            m_UserGames.Reverse();
            DataList1.EditItemIndex = -1;
        }

        public void ChangeConfidence(string UserGameId, bool up)
        {
            //find the user game.
            //foreach (GameListItemViewModel itemVM in m_UserGames)
            //TODO - make async???
            //await (() => delegate
            //{

            //find the current confidence
            UserGame matchUserGame = m_UserGames.Single(item => item.Id == UserGameId);
            int currentConfidence = matchUserGame.Confidence;
            //calc the new value;
            if (up)
                currentConfidence++;
            else
                currentConfidence--;
            //find the entry with that matches the new value
            //and change it's value
            UserGame switchUserGame = m_UserGames.Single(item => item.Confidence == currentConfidence);
            if (up)
                switchUserGame.Confidence--;
            else
                switchUserGame.Confidence++;
            matchUserGame.Confidence = currentConfidence;

            //now, sort the list
            m_UserGames.Sort((s1, s2) => s1.Confidence.CompareTo(s2.Confidence));
            m_UserGames.Reverse();
        }

    }
}