using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;

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
        public List<Category> GetCategories()
        {
            return db.Categories.Where(x => x.IsActive).ToList();
        }
        #endregion
        
    }
}