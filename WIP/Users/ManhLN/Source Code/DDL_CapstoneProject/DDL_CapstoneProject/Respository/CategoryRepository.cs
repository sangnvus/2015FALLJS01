using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Respository
{
    public class CategoryRepository : SingletonBase<CategoryRepository>
    {

        #region "Constructor"
        private CategoryRepository()
        {
        }
        #endregion

        #region "Methods"


        //TrungVn


        public List<CategoryProjectCountDTO> GetCategoryProjectCount()
        {
            using (var db = new DDLDataContext())
            {
                var CategoryList = new List<CategoryProjectCountDTO>();
                var CategorySet = from category in db.Categories
                                  where category.IsActive
                                  select new CategoryProjectCountDTO
                                  {
                                      CategoryID = category.CategoryID,
                                      Name = category.Name,
                                      projectCount = (from project in category.Projects
                                                      where !project.IsExprired
                                                            && !project.Status.Equals(DDLConstants.ProjectStatus.DRAFT) && !project.Status.Equals(DDLConstants.ProjectStatus.REJECTED)
                                                            && !project.Status.Equals(DDLConstants.ProjectStatus.SUSPENDED) && !project.Status.Equals(DDLConstants.ProjectStatus.PENDING)
                                                      select project.ProjectID).Count()

                                  };
                try
                {

                    if (CategorySet.Any())
                    {
                        CategoryList = CategorySet.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
                return CategoryList;
            }
        }

        public List<CategoryProjectCountDTO> GetCategoryProjectStatistic()
        {
            using (var db = new DDLDataContext())
            {
                var CategorySet = from category in db.Categories
                                  select new CategoryProjectCountDTO
                                  {
                                      CategoryID = category.CategoryID,
                                      Name = category.Name,
                                      CategoryFailFunded =
                                          category.Projects.Where(x => x.IsExprired && !x.IsFunded)
                                              .Sum(x => (decimal?)x.CurrentFunded) ?? 0,
                                      CategorySuccessFunded =
                                          category.Projects.Where(x => x.IsExprired && x.IsFunded)
                                              .Sum(x => (decimal?)x.CurrentFunded) ?? 0,
                                  };
                return CategorySet.ToList();
            }
        }


        // GET: api/Category
        public List<CategoryProjectCountDTO> GetAllCategories()
        {
            using (var db = new DDLDataContext())
            {
                // Get rewardPkg list
                var listCategoryDTO = (from category in db.Categories
                                       where category.IsActive
                                       select new CategoryProjectCountDTO
                                       {
                                           CategoryID = category.CategoryID,
                                           Name = category.Name,
                                       }).ToList();
                return listCategoryDTO;
            }
        }
        public Dictionary<string, List<CategoryProjectCountDTO>> listDataForStatistic()
        {
            Dictionary<string, List<CategoryProjectCountDTO>> dic = new Dictionary<string, List<CategoryProjectCountDTO>>();
            dic.Add("GetAllCategories", GetAllCategories());
            dic.Add("GetCategoryProjectStatistic", GetCategoryProjectStatistic());
            return dic;
        }
        //endTrungVN


        // HuyNM
        /// <summary>
        /// Get categories for admin
        /// </summary>
        /// <returns>listCategoryDTO</returns>
        public List<AdminCategoryDTO> GetCategoriesForAdmin()
        {
            using (var db = new DDLDataContext())
            {
                // Get category list

                var listCategoryDTO = (from category in db.Categories
                                       orderby category.CategoryID ascending
                                       select new AdminCategoryDTO
                                       {
                                           IsActive = category.IsActive,
                                           Description = category.Description,
                                           Name = category.Name,
                                           CategoryID = category.CategoryID,
                                           ProjectCount = category.Projects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT && x.Status != DDLConstants.ProjectStatus.REJECTED && x.Status != DDLConstants.ProjectStatus.PENDING)
                                       }).ToList();

                return listCategoryDTO;
            }
        }


        // GET: api/Category
        public List<CategoryDTO> GetCategories()
        {
            using (var db = new DDLDataContext())
            {
                // Get rewardPkg list
                var listCategories = db.Categories.Where(x => x.IsActive).ToList();

                return listCategories.Select(Category => new CategoryDTO
                {
                    CategoryID = Category.CategoryID,
                    Name = Category.Name,
                }).ToList();
            }
        }

        public AdminCategoryDTO ChangeCategoryStatus(int id)
        {
            using (var db = new DDLDataContext())
            {
                var category = db.Categories.FirstOrDefault(x => x.CategoryID == id);
                if (category == null) throw new KeyNotFoundException();

                category.IsActive = !category.IsActive;
                db.SaveChanges();
                var categoryDTO = new AdminCategoryDTO
                {
                    CategoryID = category.CategoryID,
                    IsActive = category.IsActive,
                    Description = category.Description,
                    Name = category.Name,
                    ProjectCount = category.Projects.Count
                };

                return categoryDTO;
            }
        }

        public AdminCategoryDTO AddNewCategory(AdminCategoryDTO category)
        {
            using (var db = new DDLDataContext())
            {
                // Check duplicate.
                if (db.Categories.Any(x => x.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new DuplicateNameException();
                }

                // Create new.
                var newCategory = db.Categories.Create();
                newCategory.Name = category.Name;
                newCategory.Description = category.Description;
                newCategory.IsActive = false;

                db.Categories.Add(newCategory);
                db.SaveChanges();
                // Return data.
                category.IsActive = newCategory.IsActive;
                category.CategoryID = newCategory.CategoryID;

                return category;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (var db = new DDLDataContext())
            {
                var category = db.Categories.FirstOrDefault(x => x.CategoryID == id);
                if (category == null)
                {
                    throw new KeyNotFoundException();
                }

                // Check number projects in category
                // only delete when there aren't any project in this category.
                if (category.Projects.Count > 0)
                {
                    throw new Exception();
                }

                db.Categories.Remove(category);
                db.SaveChanges();

                return true;
            }
        }

        public AdminCategoryDTO EditCategory(AdminCategoryDTO category)
        {
            using (var db = new DDLDataContext())
            {
                var categoryresult = db.Categories.FirstOrDefault(x => x.CategoryID == category.CategoryID);
                if (categoryresult == null)
                {
                    throw new KeyNotFoundException();
                }

                // Edit category.
                categoryresult.Description = category.Description;
                categoryresult.Name = category.Name;

                db.Categories.AddOrUpdate(categoryresult);
                db.SaveChanges();

                // update model.
                category.IsActive = categoryresult.IsActive;
                category.Name = categoryresult.Name;
                category.Description = categoryresult.Description;

                return category;
            }
        }
        #endregion

    }
}