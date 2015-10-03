using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;

namespace DDL_CapstoneProject.Respository
{
    public class UserRepository : SingletonBase<UserRepository>
    {
        private DDLDataContext db;

        private UserRepository()
        {
            db = new DDLDataContext();
        }
    }
}