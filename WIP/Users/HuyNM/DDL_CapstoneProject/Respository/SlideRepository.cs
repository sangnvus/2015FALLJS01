using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;

namespace DDL_CapstoneProject.Respository
{
    public class SlideRepository : SingletonBase<SlideRepository>
    {
        private DDLDataContext db;

        private SlideRepository()
        {
            db = new DDLDataContext();
        }


        // GET: api/Slides
        public List<Slide> GetSlides()
        {   
            return db.Slides.Where(x => x.IsActive).OrderBy(x => x.Order).ToList();
        }

    }
}