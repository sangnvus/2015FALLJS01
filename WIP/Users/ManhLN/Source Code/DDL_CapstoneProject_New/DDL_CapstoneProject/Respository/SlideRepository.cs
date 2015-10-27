using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using System.Diagnostics;
using DDL_CapstoneProject.Ultilities;

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
        public List<SlideDTO> GetSlidesForAdmin()
        {
            using (var db = new DDLDataContext())
            {
                var listSlides = (from slide in db.Slides
                                  orderby slide.Order ascending
                                  select new SlideDTO
                                  {
                                      SlideID = slide.SlideID,
                                      Order = slide.Order,
                                      Title = slide.Title,
                                      Description = slide.Description,
                                      ImageUrl = slide.ImageUrl,
                                      ButtonText = slide.ButtonText,
                                      ButtonColor = slide.ButtonColor,
                                      TextColor = slide.TextColor,
                                      SlideUrl = slide.SlideUrl,
                                      VideoUrl = slide.VideoUrl,
                                      IsActive = slide.IsActive
                                  }).ToList();

                return listSlides;
            }
        }

        public SlideDTO CreateSlide(SlideDTO newSlideDTO)
        {
            using (var db = new DDLDataContext())
            {
                // create new record.
                var newSlide = db.Slides.Create();
                newSlide.Title = newSlideDTO.Title;
                newSlide.Description = newSlideDTO.Description;
                newSlide.SlideUrl = newSlideDTO.SlideUrl;
                newSlide.ButtonColor = newSlideDTO.ButtonColor;
                newSlide.ButtonText = newSlideDTO.ButtonText;
                newSlide.ImageUrl = newSlideDTO.ImageUrl;
                newSlide.SlideUrl = newSlideDTO.SlideUrl;
                newSlide.TextColor = newSlideDTO.TextColor;
                newSlide.VideoUrl = newSlideDTO.VideoUrl;
                newSlide.CreatedDate = DateTime.UtcNow;
                newSlide.IsActive = false;

                // Get order max.
                var orderMax = db.Slides.Max(s => s.Order);
                newSlide.Order = orderMax + 1;

                db.Slides.Add(newSlide);
                db.SaveChanges();

                newSlideDTO.SlideID = newSlide.SlideID;
                newSlideDTO.Order = newSlide.Order;
                newSlideDTO.IsActive = newSlide.IsActive;

                return newSlideDTO;
            }
        }

        public SlideDTO EditSlide(SlideDTO slideDTO)
        {
            using (var db = new DDLDataContext())
            {
                var slide = db.Slides.FirstOrDefault(x => x.SlideID == slideDTO.SlideID);
                if (slide == null)
                {
                    throw new KeyNotFoundException();
                }

                // create new record.
                slide.Title = slideDTO.Title;
                slide.Description = slideDTO.Description;
                slide.SlideUrl = slideDTO.SlideUrl;
                slide.ButtonColor = slideDTO.ButtonColor;
                slide.ButtonText = slideDTO.ButtonText;
                slide.ImageUrl = slideDTO.ImageUrl.Equals(String.Empty) ? slide.ImageUrl : slideDTO.ImageUrl;
                slide.SlideUrl = slideDTO.SlideUrl;
                slide.TextColor = slideDTO.TextColor;
                slide.VideoUrl = slideDTO.VideoUrl;

                db.Slides.AddOrUpdate(slide);
                db.SaveChanges();

                slideDTO.SlideID = slide.SlideID;
                slideDTO.Order = slide.Order;
                slideDTO.IsActive = slide.IsActive;
                slideDTO.ImageUrl = slide.ImageUrl;

                return slideDTO;
            }
        }

        public List<SlideDTO> DeleteSlide(int id)
        {
            using (var db = new DDLDataContext())
            {
                var slide = db.Slides.FirstOrDefault(x => x.SlideID == id);
                if (slide == null)
                {
                    throw new KeyNotFoundException();
                }

                db.Slides.Remove(slide);
                CommonUtils.DeleteFile(slide.ImageUrl, DDLConstants.FileType.SLIDE);
                db.SaveChanges();

                var listslide = db.Slides.ToList();
                for (int i = 0; i < listslide.Count(); i++)
                {
                    listslide[i].Order = i + 1;
                }

                listslide.ForEach(x => db.Slides.AddOrUpdate(x));
                db.SaveChanges();

                var slides = GetSlidesForAdmin();
                return slides;
            }
        }

        public SlideDTO ChangeSlideStatus(int id)
        {
            using (var db = new DDLDataContext())
            {
                var slide = db.Slides.FirstOrDefault(x => x.SlideID == id);
                if (slide == null) throw new KeyNotFoundException();

                slide.IsActive = !slide.IsActive;
                db.SaveChanges();
                var slideDto = new SlideDTO
                {
                    SlideID = slide.SlideID,
                    IsActive = slide.IsActive,
                    Description = slide.Description,
                    Title = slide.Title,
                    Order = slide.Order,
                    ImageUrl = slide.ImageUrl,
                    VideoUrl = slide.VideoUrl,
                    SlideUrl = slide.SlideUrl,
                    ButtonText = slide.ButtonText,
                    TextColor = slide.TextColor,
                    ButtonColor = slide.ButtonColor
                };

                return slideDto;
            }
        }

        public List<SlideDTO> ChangeOrder(int id, string type)
        {
            using (var db = new DDLDataContext())
            {
                var slide = db.Slides.FirstOrDefault(x => x.SlideID == id);
                if (slide == null) throw new KeyNotFoundException();

                var swapSlide = type.Equals("up", StringComparison.OrdinalIgnoreCase)
                    ? db.Slides.FirstOrDefault(x => x.Order == slide.Order - 1)
                    : db.Slides.FirstOrDefault(x => x.Order == slide.Order + 1);

                if (swapSlide == null) throw new KeyNotFoundException();

                int tempData = slide.Order;
                slide.Order = swapSlide.Order;
                swapSlide.Order = tempData;

                db.Slides.AddOrUpdate(slide);
                db.Slides.AddOrUpdate(swapSlide);
                db.SaveChanges();

                return GetSlidesForAdmin();
            }
        }

    }
}