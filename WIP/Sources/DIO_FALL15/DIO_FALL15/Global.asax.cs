using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DIO_FALL15.Respository;

namespace DIO_FALL15
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public DatabaseContext Db = new DatabaseContext();
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Dynamically create new timer
            System.Timers.Timer timScheduledTask = new System.Timers.Timer();

            // Timer interval is set in miliseconds,
            // In this case, we'll run a task every minute
            timScheduledTask.Interval = 60 * 1000;

            timScheduledTask.Enabled = true;

            // Add handler for Elapsed event
            timScheduledTask.Elapsed +=
            new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);
        }
   
        void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Execute some task
            if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 5)
            {
                // do whatever
                FirstTask();
            }
        }
   
        void FirstTask()
        {
            // Here is the code we need to execute periodically
            var Projects = Db.Projects.Take(5);
            foreach (var p in Projects)
            {
                p.CurrentFund += 100;
            }

            Db.SaveChanges();
        }

    }
}
