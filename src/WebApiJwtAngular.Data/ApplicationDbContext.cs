using Microsoft.AspNet.Identity.EntityFramework;
using WebApiJwtAngular.Data.Entities;

namespace WebApiJwtAngular.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
