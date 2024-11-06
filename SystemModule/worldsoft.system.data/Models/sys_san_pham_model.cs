using System.Collections.Generic;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_san_pham_model
    {
        public sys_san_pham_model()
        {
            db = new sys_san_pham_db();
            list_file = new List<sys_san_pham_file_upload_model>();
        }
        public string khuyen_mai { get; set; }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string image { get; set; }
        public int? so_luong { get; set; }
        public string ten_loai { get; set; }
        public sys_san_pham_db db { get; set; }
        public List<sys_san_pham_file_upload_model> list_file { get; set; }
        public string ten_san_pham_language { get; set; }
        public string thong_so_ky_thuat_language { get; set; }
        public string mo_ta_language { get; set; }
        public string thong_tin_bo_sung_language { get; set; }
        public string thong_so_ky_thuat_language_mobile { get; set; }
        public string mo_ta_language_mobile { get; set; }
        public string thong_tin_bo_sung_language_mobile { get; set; }
    }
}
