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
    public partial class Locked : Page
    {
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
 
    }
}