using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GamePool2Core.Entities;

namespace GamePoolWeb2
{
    public partial class AdminTeams : System.Web.UI.Page
    {
        private List<Team> m_Teams;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["m_Teams"] != null)
            {
                m_Teams = (List<Team>)ViewState["m_Teams"];
            }
            else
            {

                Repository m_Repository = new Repository();
                //User user = await m_Repository.GetUser(Session["UserName"].ToString());

                IList<Team> teams = await m_Repository.GetTeams();

                m_Teams = new List<Team>();
                foreach (Team team in teams)
                {
                    m_Teams.Add(team);
                }

            }

        }


        protected override void OnPreRender(EventArgs e)
        {
            GridView1.DataSource = m_Teams;
            GridView1.DataBind();

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
            Team editTeam = m_Teams[GridView1.EditIndex];

            editTeam.Description = ((TextBox) (row.Cells[2].Controls[0])).Text;

            // update the selected team in the repository
            Repository m_Repository = new Repository();

            if (string.IsNullOrWhiteSpace(editTeam.Id))
            {
                editTeam.Id = Guid.NewGuid().ToString();
                await m_Repository.InsertTeam(editTeam);
            }
            else
            {
                await m_Repository.UpdateTeam(editTeam);
            }

            GridView1.EditIndex = -1;
        }

        protected void GridView1_RowCancelling(object sender, GridViewCancelEditEventArgs e)
        {
            Team editTeam = m_Teams[GridView1.EditIndex];
            if (string.IsNullOrWhiteSpace(editTeam.Id))
                m_Teams.Remove(editTeam);    
            GridView1.EditIndex = -1;
        }

        protected async void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Team deleteTeam = m_Teams[e.RowIndex];
            Repository m_Repository = new Repository();

            m_Teams.Remove(deleteTeam);
            await m_Repository.DeleteTeam(deleteTeam);

            GridView1.EditIndex = -1;
            
        }

        protected void AddTeam_Click(object sender, EventArgs e)
        {
            //create new team, add to collection.
            Team newTeam = new Team();
            m_Teams.Insert(0, newTeam);//.Add(newTeam);
            //GridView1.EditIndex = m_Teams.Count();
        }
    }
}