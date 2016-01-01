using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GamePool2Core.Entities;
using Microsoft.AspNet.Identity;
using WebGrease.Css;

namespace GamePoolWeb2
{
    public partial class AdminUsers : System.Web.UI.Page
    {
        private List<User> m_Users;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["m_Users"] != null)
            {
                m_Users = (List<User>)ViewState["m_Users"];
            }
            else
            {

                Repository m_Repository = new Repository();
                //User user = await m_Repository.GetUser(Session["UserName"].ToString());

                IList<User> users = await m_Repository.GetPoolUsers(string.Empty);

                m_Users = new List<User>();
                foreach (User user in users)
                {
                    m_Users.Add(user);
                }

            }

        }


        protected override void OnPreRender(EventArgs e)
        {
            GridView1.DataSource = m_Users;
            GridView1.DataBind();

            ViewState.Add("m_Users", m_Users);

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
            User editUser = m_Users[GridView1.EditIndex];

            editUser.UserName = ((TextBox) (row.Cells[2].Controls[0])).Text;
            editUser.IsLocked = Convert.ToBoolean(((TextBox)(row.Cells[3].Controls[0])).Text);
            editUser.IsEligible = Convert.ToBoolean(((TextBox)(row.Cells[4].Controls[0])).Text);
            editUser.PoolScore = Convert.ToInt32(((TextBox)(row.Cells[5].Controls[0])).Text);

            // update the selected team in the repository
            Repository m_Repository = new Repository();

            //if (string.IsNullOrWhiteSpace(editUser.Id))
            //{
            //    editTeam.Id = Guid.NewGuid().ToString();
            //    await m_Repository.InsertTeam(editTeam);
            //}
            //else
            {
                await m_Repository.UpdateUser(editUser);
            }

            GridView1.EditIndex = -1;
        }

        protected void GridView1_RowCancelling(object sender, GridViewCancelEditEventArgs e)
        {
            User editUser = m_Users[GridView1.EditIndex];
            if (string.IsNullOrWhiteSpace(editUser.Id))
                m_Users.Remove(editUser);    
            GridView1.EditIndex = -1;
        }

        protected async void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            User deleteUser = m_Users[e.RowIndex];
            Repository m_Repository = new Repository();

            m_Users.Remove(deleteUser);
            //delete user games - this doesn't work :(
            IList<UserGame> userGames = await m_Repository.GetUserGames(deleteUser.Id);
            foreach (UserGame userGame in userGames)
                await m_Repository.DeleteUserGame(userGame);
                
            await m_Repository.DeleteUser(deleteUser);

            GridView1.EditIndex = -1;
            
        }

        //protected void AddTeam_Click(object sender, EventArgs e)
        //{
        //    //create new team, add to collection.
        //    Team newTeam = new Team();
        //    m_Users.Insert(0, newTeam);//.Add(newTeam);
        //    //GridView1.EditIndex = m_Teams.Count();
        //}
        protected void LockAllUsers_Click(object sender, EventArgs e)
        {
            Repository m_Repository = new Repository();
            foreach (User user in m_Users)
            {
                user.IsLocked = true;
                m_Repository.UpdateUser(user);
            }
        }

        protected void UnlockAllUsers_Click(object sender, EventArgs e)
        {
            Repository m_Repository = new Repository();
            foreach (User user in m_Users)
            {
                user.IsLocked = false;
                m_Repository.UpdateUser(user);
            }
        }

    }
}