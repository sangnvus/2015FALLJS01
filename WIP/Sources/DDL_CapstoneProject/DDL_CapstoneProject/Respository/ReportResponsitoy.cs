using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Respository
{
    public class ReportResponsitoy : SingletonBase<ReportResponsitoy>
    {
        #region "Constructor"
        private ReportResponsitoy()
        {
        }
        #endregion
        #region method


        public void ReportProject(string userName, string projectCode, string content)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userName));
                var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
                var report = new ReportProject
                {
                    Project = project,
                    Reporter = userCurrent,
                    ProjectID = project.ProjectID,
                    ReportContent = content,
                    ReportedDate = DateTime.Now,
                    ReporterID = userCurrent.DDL_UserID,
                    Status = "unread",
                    Subject = "Report " + project.Title
                };
                db.ReportProjects.Add(report);
                db.SaveChanges();
            }

        }

        public void ReportUser(string userName, string reportedUsername, string content)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userName));
                var reportedUser = db.DDL_Users.FirstOrDefault(x => x.Username == reportedUsername);
                var report = new ReportUser
                {
                    ReporterID = userCurrent.DDL_UserID,
                    ReportedUserID = reportedUser.DDL_UserID,
                    Subject = "Báo xấu " + reportedUser.UserInfo.FullName,
                    ReportContent = content,
                    ReportedDate = DateTime.UtcNow,
                    Status = DDLConstants.ReportStatus.NEW,

                };
                db.ReportUsers.Add(report);
                db.SaveChanges();
            }

        }

        public List<ReportProjectDTO> GetReportProjects()
        {
            using (var db = new DDLDataContext())
            {
                var ReportProjectSet = from ReportProject in db.ReportProjects
                                       orderby ReportProject.ReportedDate descending 
                                       select new ReportProjectDTO
                                       {
                                           ProjectID = ReportProject.ProjectID,
                                           ProjectCode = ReportProject.Project.ProjectCode,
                                           ProjectTitle = ReportProject.Project.Title,
                                           ReportContent = ReportProject.ReportContent,
                                           ReportedDate = ReportProject.ReportedDate,
                                           ReporterUsername = ReportProject.Reporter.Username,
                                           reporterName = ReportProject.Reporter.UserInfo.FullName,
                                           ReportID = ReportProject.ReportID,
                                           Status = ReportProject.Status,
                                           Subject = ReportProject.Subject,
                                           CreatorID = ReportProject.Project.CreatorID,
                                           CreatorName = ReportProject.Project.Creator.UserInfo.FullName,
                                           CreatorUsername = ReportProject.Project.Creator.Username
                                       };
                var a = ReportProjectSet.ToList();
                return ReportProjectSet.ToList();
            }
        }
        public List<ReportUserDTO> GetReportUsers()
        {
            using (var db = new DDLDataContext())
            {
                var ReportUserSet = from ReportUser in db.ReportUsers
                                    orderby ReportUser.ReportedDate descending
                                    select new ReportUserDTO
                                    {
                                        ReportID = ReportUser.ReportID,
                                        ReportContent = ReportUser.ReportContent,
                                        ReportedDate = ReportUser.ReportedDate,
                                        ReportedFullname = ReportUser.ReportedUser.UserInfo.FullName,
                                        ReportedUserID = ReportUser.ReportedUserID,
                                        ReportedUsername = ReportUser.ReportedUser.Username,
                                        ReporterFullname = ReportUser.Reporter.UserInfo.FullName,
                                        ReporterID = ReportUser.ReporterID,
                                        ReporterUsername = ReportUser.Reporter.Username,
                                        Status = ReportUser.Status,
                                        Subject = ReportUser.Subject
                                    };
                var a = ReportUserSet.ToList();
                return ReportUserSet.ToList();
            }
        }


        public void changeReportProjectStatus(int id, string status)
        {
            var stt = DDLConstants.ReportStatus.NEW;
            switch (status)
            {
                case "viewed":
                    stt = DDLConstants.ReportStatus.VIEWED;
                    break;
                case "rejected":
                    stt = DDLConstants.ReportStatus.REJECTED;
                    break;
                case "done":
                    stt = DDLConstants.ReportStatus.DONE;
                    break;
                default:
                    stt = DDLConstants.ReportStatus.NEW;
                    break;

            }

            using (var db = new DDLDataContext())
            {
                var report = db.ReportProjects.FirstOrDefault(x => x.ReportID == id);
                if (report == null) throw new KeyNotFoundException();

                report.Status = stt;
                db.SaveChanges();
            }
        }

        public void changeReportUserStatus(int id, string status)
        {
            var stt = DDLConstants.ReportStatus.NEW;
            switch (status)
            {
                case "viewed":
                    stt = DDLConstants.ReportStatus.VIEWED;
                    break;
                case "rejected":
                    stt = DDLConstants.ReportStatus.REJECTED;
                    break;
                case "done":
                    stt = DDLConstants.ReportStatus.DONE;
                    break;
                default:
                    stt = DDLConstants.ReportStatus.NEW;
                    break;

            }

            using (var db = new DDLDataContext())
            {
                var report = db.ReportUsers.FirstOrDefault(x => x.ReportID == id);
                if (report == null) throw new KeyNotFoundException();

                report.Status = stt;
                db.SaveChanges();

            }
        }
        #endregion
    }
}