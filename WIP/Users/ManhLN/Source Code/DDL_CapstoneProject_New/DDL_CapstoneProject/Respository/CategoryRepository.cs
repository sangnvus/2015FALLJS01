using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;

namespace DDL_CapstoneProject.Respository
{
    public class CategoryRepository : SingletonBase<CategoryRepository>
    {
        private DDLDataContext db;

        #region "Constructor"
        private CategoryRepository()
        {
            db = new DDLDataContext();
        }
        #endregion

        #region "Methods"
        public List<CategoryDTO> GetCategories()
        {
            // Get rewardPkg list
            var listCategories = db.Categories.Where(x => x.IsActive).ToList();

            var listCategoryDTO = new List<CategoryDTO>();

            foreach (var Category in listCategories)
            {
                listCategoryDTO.Add(
                    new CategoryDTO
                    {
                        CategoryID = Category.CategoryID,
                        Name = Category.Name,
                    });
            }
            return listCategoryDTO;
        }
        public List<CategoryProjectCountDTO> GetCategoryProjectCount()
        {
            var CategoryList = new List<CategoryProjectCountDTO>();
            var CategorySet = from category in db.Categories
                              select new CategoryProjectCountDTO
                              {
                                  CategoryID = category.CategoryID,
                                  Name = category.Name,
                                  projectCount = (from pro in category.Projects
                                                  where !pro.IsExprired
                                                  select pro.ProjectID).Count()

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

            }
            return CategoryList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AdminCategoryDTO> GetCategoriesForAdmin()
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
                    ProjectCount = category.Projects.Count
                }).ToList();

            return listCategoryDTO;
        }

        public AdminCategoryDTO ChangeCategoryStatus(int id)
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

        public AdminCategoryDTO AddNewCategory(AdminCategoryDTO category)
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

        public bool DeleteCategory(int id)
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

        public AdminCategoryDTO EditCategory(AdminCategoryDTO category)
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
        #endregion

    }
}