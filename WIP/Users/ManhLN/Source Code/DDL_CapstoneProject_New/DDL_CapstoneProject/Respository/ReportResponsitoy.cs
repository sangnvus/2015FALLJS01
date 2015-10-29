using DDL_CapstoneProject.Helpers;
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
        public List<ReportProjectDTO> GetReportProjects()
        {
            using (var db = new DDLDataContext())
            {
                var ReportProjectSet = from ReportProject in db.ReportProjects
                                       select new ReportProjectDTO
                                       {
                                           ProjectID = ReportProject.ProjectID,
                                           ProjectTitle = ReportProject.Project.Title,
                                           ReportContent = ReportProject.ReportContent,
                                           ReportedDate = ReportProject.ReportedDate.Day + "/" + ReportProject.ReportedDate.Month + "/" + ReportProject.ReportedDate.Year,
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
                                    select new ReportUserDTO
                                    {
                                        ReportID = ReportUser.ReportID,
                                        ReportContent = ReportUser.ReportContent,
                                        ReportedDate = ReportUser.ReportedDate.Day + "/" + ReportUser.ReportedDate.Month + "/" + ReportUser.ReportedDate.Year,
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


        public void changeReportStatus(int id, string status, string reportType)
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
                if (reportType == "Project")
                {
                    var report = db.ReportProjects.FirstOrDefault(x => x.ReportID == id);
                    if (report == null) throw new KeyNotFoundException();

                    report.Status = stt;
                    db.SaveChanges();
                }
                else
                {
                    var report = db.ReportUsers.FirstOrDefault(x => x.ReportID == id);
                    if (report == null) throw new KeyNotFoundException();

                    report.Status = stt;
                    db.SaveChanges();
                }

            }
        }

        #endregion
    }
}