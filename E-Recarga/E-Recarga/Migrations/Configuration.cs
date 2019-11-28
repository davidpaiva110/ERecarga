namespace E_Recarga.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<E_Recarga.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(E_Recarga.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] roleNames = { "Admin", "Rede Proprietaria", "Utilizador" };

            foreach (var roles in roleNames)
            {
                if (!roleManager.RoleExists(roles))
                {
                    var role = new IdentityRole();
                    role.Name = roles;
                    roleManager.Create(role);
                }
            }

            base.Seed(context);
        }
    }
}
