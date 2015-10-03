using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;

namespace DDL_CapstoneProject.Respository
{
    public class MessageRepository: SingletonBase<MessageRepository>
    {
        private DDLDataContext db;

        private MessageRepository()
        {
            db = new DDLDataContext();
        }
    }
}