using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 縱火0709.Models.ViewModel
{
    public class ImgViewModel
    {
        public ImgClass Img { get; set; }
        public HttpPostedFileBase file { get; set; }
        public IEnumerable<HttpPostedFileBase> files { get; set; }
    }
}