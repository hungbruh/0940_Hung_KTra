using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace De02.Entities
{
    public class SanPham
    {
        public string MaSP { get; set; }     // "SP001"
        public string TenSP { get; set; }
        public DateTime Ngaynhap { get; set; }
        public string MaLoai { get; set; }   // FK to LoaiSP.MaLoai
        // optional: public string TenLoai { get; set; } for display
    }
}
