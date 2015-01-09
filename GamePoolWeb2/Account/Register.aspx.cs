using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using GamePool2Core.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using GamePoolWeb2.Models;

namespace GamePoolWeb2.Account
{
    public partial class Register : Page
    {
        bool m_IsLocked = false;
        protected async void Page_Load(object sender, EventArgs e)
        {
            Repository m_Repository = new Repository();
            Setting setting = await m_Repository.GetSetting("IsLocked");
            bool.TryParse(setting.Value, out m_IsLocked);

            if (m_IsLocked)
                Response.Redirect("Locked.aspx");
        }

        public bool IsLocked
        {
            get { return m_IsLocked; }

        }

        protected async void CreateUser_Click(object sender, EventArgs e)
        {
            Repository m_Repository = new Repository();

            PasswordHasher hasher = new PasswordHasher();
            string pwdhash = hasher.HashPassword(Password.Text);
            User user = await m_Repository.CreateUser(Username.Text, Email.Text, pwdhash);

            if (user == null)
            {
                //notify the user that this user already exists
                ErrorMessage.Text = "User already exists";
            }
            else
            {
                HttpCookie cookie = new HttpCookie("UserName", user.UserName);
                //cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);
                Response.Redirect("~/MyPicks.aspx");
            }



            //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            //IdentityResult result = manager.Create(user, Password.Text);
            //if (result.Succeeded)
            //{
            //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //    //string code = manager.GenerateEmailConfirmationToken(user.Id);
            //    //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
            //    //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

            //    IdentityHelper.SignIn(manager, user, isPersistent: false);
            //    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            //}
            //else 
            //{
            //    ErrorMessage.Text = result.Errors.FirstOrDefault();
            //}
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
 
    }
}