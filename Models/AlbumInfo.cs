using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace moon_album.Models
{
    public class AlbumInfo
    {
        public int Album_Id { get; set; }
        public string Album_Name { get; set; }
        public string Album_Description { get; set; }
        public int Album_Cover_ImgId { get; set; }
        public int Image_Count { get; set; }
        
    }
}