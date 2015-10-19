﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Respository
{
    public class TimeLineRepository : SingletonBase<TimeLineRepository>
    {
        private DDLDataContext db;

        #region "Constructors"
        private TimeLineRepository()
        {
            db = new DDLDataContext();
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Get timeline list of a project
        /// </summary>
        /// <returns>timelineList</returns>
        public List<TimeLineDTO> GetTimeLine(int ProjectID)
        {
            // Get rewardPkg list
            var timelineList = from Timeline in db.Timelines
                               where Timeline.ProjectID == ProjectID
                               orderby Timeline.DueDate ascending
                               select new TimeLineDTO()
                               {
                                   ImageUrl = Timeline.ImageUrl,
                                   DueDate = Timeline.DueDate,
                                   Description = Timeline.Description,
                                   Title = Timeline.Title,
                                   TimelineID = Timeline.TimelineID
                               };

            return timelineList.ToList();
        }

        /// <summary>
        /// Create a new timeline point
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="timeline"></param>
        /// <returns>newRewardPkg</returns>
        public TimeLineDTO CreateTimeline(int ProjectID, TimeLineDTO timeline, string img)
        {

            var newTimeline = db.Timelines.Create();
            newTimeline.ProjectID = ProjectID;
            newTimeline.Description = timeline.Description;
            newTimeline.DueDate = timeline.DueDate;
            newTimeline.ImageUrl = img;
            newTimeline.Title = timeline.Title;

            db.Timelines.Add(newTimeline);
            db.SaveChanges();

            var newTimelineDTO = new TimeLineDTO
            {
                Description = newTimeline.Description,
                DueDate = newTimeline.DueDate,
                ImageUrl = newTimeline.ImageUrl,
                Title = newTimeline.Title,
                TimelineID = newTimeline.TimelineID
            };

            return newTimelineDTO;
        }

        /// <summary>
        /// Edit timeline
        /// </summary>
        /// <returns>boolean</returns>
        public bool EditTimeline(TimeLineDTO timeline, string img)
        {
            var updateTimeline = db.Timelines.SingleOrDefault(x => x.TimelineID == timeline.TimelineID);

            if (updateTimeline == null)
            {
                throw new KeyNotFoundException();
            }

            if (img != string.Empty)
            {
                updateTimeline.ImageUrl = img;
            }

            updateTimeline.Description = timeline.Description;
            updateTimeline.DueDate = timeline.DueDate;
            updateTimeline.Title = timeline.Title;

            db.SaveChanges();


            return true;
        }

        /// <summary>
        /// Delete a timeline point
        /// </summary>
        /// <param name="timelineID"></param>
        /// <returns>boolean</returns>
        public bool DeleteTimeline(int timelineID)
        {
            var deleteTimeline = db.Timelines.SingleOrDefault(x => x.TimelineID == timelineID);

            if (deleteTimeline == null)
            {
                throw new KeyNotFoundException();
            }

            db.Timelines.Remove(deleteTimeline);
            db.SaveChanges();

            return true;
        }

        #endregion
    }
}