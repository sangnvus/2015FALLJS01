using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;

namespace DDL_CapstoneProject.Respository
{
    public class ProjectRepository: SingletonBase<ProjectRepository>
    {
        private DDLDataContext db;

        private ProjectRepository()
        {
            db = new DDLDataContext();
        }
    }
}