using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;

namespace DDL_CapstoneProject.Respository
{
    public class CategoryRepository : SingletonBase<CategoryRepository>
    {
        private DDLDataContext db;

        private CategoryRepository()
        {
            db = new DDLDataContext();
        }
    }
}