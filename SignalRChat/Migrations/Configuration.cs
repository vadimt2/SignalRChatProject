namespace SignalRChat.Migrations
{
    using Common;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.SignalRChat.DB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.SignalRChat.DB context)
        {
            var vadim = new User("vadim", "1234");
            var yana = new User("yana", "1234");
            var test = new User("test", "1234");

            context.Users.Add(vadim);
            context.Users.Add(yana);
            context.Users.Add(test);
            context.SaveChanges();
        }
    }
}
