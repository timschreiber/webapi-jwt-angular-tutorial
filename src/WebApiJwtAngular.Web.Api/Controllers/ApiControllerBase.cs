using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web.Http;
using WebApiJwtAngular.Data;
using WebApiJwtAngular.Data.Entities;

namespace WebApiJwtAngular.Web.Api.Controllers
{
    public class ApiControllerBase : ApiController
    {
        private bool _disposed = false;
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        private ApplicationUser _authenticatedUser;

        protected ApplicationDbContext Context
        {
            get { return _context ?? (_context = Request.GetOwinContext().Get<ApplicationDbContext>()); }
        }

        protected ApplicationUserManager UserManager
        {
            get { return _userManager ?? (_userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>()); }
        }

        protected ApplicationUser AuthenticatedUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;

                if (_authenticatedUser == null)
                    _authenticatedUser = Context.Users.Find(User.Identity.GetUserId());

                return _authenticatedUser;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing && !_disposed)
            {
                if (_context != null)
                    _context.Dispose();

                if (_userManager != null)
                    _userManager.Dispose();

                base.Dispose(disposing);

                _disposed = true;
            }
        }
    }
}