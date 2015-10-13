using System;
using System.Collections.Generic;
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
        // GET: api/Category
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
        #endregion
        
    }
}