using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GamePool2Core.Entities;
using System.Threading.Tasks;

namespace GamePoolWeb2
{
    public partial class Leaderboard : System.Web.UI.Page
    {
        private List<User> m_PoolUsers;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["m_PoolUsers"] != null)
            {
                m_PoolUsers = (List<User>)ViewState["m_PoolUsers"];
            }
            else
            {
                await LoadData();

            }

        }

        private async Task LoadData()
        {
            Repository m_Repository = new Repository();
            //User user = await m_Repository.GetUser("creasewp");

            IList<User> poolUsers = await m_Repository.GetPoolUsers(string.Empty);//TODO                

            m_PoolUsers = new List<User>();
            foreach (User poolUser in poolUsers.OrderByDescending(item => item.PoolScore))
            {
                m_PoolUsers.Add(poolUser);
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            GridView1.DataSource = m_PoolUsers;
            GridView1.DataBind();

            ViewState.Add("m_PoolUsers", m_PoolUsers);

            base.OnPreRender(e);
        }

        protected async void RefreshButton_Click(object sender, EventArgs e)
        {
            await LoadData();
        }
    }
}