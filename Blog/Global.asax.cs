using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Blog.AutoMapper;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //using (var db = new EntityContext())
            //{
            //    db.Database.Initialize(false);
            //}
            //Database.SetInitializer(new DbInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EntityContext, Repository.Migrations.Configuration>());
            var dbMigrator = new DbMigrator(new Repository.Migrations.Configuration());
            dbMigrator.Update();

            SetUpAutoMapper.RegisterAutoMapper();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
