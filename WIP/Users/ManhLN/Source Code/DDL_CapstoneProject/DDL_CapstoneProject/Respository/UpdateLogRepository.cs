using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Respository
{
    public class UpdateLogRepository : SingletonBase<UpdateLogRepository>
    {
        private DDLDataContext db;

        #region "Constructors"
        private UpdateLogRepository()
        {
            db = new DDLDataContext();
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Get UpdateLog of a project
        /// </summary>
        /// <returns>updateLogList</returns>
        public List<UpdateLogDTO> GetUpdateLog(int ProjectID)
        {
            // Get updatelog list
            var updateLogList = from UpdateLog in db.UpdateLogs
                                where UpdateLog.ProjectID == ProjectID
                                orderby UpdateLog.CreatedDate descending
                                select new UpdateLogDTO()
                                {
                                    Description = UpdateLog.Description,
                                    Title = UpdateLog.Title,
                                    CreatedDate = UpdateLog.CreatedDate,
                                    UpdateLogID = UpdateLog.UpdateLogID,
                                };

            return updateLogList.ToList();
        }

        /// <summary>
        /// Create a new updateLog
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="updateLog"></param>
        /// <returns>newRewardPkg</returns>
        public UpdateLog CreateUpdateLog(int ProjectID, UpdateLogDTO newUpdateLog)
        {
            var updateLog = new UpdateLog
            {
                ProjectID = ProjectID,
                Description = newUpdateLog.Description,
                CreatedDate = newUpdateLog.CreatedDate,
                Title = newUpdateLog.Title
            };
            db.UpdateLogs.Add(updateLog);
            db.SaveChanges();

            return updateLog;
        }

        /// <summary>
        /// Edit updateLog
        /// </summary>
        /// <returns>boolean</returns>
        public bool EditUpdateLog(List<UpdateLogDTO> updateLog)
        {
            foreach (var update in updateLog)
            {
                var editLog = db.UpdateLogs.SingleOrDefault(x => x.UpdateLogID == update.UpdateLogID);

                editLog.Description = update.Description;
                editLog.Title = update.Title;
                editLog.CreatedDate = update.CreatedDate;

                db.SaveChanges();
            }

            return true;
        }

        public bool DeleteUpdateLog(int updateLogID)
        {
            var deleteUpdateLog = db.UpdateLogs.SingleOrDefault(x => x.UpdateLogID == updateLogID);

            if (deleteUpdateLog != null)
            {
                db.UpdateLogs.Remove(deleteUpdateLog);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        #endregion
    }
}