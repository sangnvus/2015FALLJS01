using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;

namespace DDL_CapstoneProject.Respository
{
    public class SlideRepository : SingletonBase<SlideRepository>
    {
        private DDLDataContext db;

        private SlideRepository()
        {
            db = new DDLDataContext();
        }
    }
}