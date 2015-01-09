using System;
using System.Security.Policy;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using GamePool2Core.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using GamePoolWeb2.Models;

namespace GamePoolWeb2.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected async void LogIn(object sender, EventArgs e)
        {
            Repository m_Repository = new Repository();
            PasswordHasher hasher = new PasswordHasher();
            string pwdhash = hasher.HashPassword(Password.Text);

            User user = await m_Repository.GetUser(Username.Text);

            if (user == null)
            {
                //notify the user that it doesn't exist
                FailureText.Text = "Invalid user name";
                ErrorMessage.Visible = true;
            }
            else if (hasher.VerifyHashedPassword(user.PasswordHash, Password.Text) == PasswordVerificationResult.Failed)
            {
                FailureText.Text = "Invalid Password";
                ErrorMessage.Visible = true;
            }
            else
            {
                HttpCookie cookie = new HttpCookie("UserName", user.UserName);
                if (RememberMe.Checked)
                    cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);
                FormsAuthentication.SetAuthCookie(user.UserName, true);
                Response.Redirect("~/MyPicks.aspx");
            }

            //if (IsValid)
            //{
            //    // Validate the user password
            //    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            //    // This doen't count login failures towards account lockout
            //    // To enable password failures to trigger lockout, change to shouldLockout: true
            //    var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

            //    switch (result)
            //    {
            //        case SignInStatus.Success:
            //            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            //            break;
            //        case SignInStatus.LockedOut:
            //            Response.Redirect("/Account/Lockout");
            //            break;
            //        case SignInStatus.RequiresVerification:
            //            Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
            //                                            Request.QueryString["ReturnUrl"],
            //                                            RememberMe.Checked),
            //                              true);
            //            break;
            //        case SignInStatus.Failure:
            //        default:
            //            FailureText.Text = "Invalid login attempt";
            //            ErrorMessage.Visible = true;
            //            break;
            //    }
            //}
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
 
    }
}