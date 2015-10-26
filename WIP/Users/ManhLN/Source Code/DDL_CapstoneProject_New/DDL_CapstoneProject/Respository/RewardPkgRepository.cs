using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Respository
{
    public class RewardPkgRepository : SingletonBase<RewardPkgRepository>
    {

        #region "Constructors"
        private RewardPkgRepository()
        {
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Get rewardPkg of a project by project id
        /// </summary>
        /// <returns>rewardList</returns>
        public List<RewardPkgDTO> GetRewardPkg(int ProjectID)
        {
            using (var db = new DDLDataContext())
            {
                // Get rewardPkg list
                var rewardList = (from RewardPkg in db.RewardPkgs
                                  where RewardPkg.ProjectID == ProjectID
                                  orderby RewardPkg.PledgeAmount ascending
                                  select new RewardPkgDTO()
                                  {
                                      Description = RewardPkg.Description,
                                      PledgeAmount = RewardPkg.PledgeAmount,
                                      EstimatedDelivery = RewardPkg.EstimatedDelivery,
                                      IsHide = RewardPkg.IsHide,
                                      Quantity = RewardPkg.Quantity,
                                      RewardPkgID = RewardPkg.RewardPkgID,
                                      Type = RewardPkg.Type,
                                      CurrentQuantity = RewardPkg.CurrentQuantity,
                                      Backers = db.BackingDetails.Count(t => t.RewardPkgID == RewardPkg.RewardPkgID)
                                  }).ToList();

                rewardList.ForEach(x => x.EstimatedDelivery = CommonUtils.ConvertDateTimeFromUtc(x.EstimatedDelivery.GetValueOrDefault()));

                return rewardList;
            }
        }

        /// <summary>
        /// Get rewardPkg of a project by project code
        /// </summary>
        /// <returns>rewardList</returns>
        public List<RewardPkgDTO> GetRewardPkgByCode(string code)
        {
            using (var db = new DDLDataContext())
            {
                var project = db.Projects.SingleOrDefault(x => x.ProjectCode == code);

                // Get rewardPkg list
                var rewardList = (from RewardPkg in db.RewardPkgs
                                  where RewardPkg.ProjectID == project.ProjectID && RewardPkg.IsHide == false
                                  orderby RewardPkg.PledgeAmount ascending
                                  select new RewardPkgDTO()
                                  {
                                      Description = RewardPkg.Description,
                                      PledgeAmount = RewardPkg.PledgeAmount,
                                      EstimatedDelivery = RewardPkg.EstimatedDelivery,
                                      IsHide = RewardPkg.IsHide,
                                      Quantity = RewardPkg.Quantity,
                                      RewardPkgID = RewardPkg.RewardPkgID,
                                      Type = RewardPkg.Type,
                                      CurrentQuantity = RewardPkg.CurrentQuantity,
                                      Backers = db.BackingDetails.Count(t => t.RewardPkgID == RewardPkg.RewardPkgID)
                                  }).ToList();

                rewardList.ForEach(x => x.EstimatedDelivery = CommonUtils.ConvertDateTimeFromUtc(x.EstimatedDelivery.GetValueOrDefault()));

                return rewardList.ToList();
            }
        }

        /// <summary>
        /// Create a new rewardPkg
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="rewardPkg"></param>
        /// <returns>newRewardPkg</returns>
        public RewardPkgDTO CreateRewardPkg(int ProjectID, RewardPkgDTO rewardPkg)
        {
            using (var db = new DDLDataContext())
            {
                var newRewardPkg = db.RewardPkgs.Create();

                newRewardPkg.ProjectID = ProjectID;
                newRewardPkg.PledgeAmount = rewardPkg.PledgeAmount;
                newRewardPkg.Description = rewardPkg.Description;
                newRewardPkg.EstimatedDelivery = rewardPkg.EstimatedDelivery;
                newRewardPkg.Quantity = rewardPkg.Quantity;
                newRewardPkg.Type = rewardPkg.Type;
                newRewardPkg.IsHide = rewardPkg.IsHide;

                if (newRewardPkg.EstimatedDelivery != null)
                {
                    newRewardPkg.EstimatedDelivery =
                        CommonUtils.ConvertDateTimeToUtc(newRewardPkg.EstimatedDelivery.GetValueOrDefault());
                }

                db.RewardPkgs.Add(newRewardPkg);
                db.SaveChanges();

                var newRewardPkgDTO = new RewardPkgDTO
                {
                    Backers = db.BackingDetails.Count(t => t.RewardPkgID == newRewardPkg.RewardPkgID),
                    PledgeAmount = newRewardPkg.PledgeAmount,
                    Description = newRewardPkg.Description,
                    EstimatedDelivery = newRewardPkg.EstimatedDelivery,
                    Quantity = newRewardPkg.Quantity,
                    Type = newRewardPkg.Type,
                    IsHide = newRewardPkg.IsHide,
                    RewardPkgID = newRewardPkg.RewardPkgID
                };

                if (newRewardPkgDTO.EstimatedDelivery != null)
                {
                    newRewardPkgDTO.EstimatedDelivery = CommonUtils.ConvertDateTimeFromUtc(newRewardPkgDTO.EstimatedDelivery.GetValueOrDefault());
                }

                return newRewardPkgDTO;
            }
        }

        /// <summary>
        /// Edit rewardPkgs
        /// </summary>
        /// <returns></returns>
        public bool EditRewardPkg(RewardPkgDTO rewardPkg)
        {
            using (var db = new DDLDataContext())
            {
                var updateReward = db.RewardPkgs.SingleOrDefault(x => x.RewardPkgID == rewardPkg.RewardPkgID);

                if (updateReward == null)
                {
                    throw new KeyNotFoundException();
                }

                if (updateReward.EstimatedDelivery != null)
                {
                    updateReward.EstimatedDelivery = CommonUtils.ConvertDateTimeToUtc(updateReward.EstimatedDelivery.GetValueOrDefault());
                }

                updateReward.Description = rewardPkg.Description;
                updateReward.EstimatedDelivery = rewardPkg.EstimatedDelivery;
                updateReward.Quantity = rewardPkg.Quantity;
                updateReward.IsHide = rewardPkg.IsHide;
                updateReward.Type = rewardPkg.Type;
                updateReward.PledgeAmount = rewardPkg.PledgeAmount;

                db.SaveChanges();

                return true;
            }
        }

        /// <summary>
        /// Delete a rewardPkg
        /// </summary>
        /// <param name="rewardPkgID"></param>
        /// <returns>boolean</returns>
        public bool DeleteRewardPkg(int rewardPkgID)
        {
            using (var db = new DDLDataContext())
            {
                var deleteRewardPkg = db.RewardPkgs.SingleOrDefault(x => x.RewardPkgID == rewardPkgID);

                if (deleteRewardPkg == null)
                {
                    throw new KeyNotFoundException();
                }

                db.RewardPkgs.Remove(deleteRewardPkg);
                db.SaveChanges();

                return true;
            }
        }

        #endregion
    }
}