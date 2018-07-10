using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Net.Http;
using System.Web.Http;
using WebApiJwtAngular.Data.Entities;
using WebApiJwtAngular.Web.Api.Models.Account;
using WebApiJwtAngular.Web.Api.Util;

namespace WebApiJwtAngular.Web.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiControllerBase
    {
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [PostRoute("logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [PostRoute("password")]
        public IHttpActionResult ChangePassword(ChangePasswordApiModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = UserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

                    if (!result.Succeeded)
                        return getErrorResult(result);

                    return Ok();
                }

                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [AllowAnonymous]
        [PostRoute("register")]
        public IHttpActionResult Register(RegisterApiModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                    var result = UserManager.Create(user, model.Password);

                    if(!result.Succeeded)
                    {
                        return getErrorResult(result);
                    }

                    return Ok();
                }

                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult getErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if(!result.Succeeded)
            {
                if(result.Errors != null)
                {
                    foreach(string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if(ModelState.IsValid)
                {
                    return BadRequest(); // No ModelState errors are available, so return empty BadRequest
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
