using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using moon_album.Models;

namespace moon_album.Controllers
{
    public class HomeController : Controller
    {
        AlbumService objAlbumService = new AlbumService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Category(int category_id)
        {
            ViewBag.Title = objAlbumService.GetCategoryName(category_id);
            var All_Album = objAlbumService.GetAllAlbums(category_id);
            return View(All_Album);
        }

        [HttpGet]
        public ActionResult Album(int category_id,int id)
        {
            ViewBag.Category_id = category_id;
            ViewBag.Title = objAlbumService.GetAlbumName(id);
            var All_Photo = objAlbumService.GetAlbumDetails(id);
            return View(All_Photo);
        }

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}