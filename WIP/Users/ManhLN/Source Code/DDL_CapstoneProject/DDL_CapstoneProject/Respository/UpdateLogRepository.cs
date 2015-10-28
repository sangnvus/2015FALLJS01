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
        #region "Constructors"
        private UpdateLogRepository()
        {

        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Get UpdateLog of a project
        /// </summary>
        /// <returns>updateLogList</returns>
        public List<UpdateLogDTO> GetUpdateLog(int ProjectID)
        {
            using (var db = new DDLDataContext())
            {
                // Get updatelog list
                var updateLogList = (from UpdateLog in db.UpdateLogs
                                     where UpdateLog.ProjectID == ProjectID
                                     orderby UpdateLog.CreatedDate descending
                                     select new UpdateLogDTO()
                                     {
                                         Description = UpdateLog.Description,
                                         Title = UpdateLog.Title,
                                         CreatedDate = UpdateLog.CreatedDate,
                                         UpdateLogID = UpdateLog.UpdateLogID,
                                     }).ToList();

                updateLogList.ForEach(x => x.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(x.CreatedDate));

                return updateLogList;
            }
        }

        /// <summary>
        /// Create a new updateLog
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="updateLog"></param>
        /// <returns>newRewardPkg</returns>
        public UpdateLogDTO CreateUpdateLog(int ProjectID, UpdateLogDTO newUpdateLog)
        {
            using (var db = new DDLDataContext())
            {
                var updateLog = db.UpdateLogs.Create();
                updateLog.ProjectID = ProjectID;
                updateLog.Description = newUpdateLog.Description;
                updateLog.CreatedDate = DateTime.UtcNow;
                updateLog.Title = newUpdateLog.Title;

                db.UpdateLogs.Add(updateLog);
                db.SaveChanges();

                var updateLogDTO = new UpdateLogDTO
                {
                    Description = updateLog.Description,
                    Title = updateLog.Title,
                    CreatedDate = updateLog.CreatedDate,
                    UpdateLogID = updateLog.UpdateLogID
                };

                updateLogDTO.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(updateLogDTO.CreatedDate);

                return updateLogDTO;
            }
        }

        /// <summary>
        /// Edit updateLog
        /// </summary>
        /// <returns>boolean</returns>
        public bool EditUpdateLog(List<UpdateLogDTO> updateLog)
        {
            using (var db = new DDLDataContext())
            {
                foreach (var update in updateLog)
                {
                    var editLog = db.UpdateLogs.SingleOrDefault(x => x.UpdateLogID == update.UpdateLogID);

                    if (editLog == null)
                    {
                        throw new KeyNotFoundException();
                    }

                    editLog.Description = update.Description;
                    editLog.Title = update.Title;
                    editLog.CreatedDate = update.CreatedDate;

                    editLog.CreatedDate = CommonUtils.ConvertDateTimeToUtc(editLog.CreatedDate);

                    db.SaveChanges();
                }

                return true;
            }
        }

        public bool DeleteUpdateLog(int updateLogID)
        {
            using (var db = new DDLDataContext())
            {
                var deleteUpdateLog = db.UpdateLogs.SingleOrDefault(x => x.UpdateLogID == updateLogID);

                if (deleteUpdateLog == null)
                {
                    throw new KeyNotFoundException();
                }

                db.UpdateLogs.Remove(deleteUpdateLog);
                db.SaveChanges();

                return true;
            }
        }

        #endregion
    }
}