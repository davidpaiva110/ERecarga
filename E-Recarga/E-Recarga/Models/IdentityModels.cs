using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace E_Recarga.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RedeProprietaria> RedesProprietarias { get; set; }
        public DbSet<Estacao> Estacoes { get; set; }
        public DbSet<Posto> Postos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }

        public class ApplicationRoleManager : RoleManager<IdentityRole>
        {
            public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore) { }

            public static ApplicationRoleManager Create(
                IdentityFactoryOptions<ApplicationRoleManager> options,
                IOwinContext context)
            {
                var manager = new ApplicationRoleManager(
                    new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
                return manager;
            }
        }
    }
}