using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using moon_album.Models;

namespace moon_album.Controllers
{
    public class AdminController : Controller
    {
        AlbumService objAlbumService = new AlbumService();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/CreateAlbum
        public ActionResult CreateAlbum()
        {
            var All_Category = objAlbumService.GetAllCategory();
            return View(All_Category);
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult CreateAlbum(FormCollection collection)
        {
            var All_Category = objAlbumService.GetAllCategory();
            try
            {
                // TODO: Add insert logic here
                int state = 0;

                state = objAlbumService.CreateAlbum(Convert.ToInt32(collection["CategoryId"]), collection["AlbumName"], collection["AlbumContent"], Convert.ToInt32(collection["AlbumRow"]), Convert.ToInt32(collection["AlbumColumn"]));
                ViewBag.Status = string.Format("Add {0} Record.", state.ToString());
                if (state > 0)
                {
                    return RedirectToAction("../Home/Category/" + collection["CategoryId"]);
                }
                else
                {
                    return View(All_Category);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = ex.ToString();

                return View(All_Category);
            }
        }
        [HttpPost]
        public ActionResult MyUpload(FormCollection collection)
        {
            HttpPostedFileBase myFiles = Request.Files["myFiles"];
            
            return Content("failed");
        }
        public ActionResult InsertImage(int category_id)
        {
            var All_Album = objAlbumService.GetAllAlbums(category_id);
            ViewBag.CategoryName = objAlbumService.GetCategoryName(category_id);
            return View(All_Album);
        }
        [HttpPost]
        public ActionResult InsertImage(int category_id, FormCollection collection)
        {
            var All_Album = objAlbumService.GetAllAlbums(category_id);
            if (Request.Files.Count != 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string fileNameWithOutExt = Path.GetFileNameWithoutExtension(fileName);
                    string fileExt = Path.GetExtension(fileName);
                    bool isIncludeRowColumn = Convert.ToBoolean(collection["isFilenameIncludeRowColumn"].Split(',')[0]);
                    string photoRow = "0";
                    string photoColumn = "0";
                    if (isIncludeRowColumn)
                    {
                        string row_column = fileNameWithOutExt.Split(' ')[0];
                        photoRow = row_column.Split('-')[0];
                        photoColumn = row_column.Split('-')[1];
                        fileNameWithOutExt = fileNameWithOutExt.Split(' ')[1];
                    }
                    try
                    {
                        // TODO: Add insert logic here
                        int state = 0;

                        state = objAlbumService.CreatePhoto(Convert.ToInt32(collection["AlbumId"]), fileNameWithOutExt, fileName, Convert.ToInt32(photoRow), Convert.ToInt32(photoColumn));
                        ViewBag.Status += string.Format("Add {0}. \r\n", fileName.ToString());
                        if (state > 0)
                        {
                            int max_id = objAlbumService.GetMaxPhotoID();
                            string new_file_name = string.Format("{0}{1}", max_id, fileExt);
                            bool folder_exists = Directory.Exists(Server.MapPath(string.Format("~/images/upload/{0}/{1}/", category_id, collection["AlbumId"].ToString())));
                            if (!folder_exists)
                                Directory.CreateDirectory(Server.MapPath(string.Format("~/images/upload/{0}/{1}/", category_id, collection["AlbumId"].ToString())));
                            file.SaveAs(Server.MapPath(string.Format("~/images/upload/{0}/{1}/{2}", category_id, collection["AlbumId"].ToString(), new_file_name)));
                            //return RedirectToAction(string.Format("../Home/Album/{0}/{1}", category_id, collection["AlbumId"]));
                            
                        }
                        else
                        {
                            return View(All_Album);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Status = ex.ToString();

                        return View(All_Album);
                    }

                    

                }
                
            }
            return View(All_Album);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
