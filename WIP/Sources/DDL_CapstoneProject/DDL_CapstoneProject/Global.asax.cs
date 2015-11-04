using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using DDL_CapstoneProject.Respository;

namespace DDL_CapstoneProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Dynamically create new timer
            System.Timers.Timer timScheduledTask = new System.Timers.Timer();

            // Timer interval is set in miliseconds,
            // Run a task every minute
            timScheduledTask.Interval = 60 * 1000;

            timScheduledTask.Enabled = true;

            // Add handler for Elapsed event
            timScheduledTask.Elapsed +=
            new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);
        }

        protected void Application_BeginRequest()
        {
            if (Request.Path.Equals("/admin", StringComparison.OrdinalIgnoreCase))
            {
                var redirectUrl = VirtualPathUtility.AppendTrailingSlash(Request.Path);
                Response.RedirectPermanent(redirectUrl);
            }
        }

        protected void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Execute task
            if (DateTime.UtcNow.Hour == 23 && DateTime.UtcNow.Minute == 50)
            {
                // Check expire project
                DetechExpiredProject();
                // Caculate popular point
                CaculatePopularProject();
            }
        }

        // Check expire project
        protected void DetechExpiredProject()
        {
            using (var db = new DDLDataContext())
            {
                var projects = db.Projects.Where(x => x.IsExprired == false).ToList();

                foreach (var project in projects)
                {
                    if (project.ExpireDate.Value.Year == DateTime.UtcNow.Year && project.ExpireDate.Value.Day == DateTime.UtcNow.Day
                        && project.ExpireDate.Value.Month == DateTime.UtcNow.Month)
                    {
                        project.IsExprired = true;
                    }

                    db.SaveChanges();
                }
            }
        }

        // Caculate popular point
        protected void CaculatePopularProject()
        {
            using (var db = new DDLDataContext())
            {
                // Get all project
                var projects = db.Projects.Where(x => x.IsExprired == false).ToList();
                foreach (var project in projects)
                {
                    if (project.PopularPoint > 4)
                    {
                        project.PopularPoint = project.PopularPoint - 5;
                    }

                    db.SaveChanges();
                }

                // Get all backing of the day
                var backing =
                    db.Backings.Where(
                        x =>
                            x.BackedDate.Year == DateTime.UtcNow.Year && x.BackedDate.Day == DateTime.UtcNow.Day &&
                            x.BackedDate.Month == DateTime.UtcNow.Month).ToList();
                foreach (var back in backing)
                {
                    var project = db.Projects.SingleOrDefault(x => x.ProjectID == back.ProjectID);

                    if (project == null)
                    {
                        throw new KeyNotFoundException();
                    }
                    project.PopularPoint += 10;

                    db.SaveChanges();
                }

                // Get all comment of the day
                var comment =
                   db.Comments.Where(
                       x =>
                           x.CreatedDate.Year == DateTime.UtcNow.Year && x.CreatedDate.Day == DateTime.UtcNow.Day &&
                           x.CreatedDate.Month == DateTime.UtcNow.Month).ToList();
                foreach (var cmt in comment)
                {
                    var project = db.Projects.SingleOrDefault(x => x.ProjectID == cmt.ProjectID);

                    if (project == null)
                    {
                        throw new KeyNotFoundException();
                    }
                    project.PopularPoint += 5;

                    db.SaveChanges();
                }
            }
        }
    }
}
