using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.IO;

namespace moon_album.Models
{
    public class AlbumService
    {
        public IList<CategoryInfo> GetAllCategory()
        {
            List<CategoryInfo> objCategoryInfo = new List<CategoryInfo>();
            String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "select * from category";
            con.Open();
            OleDbCommand com = new OleDbCommand(query, con);            
            OleDbDataReader dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {

                    CategoryInfo objThisCategoryInfo = new CategoryInfo();

                    objThisCategoryInfo.Category_Id = Convert.ToInt32(dr["CategoryID"]);
                    objThisCategoryInfo.Category_Name = dr["CategoryName"].ToString();
                    objThisCategoryInfo.Category_Description= dr["CategoryContent"].ToString();

                    objCategoryInfo.Add(objThisCategoryInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
                con.Close();
            }
            return objCategoryInfo.ToList();
        }
        public IList<AlbumInfo> GetAllAlbums(int category_id)
        {
            List<AlbumInfo> objAlbumInfo = new List<AlbumInfo>();
            String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "select * from Album where categoryid=@category_id order by AlbumRow,AlbumColumn";
            con.Open();
            OleDbCommand com = new OleDbCommand(query, con);
            com.Parameters.Add(new OleDbParameter("@category_id", category_id));
            OleDbDataReader dr = com.ExecuteReader();
            try
            {
                while (dr.Read())
                {

                    AlbumInfo objThisAlbumInfo = new AlbumInfo();
                    if (dr["albumcontent"].ToString() != "")
                    {
                        objThisAlbumInfo.Album_Description = dr["albumcontent"].ToString();
                    }
                    objThisAlbumInfo.Album_Id = Convert.ToInt32(dr["AlbumID"]);
                    objThisAlbumInfo.Album_Name = dr["AlbumName"].ToString();
                    objThisAlbumInfo.Album_Cover_ImgId = Convert.ToInt32(dr["DefaultPhotID"]);


                    query = "select count(photoid) from PhotAlbum where AlbumID=@albumid";
                    OleDbCommand com2 = new OleDbCommand(query, con);
                    com2.Parameters.Add(new OleDbParameter("@albumid", objThisAlbumInfo.Album_Id));
                    OleDbDataReader dr2 = com2.ExecuteReader();
                    dr2.Read();
                    objThisAlbumInfo.Image_Count = Convert.ToInt32(dr2[0]);
                    dr2.Close();
                    objAlbumInfo.Add(objThisAlbumInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
                con.Close();
            }
            return objAlbumInfo.ToList();
        }

        public IList<ImageInfo> GetAlbumDetails(int album_id)
        {
            List<ImageInfo> objImageInfo = new List<ImageInfo>();
            String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "select * from PhotAlbum where AlbumID=@album_id order by PhotoRow,PhotoColumn";
            con.Open();
            OleDbCommand com = new OleDbCommand(query, con);
            com.Parameters.Add(new OleDbParameter("@album_id", album_id));
            OleDbDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                ImageInfo objThisImageInfo = new ImageInfo();
                objThisImageInfo.Image_Id = Convert.ToInt32(dr["PhotoID"]);
                objThisImageInfo.Image_Name = dr["PhotoName"].ToString();
                objThisImageInfo.File_Name = dr["photo"].ToString();
                objImageInfo.Add(objThisImageInfo);
            }
            return objImageInfo.ToList();
        }

        public string GetCategoryName(int category_id)
        {
            string category_name = "";
            String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "select categoryname from category where categoryid=@category_id";
            con.Open();
            OleDbCommand com = new OleDbCommand(query, con);
            com.Parameters.Add(new OleDbParameter("@category_id", category_id));
            OleDbDataReader dr = com.ExecuteReader();
            dr.Read();
            category_name = dr[0].ToString();
            return category_name;
        }

        public string GetAlbumName(int album_id)
        {
            string album_name = "";
            String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "select albumname from album where albumid=@album_id";
            con.Open();
            OleDbCommand com = new OleDbCommand(query, con);
            com.Parameters.Add(new OleDbParameter("@album_id", album_id));
            OleDbDataReader dr = com.ExecuteReader();
            dr.Read();
            album_name = dr[0].ToString();
            return album_name;
        }

        public int GetMaxPhotoID()
        {
            int max_id;
            String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "select max(PhotoID) from PhotAlbum";
            con.Open();
            OleDbCommand com = new OleDbCommand(query, con);
            OleDbDataReader dr = com.ExecuteReader();
            dr.Read();
            max_id = Convert.ToInt32(dr[0].ToString());
            return max_id;
        }
        public int CreateAlbum(int category_id,string album_name,string album_content,int album_row,int album_column)
        {
            int state = 0;
            try
            {
                String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
                OleDbConnection con = new OleDbConnection(connectionString);
                string query = "Insert into Album (AlbumName,AlbumContent,AlbumRow,AlbumColumn,CategoryID) values (@album_name,@album_content,@album_row,@album_column,@category_id);";
                OleDbCommand com = new OleDbCommand(query, con);
                com.Parameters.Add("@album_name", OleDbType.VarChar, 100).Value = album_name;
                com.Parameters.Add("@album_content", OleDbType.VarChar, 100).Value = album_content;
                com.Parameters.Add("@album_row", OleDbType.Integer).Value = album_row;
                com.Parameters.Add("@album_column", OleDbType.Integer).Value = album_column;
                com.Parameters.Add("@category_id", OleDbType.Integer).Value = category_id;
                con.Open();
                state = com.ExecuteNonQuery();
                con.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return state;
        }
        public int CreatePhoto(int album_id, string photo_name, string file_path, int photo_row, int photo_column)
        {
            int state = 0;
            try
            {
                String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/moon88.mdb");
                OleDbConnection con = new OleDbConnection(connectionString);
                string query = "Insert into PhotAlbum (AlbumID,PhotoRow,PhotoColumn,Photo,PhotoName) values (@album_id,@photo_row,@photo_column,@file_path,@photo_name);";
                OleDbCommand com = new OleDbCommand(query, con);
                com.Parameters.Add("@album_id", OleDbType.VarChar, 100).Value = album_id;
                com.Parameters.Add("@photo_row", OleDbType.Integer).Value = photo_row;
                com.Parameters.Add("@photo_column", OleDbType.Integer).Value = photo_column;
                com.Parameters.Add("@file_path", OleDbType.VarChar, 100).Value = file_path;
                com.Parameters.Add("@photo_name", OleDbType.VarChar, 100).Value = photo_name;
                con.Open();
                state = com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return state;
        }
    }
}