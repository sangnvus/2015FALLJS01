﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;

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
            var dateTimeNow = CommonUtils.DateTimeNowGMT7();
            // Execute task: Check expire project
            if (dateTimeNow.Hour == 23 && dateTimeNow.Minute == 50)
            {
                // Check expire project
                DetechExpiredProject();
                // Check pending project
                DetechPendingProject();
            }

            // Execute task: Caculate popular point
            if (dateTimeNow.Hour == 23 && dateTimeNow.Minute == 55)
            {
                // Caculate popular point
                CaculatePopularProject();
            }

            // Test timer
            //Test();
        }

        // Test timer PRJ1
        protected void Test()
        {
            using (var db = new DDLDataContext())
            {
                var project = db.Projects.SingleOrDefault(x => x.ProjectID == 1);

                if (project == null)
                {
                    throw new KeyNotFoundException();
                }

                project.PopularPoint += 10;

                db.SaveChanges();
            }
        }

        // Check expire project
        protected void DetechExpiredProject()
        {
            using (var db = new DDLDataContext())
            {
                var projects = db.Projects.Where(x => x.IsExprired == false && x.Status != DDLConstants.ProjectStatus.DRAFT).ToList();

                foreach (var project in projects)
                {
                    if (project.ExpireDate.Value.Year == DateTime.UtcNow.Year && project.ExpireDate.Value.Day == DateTime.UtcNow.Day
                        && project.ExpireDate.Value.Month == DateTime.UtcNow.Month)
                    {
                        project.IsExprired = true;
                        //project.Status = DDLConstants.ProjectStatus.EXPIRED;
                    }

                    db.SaveChanges();
                }
            }
        }

        // Check pending project
        protected void DetechPendingProject()
        {
            using (var db = new DDLDataContext())
            {
                var projects = db.Projects.Where(x => x.Status == DDLConstants.ProjectStatus.PENDING).ToList();

                foreach (var project in projects)
                {
                    var timespan = DateTime.UtcNow - project.CreatedDate;
                    if (timespan.Days > 3)
                    {
                        var detectedProject = new ProjectEditDTO
                        {
                            ProjectID = project.ProjectID,
                            Status = DDLConstants.ProjectStatus.REJECTED
                        };
                        var result = ProjectRepository.Instance.AdminChangeProjectStatus(detectedProject, "admin001");
                    }

                    //db.SaveChanges();
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
                    if (project.PopularPoint != 0)
                    {
                        project.PopularPoint = project.PopularPoint / 2;
                    }

                    project.PopularPoint += project.PointOfTheDay;
                    project.PointOfTheDay = 0;
                }

                //// Get all backing of the day
                //var backing =
                //    db.Backings.Where(
                //        x =>
                //            x.BackedDate.Year == DateTime.UtcNow.Year && x.BackedDate.Day == DateTime.UtcNow.Day &&
                //            x.BackedDate.Month == DateTime.UtcNow.Month).ToList();
                //foreach (var back in backing)
                //{
                //    var project = projects.SingleOrDefault(x => x.ProjectID == back.ProjectID);

                //    if (project == null)
                //    {
                //        throw new KeyNotFoundException();
                //    }
                //    project.PopularPoint += 10;
                //}

                //// Get all comment of the day
                //var comment =
                //   db.Comments.Where(
                //       x =>
                //           x.CreatedDate.Year == DateTime.UtcNow.Year && x.CreatedDate.Day == DateTime.UtcNow.Day &&
                //           x.CreatedDate.Month == DateTime.UtcNow.Month).ToList();
                //foreach (var cmt in comment)
                //{
                //    var project = projects.SingleOrDefault(x => x.ProjectID == cmt.ProjectID && cmt.UserID != x.CreatorID);

                //    if (project == null)
                //    {
                //        throw new KeyNotFoundException();
                //    }
                //    project.PopularPoint += 2;
                //}

                db.SaveChanges();
            }
        }
    }
}
