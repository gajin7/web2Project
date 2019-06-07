namespace WebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApp.Persistence.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Controller"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Controller" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "AppUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppUser" };

                manager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin@yahoo.com"))
            {
                var user = new ApplicationUser() { Id = "admin", UserName = "admin@yahoo.com", Email = "admin@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Admin123!") };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "appu@yahoo.com"))
            { 
                var user = new ApplicationUser() { Id = "appu", UserName = "appu@yahoo.com", Email = "appu@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Appu123!") };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }


            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.TimeTicket))
            {
                var price = new Prices() { price = 179, ticketType = Enums.TicketType.TimeTicket, LineType= Enums.LineTypes.Urban };
                context.Prices.Add(price);
            }
            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.TimeTicket))
            {
                var price = new Prices() { price = 229, ticketType = Enums.TicketType.TimeTicket, LineType = Enums.LineTypes.Suburban };
                context.Prices.Add(price);
            }
            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.MonthlyTicket))
            {
                var price = new Prices() { price = 2990, ticketType = Enums.TicketType.MonthlyTicket, LineType = Enums.LineTypes.Urban };
                context.Prices.Add(price);
            }
            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.MonthlyTicket))
            {
                var price = new Prices() { price = 3490, ticketType = Enums.TicketType.MonthlyTicket, LineType = Enums.LineTypes.Suburban };
                context.Prices.Add(price);
            }
            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.AnnualTicket))
            {
                var price = new Prices() { price = 25502, ticketType = Enums.TicketType.AnnualTicket, LineType = Enums.LineTypes.Urban };
                context.Prices.Add(price);
            }
            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.AnnualTicket))
            {
                var price = new Prices() { price = 28704, ticketType = Enums.TicketType.AnnualTicket, LineType = Enums.LineTypes.Suburban };
                context.Prices.Add(price);
            }
            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.DailyTicket))
            {
                var price = new Prices() { price = 499, ticketType = Enums.TicketType.DailyTicket, LineType = Enums.LineTypes.Urban };
                context.Prices.Add(price);
            }
            if (!context.Prices.Any(u => u.ticketType == Enums.TicketType.DailyTicket))
            {
                var price = new Prices() { price = 625, ticketType = Enums.TicketType.DailyTicket, LineType = Enums.LineTypes.Suburban };
                context.Prices.Add(price);
            }


            if (!context.Lines.Any(u => u.Name == "1"))
            {
                var line = new Line() { Name = "1", LineType = Enums.LineTypes.Urban };
                context.Lines.Add(line);
            }
        }
    }
}
