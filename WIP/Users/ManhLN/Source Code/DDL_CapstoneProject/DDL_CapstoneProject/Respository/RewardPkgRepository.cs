using System;
using System.Collections.Generic;
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
        private DDLDataContext db;

        #region "Constructors"
        private RewardPkgRepository()
        {
            db = new DDLDataContext();
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Get rewardPkg of a project
        /// </summary>
        /// <returns>rewardList</returns>
        public List<RewardPkgDTO> GetRewardPkg(int ProjectID)
        {
            // Get rewardPkg list
            var rewardList = from RewardPkg in db.RewardPkgs
                             where RewardPkg.ProjectID == ProjectID
                             orderby RewardPkg.Type ascending
                             select new RewardPkgDTO()
                             {
                                 Description = RewardPkg.Description,
                                 PledgeAmount = RewardPkg.PledgeAmount,
                                 EstimatedDelivery = RewardPkg.EstimatedDelivery,
                                 IsHide = RewardPkg.IsHide,
                                 Quantity = RewardPkg.Quantity,
                                 RewardPkgID = RewardPkg.RewardPkgID,
                                 Type = RewardPkg.Type,
                                 Backers = db.BackingDetails.Count(t => t.RewardPkgID == RewardPkg.RewardPkgID)
                             };

            return rewardList.ToList();
        }

        /// <summary>
        /// Create a new rewardPkg
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="rewardPkg"></param>
        /// <returns>newRewardPkg</returns>
        public RewardPkgDTO CreateRewardPkg(int ProjectID, RewardPkgDTO rewardPkg)
        {
            var newRewarPkg = new RewardPkg
            {
                ProjectID = ProjectID,
                PledgeAmount = rewardPkg.PledgeAmount,
                Description = rewardPkg.Description,
                EstimatedDelivery = rewardPkg.EstimatedDelivery,
                Quantity = rewardPkg.Quantity,
                Type = rewardPkg.Type,
                IsHide = rewardPkg.IsHide
            };
            db.RewardPkgs.Add(newRewarPkg);
            db.SaveChanges();

            var newRewardPkgDTO = new RewardPkgDTO
            {
                Backers = db.BackingDetails.Count(t => t.RewardPkgID == newRewarPkg.RewardPkgID),
                PledgeAmount = newRewarPkg.PledgeAmount,
                Description = newRewarPkg.Description,
                EstimatedDelivery = newRewarPkg.EstimatedDelivery,
                Quantity = newRewarPkg.Quantity,
                Type = newRewarPkg.Type,
                IsHide = newRewarPkg.IsHide
            };

            return newRewardPkgDTO;
        }

        /// <summary>
        /// Edit rewardPkgs
        /// </summary>
        /// <returns></returns>
        public bool EditRewardPkg(List<RewardPkgDTO> rewardPkg)
        {
            foreach (var reward in rewardPkg)
            {
                var updateReward = db.RewardPkgs.SingleOrDefault(x => x.RewardPkgID == reward.RewardPkgID);

                if (updateReward == null)
                {
                    throw new KeyNotFoundException();
                }

                updateReward.Description = reward.Description;
                updateReward.EstimatedDelivery = reward.EstimatedDelivery;
                updateReward.Quantity = reward.Quantity;
                updateReward.IsHide = reward.IsHide;
                updateReward.Type = reward.Type;
                updateReward.PledgeAmount = reward.PledgeAmount;

                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Delete a rewardPkg
        /// </summary>
        /// <param name="rewardPkgID"></param>
        /// <returns>boolean</returns>
        public bool DeleteRewardPkg(int rewardPkgID)
        {
            var deleteRewardPkg = db.RewardPkgs.SingleOrDefault(x => x.RewardPkgID == rewardPkgID);

            if (deleteRewardPkg == null)
            {
                throw new KeyNotFoundException();
            }

            //RewardPkg deleteRewardPkg = new RewardPkg() { RewardPkgID = rewardPkgID };
            //db.RewardPkgs.Attach(deleteRewardPkg);
            //db.RewardPkgs.Remove(deleteRewardPkg);
            //db.SaveChanges();

            db.RewardPkgs.Remove(deleteRewardPkg);
            db.SaveChanges();

            return true;
        }

        #endregion
    }
}