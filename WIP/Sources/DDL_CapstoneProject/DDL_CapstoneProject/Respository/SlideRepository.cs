using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using System.Diagnostics;

namespace DDL_CapstoneProject.Respository
{
    public class SlideRepository : SingletonBase<SlideRepository>
    {

        private SlideRepository()
        {
        }

        // GET: api/Slides
        public List<SlideDTO> GetSlides()
        {
            using (var db = new DDLDataContext())
            {
                try
                {
                    var SlideList = from slide in db.Slides
                        where slide.IsActive
                        orderby slide.Order
                        select new SlideDTO
                        {
                            Title = slide.Title,
                            ButtonColor = slide.ButtonColor,
                            SlideUrl = slide.SlideUrl,
                            ImageUrl = slide.ImageUrl,
                            ButtonText = slide.ButtonText,
                            Description = slide.Description,
                            TextColor = slide.TextColor,
                            VideoUrl = slide.VideoUrl,
                        };

                    return SlideList.ToList();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return new List<SlideDTO>();
            }
        }

    }
}